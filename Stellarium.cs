using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OriginLiveView
{
    public partial class Stellarium : Form
    {
        private TcpListener listener10001; // Original server for telescope control
        private TcpListener listener10002; // New server for image scheduling
        private Task serverTask10001; // Replaced Thread with Task
        private Task serverTask10002; // Replaced Thread with Task
        private bool isRunning = false;
        private readonly TelescopeControl _telescopeControl;
        private CancellationTokenSource _cts; // For graceful shutdown
        private readonly List<Task> _clientTasks = new List<Task>(); // Track client tasks
        private readonly List<TcpClient> _activeClients = new List<TcpClient>(); // Track active clients
        private readonly object _clientsLock = new object(); // Lock for _activeClients
        private SchedulePick _schedulePick; // Single instance of SchedulePick form

        // Conversion constants for Stellarium's ra_int and dec_int to radians
        private const double StellariumScaleFactor = Math.PI / 2147483648.0;
        private const double TwoTo32 = 4294967296.0;

        // Configuration constants
        private static class Config
        {
            public const int TelescopePort = 10001;
            public const int CapturePort = 10002;
            public const int BufferSize = 1024;
            public const int ClientPollInterval = 50; // ms
        }

        public Stellarium(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));
        }

        // Generalized server logic for both ports
        private async Task RunServer(TcpListener listener, int port, bool isTelescopeControl, CancellationToken cancellationToken)
        {
            ErrorLogger logger = new ErrorLogger();
            try
            {
                listener.Start();
                logger.LogError($"Server started on port {port}...");

                while (!cancellationToken.IsCancellationRequested)
                {
                    var acceptTask = listener.AcceptTcpClientAsync();
                    await Task.Run(() => acceptTask, cancellationToken).ConfigureAwait(false);
                    TcpClient client = await acceptTask.ConfigureAwait(false);
                    logger.LogError($"Connected to Stellarium on port {port} from {client.Client.RemoteEndPoint}");
                    lock (_clientsLock)
                    {
                        _activeClients.Add(client);
                    }
                    Task clientTask = HandleClientAsync(client, port, isTelescopeControl, cancellationToken);
                    lock (_clientTasks)
                    {
                        _clientTasks.Add(clientTask);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogError($"Port {port}: Server shutdown requested.");
            }
            catch (SocketException ex)
            {
                logger.LogError($"Port {port}: Socket error: {ex.Message}");
                if (!IsDisposed && !Disposing)
                {
                    BeginInvoke((MethodInvoker)delegate
                    {
                        MessageBox.Show($"Port {port} Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                }
            }
            finally
            {
                listener?.Stop();
                logger.LogError($"Port {port}: Server stopped.");
            }
        }

        // Handle individual client connection
        private async Task HandleClientAsync(TcpClient client, int port, bool isTelescopeControl, CancellationToken cancellationToken)
        {
            ErrorLogger logger = new ErrorLogger();
            NetworkStream stream = null;
            try
            {
                // Enable TCP keep-alive
                client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                byte[] keepAlive = new byte[12];
                BitConverter.GetBytes(1u).CopyTo(keepAlive, 0); // Enable keep-alive
                BitConverter.GetBytes(30000u).CopyTo(keepAlive, 4); // 30 seconds time
                BitConverter.GetBytes(10000u).CopyTo(keepAlive, 8); // 10 seconds interval
                client.Client.IOControl(unchecked((int)0x98000004), keepAlive, null); // SIO_KEEPALIVE_VALS

                stream = client.GetStream();

                while (client.Connected && !cancellationToken.IsCancellationRequested)
                {
                    byte[] buffer = new byte[Config.BufferSize];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
                    if (bytesRead == 0)
                    {
                        logger.LogError($"Port {port}: Client disconnected (no data).");
                        break; // Client closed connection
                    }

                    logger.LogError($"Port {port}: Read {bytesRead} bytes from client.");

                    ushort messageLength = BitConverter.ToUInt16(buffer, 0);
                    ushort messageType = BitConverter.ToUInt16(buffer, 2);
                    logger.LogError($"Port {port}: Received message: Length={messageLength}, Type={messageType}");

                    if (messageLength == 20 && messageType == 0)
                    {
                        int raMicrodegrees = BitConverter.ToInt32(buffer, 12);
                        int decMicrodegrees = BitConverter.ToInt32(buffer, 16);
                        double raInt = raMicrodegrees;

                        lock (GlobalData.LockObject)
                        {
                            if (isTelescopeControl)
                            {
                                // Port 10001: Update telescope coordinates and slew
                                GlobalData.RAMicrodegrees = raMicrodegrees;
                                GlobalData.DecMicrodegrees = decMicrodegrees;
                                GlobalData.GotoRA = (raInt < 0 ? raInt + TwoTo32 : raInt) * StellariumScaleFactor;
                                GlobalData.GotoDec = decMicrodegrees * StellariumScaleFactor;
                                logger.LogError($"Port 10001: Goto RAInt={raMicrodegrees}, DecInt={decMicrodegrees}");
                            }
                            else
                            {
                                // Port 10002: Update capture coordinates only
                                GlobalData.CaptureRA = (raInt < 0 ? raInt + TwoTo32 : raInt) * StellariumScaleFactor;
                                GlobalData.CaptureDec = decMicrodegrees * StellariumScaleFactor;
                                logger.LogError($"Port 10002: Capture RA={GlobalData.CaptureRA:F6} Rad, Dec={GlobalData.CaptureDec:F6} Rad");
                                if (!IsDisposed && !Disposing)
                                {
                                    BeginInvoke((MethodInvoker)ShowSchedulePick);
                                }
                            }
                        }

                        if (isTelescopeControl)
                        {
                            try
                            {
                                await _telescopeControl.SendSlewToCommandAsync();
                                logger.LogError($"Port 10001: Slew command sent: RA={GlobalData.GotoRA:F6} Rad, Dec={GlobalData.GotoDec:F6} Rad");
                            }
                            catch (Exception ex)
                            {
                                logger.LogError($"Port 10001: Failed to send slew command: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        logger.LogError($"Port {port}: Unknown message type");
                    }

                    // Send telescope's current position back to Stellarium
                    byte[] response = new byte[24];
                    BitConverter.GetBytes((ushort)24).CopyTo(response, 0);
                    BitConverter.GetBytes((ushort)0).CopyTo(response, 2);
                    BitConverter.GetBytes(DateTime.Now.ToFileTimeUtc()).CopyTo(response, 4);
                    lock (GlobalData.LockObject)
                    {
                        int raIntResponse = (int)(GlobalData.RA / StellariumScaleFactor);
                        int decIntResponse = (int)(GlobalData.Dec / StellariumScaleFactor);
                        BitConverter.GetBytes(raIntResponse).CopyTo(response, 12);
                        BitConverter.GetBytes(decIntResponse).CopyTo(response, 16);
                    }
                    BitConverter.GetBytes(0).CopyTo(response, 20);

                    await stream.WriteAsync(response, 0, response.Length, cancellationToken).ConfigureAwait(false);
                    await stream.FlushAsync(cancellationToken).ConfigureAwait(false);

                    await Task.Delay(Config.ClientPollInterval, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset || ex.SocketErrorCode == SocketError.ConnectionAborted)
            {
                logger.LogError($"Port {port}: Client disconnected unexpectedly: {ex.Message}");
            }
            catch (SocketException ex)
            {
                logger.LogError($"Port {port}: Socket error: {ex.Message}");
            }
            catch (OperationCanceledException)
            {
                logger.LogError($"Port {port}: Client handling cancelled.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Port {port}: Client error: {ex.Message}");
            }
            finally
            {
                try
                {
                    stream?.Close();
                    client?.Close();
                }
                catch (Exception ex)
                {
                    logger.LogError($"Port {port}: Error closing client: {ex.Message}");
                }
                finally
                {
                    stream?.Dispose();
                    client?.Dispose();
                    lock (_clientsLock)
                    {
                        _activeClients.Remove(client);
                    }
                    lock (_clientTasks)
                    {
                        _clientTasks.RemoveAll(t => t.IsCompleted || t.IsFaulted || t.IsCanceled);
                    }
                    logger.LogError($"Port {port}: Client disconnected.");
                }
            }
        }

        // Show single instance of SchedulePick form
        private void ShowSchedulePick()
        {
            if (_schedulePick == null || _schedulePick.IsDisposed)
            {
                _schedulePick = new SchedulePick(_telescopeControl);
                _schedulePick.Show();
            }
            else
            {
                _schedulePick.BringToFront();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                string ipAddressInput = STIP_Box.Text.Trim();
                if (string.IsNullOrEmpty(ipAddressInput))
                {
                    MessageBox.Show("Please enter a valid IP address.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!System.Net.IPAddress.TryParse(ipAddressInput, out var ipAddress))
                {
                    MessageBox.Show("Invalid IP address format.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                isRunning = true;
                _cts = new CancellationTokenSource();

                listener10001 = new TcpListener(ipAddress, Config.TelescopePort);
                serverTask10001 = Task.Run(() => RunServer(listener10001, Config.TelescopePort, true, _cts.Token));

                listener10002 = new TcpListener(ipAddress, Config.CapturePort);
                serverTask10002 = Task.Run(() => RunServer(listener10002, Config.CapturePort, false, _cts.Token));

                ErrorLogger logger = new ErrorLogger();
                logger.LogError($"Starting servers on IP {ipAddress} ports {Config.TelescopePort} and {Config.CapturePort}...");
                ConnectedStatus.Text = $"Started on ({ipAddress}:{Config.TelescopePort} & {Config.CapturePort})";
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (!isRunning)
                return;

            isRunning = false;
            ConnectedStatus.Text = "Disconnecting";
            ErrorLogger logger = new ErrorLogger();

            _cts?.Cancel();

            lock (_clientsLock)
            {
                foreach (var client in _activeClients)
                {
                    try
                    {
                        var stream = client?.GetStream();
                        stream?.Close();
                        client?.Close();
                        stream?.Dispose();
                        client?.Dispose();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Error closing client: {ex.Message}");
                    }
                }
                _activeClients.Clear();
            }

            try
            {
                Task[] tasksToAwait;
                lock (_clientTasks)
                {
                    tasksToAwait = _clientTasks.ToArray();
                    _clientTasks.Clear();
                }
                if (tasksToAwait.Length > 0)
                {
                    await Task.WhenAll(tasksToAwait).WithTimeout(TimeSpan.FromSeconds(10));
                }
            }
            catch (TimeoutException)
            {
                logger.LogError("Some client tasks did not complete within 10 seconds");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error awaiting client tasks: {ex.Message}");
            }

            try
            {
                listener10001?.Stop();
                listener10002?.Stop();
                var serverTasks = new[] { serverTask10001, serverTask10002 }.Where(t => t != null).ToArray();
                if (serverTasks.Length > 0)
                {
                    await Task.WhenAll(serverTasks).WithTimeout(TimeSpan.FromSeconds(10));
                }
            }
            catch (TimeoutException)
            {
                logger.LogError("Server tasks did not complete within 10 seconds");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error awaiting server tasks: {ex.Message}");
            }

            listener10001 = null;
            listener10002 = null;
            serverTask10001 = null;
            serverTask10002 = null;
            _cts?.Dispose();
            _cts = null;

            ConnectedStatus.Text = "Servers Stopped";
        }

        private void Stellarium_Load(object sender, EventArgs e)
        {
        }
    }

    // Extension method for task timeout
    public static class TaskExtensions
    {
        public static async Task WithTimeout(this Task task, TimeSpan timeout)
        {
            using (var cts = new CancellationTokenSource())
            {
                var delayTask = Task.Delay(timeout, cts.Token);
                var completedTask = await Task.WhenAny(task, delayTask);
                if (completedTask == delayTask)
                {
                    throw new TimeoutException();
                }
                cts.Cancel(); // Cancel delay task
                await task; // Ensure task completes
            }
        }
    }
}