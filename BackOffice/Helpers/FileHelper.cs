using BackOffice.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using BackOffice.Models.DTOs.FileSystem;
using BackOffice.Services;

namespace BackOffice.Helpers
{
    public static class FileHelper
    {
        private static readonly HttpClient _httpClient;
        private static readonly ApiClient _apiClient;

        static FileHelper()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(Settings.Default.ApiBaseUrl)
            };
            _apiClient = new ApiClient();
        }

        /// <summary>
        /// Downloads and opens a file in the default application
        /// </summary>
        /// <param name="documentId">
        /// The ID of the document to view
        /// </param>
        public static async Task ViewFile(int documentId)
        {
            string tempFilePath = null;
            try
            {
                var response = await _httpClient.GetAsync($"FileSystem/download/{documentId}");

                if (response.IsSuccessStatusCode)
                {
                    var document = await _apiClient.GetAsync<DocumentDto>($"FileSystem/{documentId}");

                    tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_{document.FileName}");

                    await using (var stream = await response.Content.ReadAsStreamAsync())
                    await using (var fileStream = File.Create(tempFilePath))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    // Wait for the file to be accessible
                    await WaitForFileAccess(tempFilePath);

                    // Open the file with the default application
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = tempFilePath,
                            UseShellExecute = true,
                            Verb = "open"
                        }
                    };

                    process.Exited += async (sender, args) =>
                    {
                        // Add delay before deleting to ensure file is not in use
                        await Task.Delay(1000);
                        try
                        {
                            if (File.Exists(tempFilePath))
                            {
                                File.Delete(tempFilePath);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Failed to delete temporary file: {ex.Message}");
                        }
                    };

                    process.EnableRaisingEvents = true;
                    process.Start();
                }
                else
                {
                    MessageBox.Show(
                        $"Failed to download PDF. Status Code: {response.StatusCode}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while downloading the PDF: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                if (tempFilePath != null && File.Exists(tempFilePath))
                {
                    try
                    {
                        File.Delete(tempFilePath);
                    }
                    catch
                    {
                        /* Ignore cleanup errors */
                    }
                }
            }
        }

        /// <summary>
        /// Downloads a file to a specified location
        /// </summary>
        /// <param name="documentId">The ID of the document to download</param>
        /// <param name="destinationPath">The full path (including filename) where the file should be saved</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task DownloadFile(int documentId, string destinationPath)
        {
            try
            {
                var response = await _httpClient.GetAsync($"FileSystem/download/{documentId}");

                if (response.IsSuccessStatusCode)
                {
                    // Ensure the destination directory exists
                    var destinationDirectory = Path.GetDirectoryName(destinationPath);
                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(destinationDirectory);
                    }

                    // Save the file to the specified location
                    await using (var stream = await response.Content.ReadAsStreamAsync())
                    await using (var fileStream = File.Create(destinationPath))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    MessageBox.Show(
                        $"File downloaded successfully to: {destinationPath}",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Failed to download file. Status Code: {response.StatusCode}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while downloading the file: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Downloads a file to a specified location and opens a dialog to select the destination
        /// </summary>
        /// <param name="documentId">
        /// The ID of the document to download
        /// </param>
        /// <returns></returns>
        public static async Task DownloadFileWithDialog(int documentId)
        {
            try
            {
                // Fetch document metadata to get the file name
                var document = await _apiClient.GetAsync<DocumentDto>($"FileSystem/{documentId}");

                // Extract the file extension from the file name
                var fileExtension = Path.GetExtension(document.FileName);

                // If the file name has no extension, fall back to the DocumentType.FileExtension
                if (string.IsNullOrEmpty(fileExtension))
                {
                    fileExtension = document.DocumentType.FileExtension;
                    // Ensure the file extension starts with a dot
                    if (!string.IsNullOrEmpty(fileExtension) && !fileExtension.StartsWith("."))
                    {
                        fileExtension = "." + fileExtension;
                    }
                }

                // If no extension is available, default to ".bin"
                if (string.IsNullOrEmpty(fileExtension))
                {
                    fileExtension = ".bin";
                }

                // Create a SaveFileDialog
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = document.FileName,
                    DefaultExt = fileExtension,
                    Filter = $"{document.DocumentType.Name} Files (*{fileExtension})|*{fileExtension}|All Files (*.*)|*.*", 
                    Title = "Save File"
                };

                // Show the dialog and check if the user clicked "Save"
                if (saveFileDialog.ShowDialog() == true)
                {
                    var destinationPath = saveFileDialog.FileName;
                    await DownloadFile(documentId, destinationPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while preparing the download: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Cleans up temporary files created by the FileHelper
        /// </summary>
        public static void CleanupTempFiles()
        {
            try
            {
                var tempPath = Path.GetTempPath();
                var tempFiles = Directory.GetFiles(tempPath, "document_*.pdf");

                foreach (var file in tempFiles)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { /* Ignore individual file deletion errors */ }
                }
            }
            catch { /* Ignore cleanup errors */ }
        }

        /// <summary>
        /// Waits for a file to be accessible
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="timeoutMs"></param>
        /// <returns></returns>
        /// <exception cref="TimeoutException"></exception>
        private static async Task WaitForFileAccess(string filePath, int timeoutMs = 5000)
        {
            var startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalMilliseconds < timeoutMs)
            {
                try
                {
                    await using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    // If we can open the file, it's ready
                    return;
                }
                catch (IOException)
                {
                    // File is still locked, wait a bit
                    await Task.Delay(100);
                }
            }

            throw new TimeoutException($"File {filePath} remained locked for too long");
        }
    }
}