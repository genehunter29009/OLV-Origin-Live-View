using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OriginLiveView
{
    public partial class OLVComposeMosaic : Form
    {
        public OLVComposeMosaic()
        {
            InitializeComponent();
        }

        private void ButtonDownload_Click(object sender, EventArgs e)
        {
            string mosaicName = TextBoxName.Text.Trim();
            if (string.IsNullOrEmpty(mosaicName))
            {
                MessageBox.Show("Please enter a mosaic name in the text box.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Define FTP server and local download path
                string ftpHost = "ftp://origin.local";
                string ftpPath = "/Astrophotography";
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string sirilImagesPath = Path.Combine(desktopPath, "OLVImages");
                string localBasePath = Path.Combine(sirilImagesPath, $"Mosaic_{mosaicName}_{DateTime.Now:yyyyMMdd_HHmmss}");

                // Create SirilImages folder if it doesn't exist
                Directory.CreateDirectory(sirilImagesPath);

                // Get list of directories
                var directories = ListFtpDirectories(ftpHost + ftpPath, mosaicName);
                if (!directories.Any())
                {
                    MessageBox.Show($"No folders found containing '{mosaicName}' at {ftpHost}{ftpPath}.", "No Matches", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Directory.CreateDirectory(localBasePath);

                // Download each matching directory
                foreach (string dir in directories)
                {
                    string localDirPath = Path.Combine(localBasePath, Path.GetFileName(dir));
                    DownloadFtpDirectory(dir, localDirPath);
                }

                MessageBox.Show($"Successfully downloaded {directories.Count()} folder(s) to {localBasePath}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger();
                logger.LogError($"Failed to download folders: {ex.Message}");
                MessageBox.Show($"Error downloading folders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<string> ListFtpDirectories(string ftpUrl, string searchText)
        {
            var directories = new List<string>();
            var request = (FtpWebRequest)WebRequest.Create(ftpUrl);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential("anonymous", "");
            request.UseBinary = true;
            request.KeepAlive = false;

            try
            {
                using (var response = (FtpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("d") && line.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            string dirName = line.Substring(line.LastIndexOf(' ') + 1);
                            directories.Add($"{ftpUrl}/{dirName}");
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                throw new Exception($"Failed to list directories at {ftpUrl}: {ex.Message}", ex);
            }
            return directories;
        }

        private void DownloadFtpDirectory(string ftpDirUrl, string localDirPath)
        {
            Directory.CreateDirectory(localDirPath);
            var request = (FtpWebRequest)WebRequest.Create(ftpDirUrl);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential("anonymous", "");
            request.UseBinary = true;
            request.KeepAlive = false;

            try
            {
                using (var response = (FtpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string name = line.Substring(line.LastIndexOf(' ') + 1);
                        string fullFtpPath = $"{ftpDirUrl}/{name}";

                        if (line.StartsWith("d"))
                        {
                            DownloadFtpDirectory(fullFtpPath, Path.Combine(localDirPath, name));
                        }
                        else
                        {
                            DownloadFtpFile(fullFtpPath, Path.Combine(localDirPath, name));
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                throw new Exception($"Failed to download directory {ftpDirUrl}: {ex.Message}", ex);
            }
        }

        private void DownloadFtpFile(string ftpFileUrl, string localFilePath)
        {
            var request = (FtpWebRequest)WebRequest.Create(ftpFileUrl);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential("anonymous", "");
            request.UseBinary = true;
            request.KeepAlive = false;

            try
            {
                using (var response = (FtpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var fileStream = File.Create(localFilePath))
                {
                    stream.CopyTo(fileStream);
                }
            }
            catch (WebException ex)
            {
                throw new Exception($"Failed to download file {ftpFileUrl}: {ex.Message}", ex);
            }
        }

        private void ButtonCopyRen_Click(object sender, EventArgs e)
        {
            string mosaicName = TextBoxName.Text.Trim();
            if (string.IsNullOrEmpty(mosaicName))
            {
                MessageBox.Show("Please enter a mosaic name in the text box.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Define paths
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string olvImagesPath = Path.Combine(desktopPath, "OLVImages");
                string lightsPath = Path.Combine(olvImagesPath, "lights");
                string darksPath = Path.Combine(olvImagesPath, "darks");
                string biasPath = Path.Combine(olvImagesPath, "biases");
                string flatsPath = Path.Combine(olvImagesPath, "flats");
                string tiffsPath = Path.Combine(olvImagesPath, "tiffs");
                string processPath = Path.Combine(olvImagesPath, "process");

                // Create subfolders
                Directory.CreateDirectory(lightsPath);
                Directory.CreateDirectory(darksPath);
                Directory.CreateDirectory(biasPath);
                Directory.CreateDirectory(flatsPath);
                Directory.CreateDirectory(tiffsPath);
                Directory.CreateDirectory(processPath);

                // Single logger instance for the method
                var logger = new ErrorLogger();

                // Delete all files and folders in the process directory
                try
                {
                    if (Directory.Exists(processPath))
                    {
                        foreach (string file in Directory.GetFiles(processPath, "*", SearchOption.AllDirectories))
                        {
                            try
                            {
                                File.Delete(file);
                            }
                            catch (Exception ex)
                            {
                                logger.LogError($"Failed to delete file {file} in process directory: {ex.Message}");
                            }
                        }

                        foreach (string dir in Directory.GetDirectories(processPath, "*", SearchOption.AllDirectories))
                        {
                            try
                            {
                                Directory.Delete(dir, true);
                            }
                            catch (Exception ex)
                            {
                                logger.LogError($"Failed to delete directory {dir} in process directory: {ex.Message}");
                            }
                        }

                        try
                        {
                            Directory.Delete(processPath, false);
                            Directory.CreateDirectory(processPath);
                        }
                        catch (IOException ex) when (ex.Message.Contains("directory is not empty"))
                        {
                            logger.LogError($"Process directory {processPath} not empty after cleanup. Proceeding.");
                        }
                        catch (Exception ex)
                        {
                            logger.LogError($"Failed to delete process directory {processPath}: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error clearing process directory {processPath}: {ex.Message}");
                    MessageBox.Show($"Error clearing process directory: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Delete all files in the lights, darks, bias, and flats folders
                foreach (string file in Directory.GetFiles(lightsPath))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Failed to delete file {file}: {ex.Message}");
                        continue;
                    }
                }
                foreach (string file in Directory.GetFiles(darksPath))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Failed to delete file {file}: {ex.Message}");
                        continue;
                    }
                }
                foreach (string file in Directory.GetFiles(biasPath))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Failed to delete file {file}: {ex.Message}");
                        continue;
                    }
                }
                foreach (string file in Directory.GetFiles(flatsPath))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Failed to delete file {file}: {ex.Message}");
                        continue;
                    }
                }

                // Get mosaic folders matching the pattern {mosaicName}_*
                string[] mosaicFolders = Directory.GetDirectories(olvImagesPath, $"{mosaicName}_*");
                if (mosaicFolders.Length == 0)
                {
                    MessageBox.Show($"No mosaic folders found for '{mosaicName}' in {olvImagesPath}.", "No Matches", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Sort folders to ensure consistent Seq numbering
                Array.Sort(mosaicFolders);

                // Process each folder with Seq1, Seq2, ..., SeqN
                int tiffFilesFound = 0;
                int tiffFilesCopied = 0;
                logger.LogError($"Starting TIFF copy process for mosaic name: {mosaicName} with {mosaicFolders.Length} folder(s)");
                foreach (string folder in mosaicFolders)
                {
                    logger.LogError($"Mosaic folder: {folder}");
                }

                for (int i = 0; i < mosaicFolders.Length; i++)
                {
                    string seqPrefix = $"Seq{i + 1}_";
                    string mosaicFolder = mosaicFolders[i];

                    logger.LogError($"Processing mosaic folder: {mosaicFolder}");

                    // Process "Light" files
                    string[] lightFiles = Directory.GetFiles(mosaicFolder, "Light*.*", SearchOption.AllDirectories);
                    foreach (string lightFile in lightFiles)
                    {
                        string fileName = Path.GetFileName(lightFile);
                        string newFileName = seqPrefix + fileName;
                        string destinationPath = Path.Combine(lightsPath, newFileName);

                        if (File.Exists(destinationPath))
                        {
                            logger.LogError($"File {newFileName} already exists in {lightsPath}. Skipping.");
                            continue;
                        }

                        File.Copy(lightFile, destinationPath);
                    }

                    // Process "Dark" files
                    string[] darkFiles = Directory.GetFiles(mosaicFolder, "Dark*.*", SearchOption.AllDirectories);
                    foreach (string darkFile in darkFiles)
                    {
                        string fileName = Path.GetFileName(darkFile);
                        string newFileName = seqPrefix + fileName;
                        string destinationPath = Path.Combine(darksPath, newFileName);

                        if (File.Exists(destinationPath))
                        {
                            logger.LogError($"File {newFileName} already exists in {darksPath}. Skipping.");
                            continue;
                        }

                        File.Copy(darkFile, destinationPath);
                    }

                    // Process "Bias" files
                    string[] biasFiles = Directory.GetFiles(mosaicFolder, "Bias*.*", SearchOption.AllDirectories);
                    foreach (string biasFile in biasFiles)
                    {
                        string fileName = Path.GetFileName(biasFile);
                        string newFileName = seqPrefix + fileName;
                        string destinationPath = Path.Combine(biasPath, newFileName);

                        if (File.Exists(destinationPath))
                        {
                            logger.LogError($"File {newFileName} already exists in {biasPath}. Skipping.");
                            continue;
                        }

                        File.Copy(biasFile, destinationPath);
                    }

                    // Process "Flat" files
                    string[] flatFiles = Directory.GetFiles(mosaicFolder, "Flat*.*", SearchOption.AllDirectories);
                    foreach (string flatFile in flatFiles)
                    {
                        string fileName = Path.GetFileName(flatFile);
                        string newFileName = seqPrefix + fileName;
                        string destinationPath = Path.Combine(flatsPath, newFileName);

                        if (File.Exists(destinationPath))
                        {
                            logger.LogError($"File {newFileName} already exists in {flatsPath}. Skipping.");
                            continue;
                        }

                        File.Copy(flatFile, destinationPath);
                    }

                    // Process ".tiff" files (case-insensitive)
                    // Inside ButtonCopyRen_Click, replace the TIFF processing loop with this
                    // Process ".tiff" files, with special handling for FinalStackedMaster.tiff
                    string[] tiffFiles = Directory.GetFiles(mosaicFolder, "*.*", SearchOption.AllDirectories)
                        .Where(f => f.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".tif", StringComparison.OrdinalIgnoreCase))
                        .ToArray();
                    tiffFilesFound += tiffFiles.Length;
                    logger.LogError($"Found {tiffFiles.Length} TIFF files in {mosaicFolder}");

                    foreach (string tiffFile in tiffFiles)
                    {
                        string fileName = Path.GetFileName(tiffFile);
                        string newFileName = fileName;
                        string destinationFolder = tiffsPath;

                        // Special handling for FinalStackedMaster.tiff
                        if (fileName.Equals("FinalStackedMaster.tiff", StringComparison.OrdinalIgnoreCase))
                        {
                            newFileName = $"{seqPrefix}FinalStackedMaster.tiff"; // e.g., Seq1_FinalStackedMaster.tiff
                            logger.LogError($"Detected FinalStackedMaster.tiff in {mosaicFolder}. Renaming to {newFileName}");
                        }

                        string destinationPath = Path.Combine(destinationFolder, newFileName);

                        logger.LogError($"Attempting to copy TIFF file: {tiffFile} to {destinationPath}");

                        try
                        {
                            if (File.Exists(destinationPath))
                            {
                                var result = MessageBox.Show($"File {newFileName} already exists in {destinationFolder}. Overwrite?",
                                    "Confirm Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.No)
                                {
                                    logger.LogError($"File {newFileName} already exists in {destinationFolder}. Skipping.");
                                    continue;
                                }
                            }

                            File.Copy(tiffFile, destinationPath, true);
                            tiffFilesCopied++;
                            logger.LogError($"Successfully copied TIFF file {newFileName} to {destinationFolder}");
                        }
                        catch (Exception ex)
                        {
                            logger.LogError($"Failed to copy TIFF file {newFileName} to {destinationFolder}: {ex.Message}");
                            continue;
                        }
                    }
                }

                MessageBox.Show($"Successfully copied and renamed Light, Dark, Bias, and Flat files from {mosaicFolders.Length} folder(s). Found {tiffFilesFound} TIFF file(s), copied {tiffFilesCopied} to {tiffsPath}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger();
                logger.LogError($"Failed to process copy/rename: {ex.Message}");
                MessageBox.Show($"Error during copy/rename: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OLVComposeMosaic_Load(object sender, EventArgs e)
        {

        }

        private async void ButtonCompose_Click(object sender, EventArgs e)
        {
            try
            {
                // Path to siril-cli executable (adjust if different)
                string sirilPath = @"C:\Program Files\SiriL\bin\siril-cli.exe";
                // Dynamically get the Desktop path
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                // Path to the script
                string scriptPath = Path.Combine(desktopPath, @"OLVImages\scripts\Celestron_Origin_Mosaic.ssf");
                // Working directory for Siril
                string workingDirectory = Path.Combine(desktopPath, "OLVImages");
                // Process directory for cleanup
                string processDirectory = Path.Combine(workingDirectory, "process");

                // Validate paths
                if (!File.Exists(sirilPath))
                {
                    MessageBox.Show("siril-cli.exe not found at: " + sirilPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!File.Exists(scriptPath))
                {
                    MessageBox.Show("Script file not found at: " + scriptPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!Directory.Exists(workingDirectory))
                {
                    MessageBox.Show("Working directory not found at: " + workingDirectory, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Command-line arguments
                string arguments = $"-s \"{scriptPath}\"";

                // Configure the process
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = sirilPath,
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                MessageBox.Show($"Siril started in background successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Start the process
                using (Process process = new Process())
                {
                    process.StartInfo = processInfo;
                    process.Start();

                    // Read output and errors asynchronously
                    string output = await process.StandardOutput.ReadToEndAsync();
                    string errors = await process.StandardError.ReadToEndAsync();
                    await Task.Run(() => process.WaitForExit());

                    // Save output for debugging
                    string outputFile = Path.Combine(processDirectory, "siril_output.txt");
                    string errorFile = Path.Combine(processDirectory, "siril_errors.txt");
                    File.WriteAllText(outputFile, output);

                    if (process.ExitCode == 0)
                    {
                        // Cleanup: Delete all files and folders in process directory except result.fit
                        if (Directory.Exists(processDirectory))
                        {
                            // Delete all files except result.fit
                            foreach (string file in Directory.GetFiles(processDirectory))
                            {
                                if (Path.GetFileName(file) != "result.fit")
                                {
                                    File.Delete(file);
                                }
                            }

                            // Delete all subdirectories
                            foreach (string dir in Directory.GetDirectories(processDirectory))
                            {
                                Directory.Delete(dir, true); // true for recursive delete
                            }

                            MessageBox.Show($"Siril processed successfully!\nOutput saved to: {processDirectory}\\result.fit\nAll other files and folders in process directory deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"Siril processed successfully, but process directory not found at: {processDirectory}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        File.WriteAllText(errorFile, errors);
                        MessageBox.Show($"Error running Siril:\nErrors saved to: {errorFile}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ButtonCleanup_Click(object sender, EventArgs e)
        {
            try
            {
                // Define paths
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string olvImagesPath = Path.Combine(desktopPath, "OLVImages");
                string[] targetFolders =
                {
            Path.Combine(olvImagesPath, "darks"),
            Path.Combine(olvImagesPath, "lights"),
            Path.Combine(olvImagesPath, "biases"),
            Path.Combine(olvImagesPath, "flats"),
            Path.Combine(olvImagesPath, "tiffs")
        };

                // Initialize logger
                var logger = new ErrorLogger();
                int filesDeleted = 0;
                int filesSkipped = 0;

                // Process each folder
                foreach (string folder in targetFolders)
                {
                    if (!Directory.Exists(folder))
                    {
                        logger.LogError($"Folder not found: {folder}");
                        continue;
                    }

                    // Get .fit, .fits, and (for tiffs folder) .tiff/.tif files in the folder (not subfolders)
                    string[] filesToDelete = Directory.GetFiles(folder)
                        .Where(f => f.EndsWith(".fit", StringComparison.OrdinalIgnoreCase) ||
                                   f.EndsWith(".fits", StringComparison.OrdinalIgnoreCase) ||
                                   (folder.EndsWith("tiffs") &&
                                    (f.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) ||
                                     f.EndsWith(".tif", StringComparison.OrdinalIgnoreCase))))
                        .ToArray();

                    foreach (string file in filesToDelete)
                    {
                        try
                        {
                            File.Delete(file);
                            filesDeleted++;
                            logger.LogError($"Deleted file: {file}");
                        }
                        catch (Exception ex)
                        {
                            filesSkipped++;
                            logger.LogError($"Failed to delete file {file}: {ex.Message}");
                        }
                    }
                }

                // Show summary
                if (filesDeleted == 0 && filesSkipped == 0)
                {
                    MessageBox.Show("No FITS or TIFF files found in the specified folders.", "Cleanup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Cleanup complete. Deleted {filesDeleted} file(s). Skipped {filesSkipped} file(s) due to errors.",
                        "Cleanup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger();
                logger.LogError($"Error during cleanup: {ex.Message}");
                MessageBox.Show($"Error during cleanup: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    
}