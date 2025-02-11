using API.Context;
using API.Models.DTOs.FileSystem;
using API.Models.FileSystem;

namespace API.Services.FileSystem
{
    public class FileSystemService
    {
        private readonly ApiDbContext _context;

        public FileSystemService(ApiDbContext context)
        {
            _context = context;
        }

        // Upload a file
        public async Task<Document> UploadFileAsync(IFormFile file, DocumentCreateDto createDto)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                var document = new Document
                {
                    DocumentTypeId = createDto.DocumentTypeId,
                    DocumentCategoryId = createDto.DocumentCategoryId,
                    VehicleId = createDto.VehicleId,
                    EmployeeId = createDto.EmployeeId,
                    CustomerId = createDto.CustomerId,
                    RentalPlaceId = createDto.RentalPlaceId,
                    RentalId = createDto.RentalId,
                    Title = createDto.Title,
                    Description = createDto.Description,
                    FileName = file.FileName,
                    OriginalFileName = file.FileName,
                    FileSizeMb = file.Length / 1024.0 / 1024.0, // Convert bytes to MB
                    FileContent = memoryStream.ToArray(),
                    CreatedDate = DateTime.UtcNow,
                    CreatedByEmployeeId = createDto.CreatedByEmployeeId,
                    IsActive = true
                };

                _context.Documents.Add(document);
                await _context.SaveChangesAsync();

                return document;
            }
        }

        // Download a file
        public async Task<(byte[] Content, string FileName)> DownloadFileAsync(int documentId)
        {
            var document = await _context.Documents.FindAsync(documentId);
            if (document == null || document.FileContent == null)
            {
                throw new FileNotFoundException("Document not found or has no content.");
            }

            return (document.FileContent, document.OriginalFileName);
        }

        // Delete a file
        public async Task<bool> DeleteFileAsync(int documentId)
        {
            var document = await _context.Documents.FindAsync(documentId);
            if (document == null)
            {
                return false;
            }

            document.IsActive = false;
            document.DeletedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
