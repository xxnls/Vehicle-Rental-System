namespace API.Models.DTOs.FileSystem
{
    public class FileUploadDto
    {
        public byte[] FileContent { get; set; }
        public int DocumentTypeId { get; set; }
        public int DocumentCategoryId { get; set; }
        public int? VehicleId { get; set; }
        public int? EmployeeId { get; set; }
        public int? CustomerId { get; set; }
        public int? RentalPlaceId { get; set; }
        public int? RentalId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string FileName { get; set; }
        public int CreatedByEmployeeId { get; set; }
    }
}
