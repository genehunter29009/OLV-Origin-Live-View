using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OnStepControl
{
    /// <summary>
    /// Standalone class to communicate with OnStep telescope mount over WiFi (TCP).
    /// Compatible with C# 7.3 (no nullable reference types).
    /// </summary>
    public class OnStepWiFi : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private bool _isConnected;
        private readonly string _defaultIp = "192.168.0.1";
        private readonly int _defaultPort = 9999;   // Standard command channel (try 9998 if needed)

        public string IpAddress { get; private set; }
        public int Port { get; private set; }

        public OnStepWiFi()
        {
            IpAddress = string.Empty;
            Port = 0;
            _isConnected = false;
        }

        /// <summary>
        /// Connect to the OnStep mount.
        /// </summary>
        /// <param name="ip">IP address (default: 192.168.0.1)</param>
        /// <param name="port">TCP port (default: 9999). Many users prefer 9998 for persistent connection.</param>
        /// <returns>True if connected successfully.</returns>
        public bool Connect(string ip = null, int port = 0)
        {
            if (string.IsNullOrEmpty(ip)) ip = _defaultIp;
            if (port <= 0) port = _defaultPort;

            Disconnect(); // Close any previous connection

            try
            {
                _client = new TcpClient();
                _client.Connect(ip, port);

                _stream = _client.GetStream();
                _stream.ReadTimeout = 3000;
                _stream.WriteTimeout = 3000;

                IpAddress = ip;
                Port = port;
                _isConnected = true;

                Console.WriteLine("✅ Connected to OnStep at " + ip + ":" + port);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Connection failed: " + ex.Message);
                Disconnect();
                return false;
            }
        }

        /// <summary>
        /// Send a raw OnStep command and return the reply.
        /// Command should start with : and end with # (it will be added if missing).
        /// </summary>
        public async Task<string> SendCommandAsync(string command)
        {
            if (!_isConnected || _stream == null)
                throw new InvalidOperationException("Not connected to OnStep.");

            // Ensure command ends with #
            if (!command.EndsWith("#"))
                command += "#";

            byte[] sendData = Encoding.ASCII.GetBytes(command + "\r\n");
            await _stream.WriteAsync(sendData, 0, sendData.Length);

            // Read response until '#' is received
            StringBuilder response = new StringBuilder();
            byte[] buffer = new byte[256];

            while (true)
            {
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string chunk = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                response.Append(chunk);

                if (chunk.Contains("#"))
                    break;
            }

            string result = response.ToString().Trim();
            if (result.EndsWith("#"))
                result = result.Substring(0, result.Length - 1);

            return result;
        }

        // Synchronous convenience methods (easy to call)
        public string GetRA()
        {
            return SendCommandAsync(":GR#").GetAwaiter().GetResult();
        }

        public string GetDec()
        {
            return SendCommandAsync(":GD#").GetAwaiter().GetResult();
        }

        public string GetStatus()
        {
            return SendCommandAsync(":GU#").GetAwaiter().GetResult();
        }

        /// <summary>
        /// Slew to target coordinates.
        /// </summary>
        /// <param name="raHours">Right Ascension in decimal hours (0-23.999)</param>
        /// <param name="decDegrees">Declination in decimal degrees (-90 to +90)</param>
        /// <returns>True if slew was accepted by OnStep.</returns>
        public bool SlewTo(double raHours, double decDegrees)
        {
            try
            {
                // Format RA: HH:MM:SS
                int raH = (int)raHours;
                int raM = (int)((raHours - raH) * 60);
                int raS = (int)Math.Round((raHours - raH - raM / 60.0) * 3600);

                string raCmd = string.Format(":Sr{0:00}:{1:00}:{2:00}#", raH, raM, raS);
                string raReply = SendCommandAsync(raCmd).GetAwaiter().GetResult();

                // Format Dec: sDD:MM:SS
                char sign = decDegrees >= 0 ? '+' : '-';
                double absDec = Math.Abs(decDegrees);
                int decD = (int)absDec;
                int decM = (int)((absDec - decD) * 60);
                int decS = (int)Math.Round((absDec - decD - decM / 60.0) * 3600);

                string decCmd = string.Format(":Sd{0}{1:00}:{2:00}:{3:00}#", sign, decD, decM, decS);
                string decReply = SendCommandAsync(decCmd).GetAwaiter().GetResult();

                if (raReply != "0" && raReply != "1") return false;
                if (decReply != "0" && decReply != "1") return false;

                string slewReply = SendCommandAsync(":MS#").GetAwaiter().GetResult();
                return slewReply == "0";   // 0 = slew started successfully
            }
            catch
            {
                return false;
            }
        }

        public bool Stop()
        {
            try
            {
                SendCommandAsync(":Q#").GetAwaiter().GetResult();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            if (_stream != null)
            {
                _stream.Close();
                _stream = null;
            }

            if (_client != null)
            {
                _client.Close();
                _client = null;
            }

            _isConnected = false;
        }

        public void Dispose()
        {
            Disconnect();
        }

        /// <summary>
        /// Quick test method
        /// </summary>
        public async Task TestConnectionAsync()
        {
            if (!Connect()) return;

            Console.WriteLine("RA     : " + GetRA());
            Console.WriteLine("Dec    : " + GetDec());
            Console.WriteLine("Status : " + GetStatus());

            Disconnect();
        }
    }
}