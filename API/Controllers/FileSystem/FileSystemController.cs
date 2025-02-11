using API.Models.DTOs.FileSystem;
using API.Models.FileSystem;
using API.Services.FileSystem;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.FileSystem
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileSystemController : BaseApiController<Document, DocumentDto, DocumentDto>
    {
        private readonly FileSystemService _fileSystemService;

        public FileSystemController(FileSystemService service) : base(service)
        {
            _fileSystemService = service;
        }

        // Upload a file
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDto uploadDto)
        {
            try
            {
                var document = await _fileSystemService.UploadFileAsync(uploadDto);
                return CreatedAtAction(nameof(DownloadFile), new { id = document.DocumentId }, document);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while uploading the file.");
            }
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

        protected override int GetEntityId(DocumentDto entity)
        {
            return entity.DocumentId;
        }
    }
}
