using BackOffice.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace BackOffice.Helpers
{
    public static class FileViewerHelper
    {
        // Static HttpClient to be reused
        private static readonly HttpClient _httpClient;

        // Static constructor to initialize HttpClient
        static FileViewerHelper()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(Settings.Default.ApiBaseUrl)
            };
        }

        public static async Task LoadPdfFromApi(int documentId)
        {
            string tempFilePath = null;
            try
            {
                var response = await _httpClient.GetAsync($"FileSystem/download/{documentId}");

                if (response.IsSuccessStatusCode)
                {
                    // Create temp file with .pdf extension
                    tempFilePath = Path.Combine(Path.GetTempPath(), $"document_{Guid.NewGuid()}.pdf");

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = File.Create(tempFilePath))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    var process = Process.Start(new ProcessStartInfo
                    {
                        FileName = tempFilePath,
                        UseShellExecute = true
                    });

                    if (process != null)
                    {
                        // Start a background task to wait for the process to exit
                        Task.Run(() =>
                        {
                            process.WaitForExit();
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
                        });
                    }
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

                // Clean up temp file in case of error
                if (tempFilePath != null && File.Exists(tempFilePath))
                {
                    try
                    {
                        File.Delete(tempFilePath);
                    }
                    catch { /* Ignore cleanup errors */ }
                }
            }
        }

        // Optional: Method to clean up any orphaned temp files
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
    }
}