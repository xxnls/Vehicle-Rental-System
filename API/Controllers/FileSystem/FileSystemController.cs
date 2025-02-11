using API.Models.DTOs.FileSystem;
using API.Services.FileSystem;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.FileSystem
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileSystemController : ControllerBase
    {
        private readonly FileSystemService _fileSystemService;

        public FileSystemController(FileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
        }

        // Upload a file
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] DocumentCreateDto createDto, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var document = await _fileSystemService.UploadFileAsync(file, createDto);
            return CreatedAtAction(nameof(DownloadFile), new { id = document.DocumentId }, document);
        }

        // Download a file
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            try
            {
                var (content, fileName) = await _fileSystemService.DownloadFileAsync(id);
                return File(content, "application/octet-stream", fileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

        // Delete a file
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var success = await _fileSystemService.DeleteFileAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
