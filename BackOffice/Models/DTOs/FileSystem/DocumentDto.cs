using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Models.DTOs.Customers;
using BackOffice.Models.DTOs.Employees;
using BackOffice.Models.DTOs.Other;
using BackOffice.Models.DTOs.Rentals;
using BackOffice.Models.DTOs.Vehicles;

namespace BackOffice.Models.DTOs.FileSystem
{
    public class DocumentDto : BaseDtoModel
    {
        public int DocumentId { get; set; }

        private int _documentTypeId;
        private int _documentCategoryId;
        private int? _vehicleId;
        private int? _employeeId;
        private int? _customerId;
        private int? _rentalPlaceId;
        private int? _rentalId;
        private string _title;
        private string _description;
        private string _fileName;
        private string _originalFileName;
        private double _fileSizeMb;
        private byte[] _fileContent;
        private int _createdByEmployeeId;
        private int? _modifiedByEmployeeId;

        public int DocumentTypeId { get => _documentTypeId; set { _documentTypeId = value; OnPropertyChanged(); } }
        public int DocumentCategoryId { get => _documentCategoryId; set { _documentCategoryId = value; OnPropertyChanged(); } }
        public int? VehicleId { get => _vehicleId; set { _vehicleId = value; OnPropertyChanged(); } }
        public int? EmployeeId { get => _employeeId; set { _employeeId = value; OnPropertyChanged(); } }
        public int? CustomerId { get => _customerId; set { _customerId = value; OnPropertyChanged(); } }
        public int? RentalPlaceId { get => _rentalPlaceId; set { _rentalPlaceId = value; OnPropertyChanged(); } }
        public int? RentalId { get => _rentalId; set { _rentalId = value; OnPropertyChanged(); } }
        public string Title { get => _title; set { _title = value; OnPropertyChanged(); } }
        public string Description { get => _description; set { _description = value; OnPropertyChanged(); } }
        public string FileName { get => _fileName; set { _fileName = value; OnPropertyChanged(); } }
        public string OriginalFileName { get => _originalFileName; set { _originalFileName = value; OnPropertyChanged(); } }
        public double FileSizeMb { get => _fileSizeMb; set { _fileSizeMb = value; OnPropertyChanged(); } }
        public byte[] FileContent { get => _fileContent; set { _fileContent = value; OnPropertyChanged(); } }
        public int CreatedByEmployeeId { get => _createdByEmployeeId; set { _createdByEmployeeId = value; OnPropertyChanged(); } }
        public int? ModifiedByEmployeeId { get => _modifiedByEmployeeId; set { _modifiedByEmployeeId = value; OnPropertyChanged(); } }

        // Navigation properties
        private EmployeeDto _createdByEmployee;
        private CustomerDto _customer;
        private DocumentCategoryDto _documentCategory;
        private DocumentTypeDto _documentType;
        private EmployeeDto _employee;
        private EmployeeDto _modifiedByEmployee;
        private RentalDto _rental;
        private RentalPlaceDto _rentalPlace;
        private VehicleDto _vehicle;

        public EmployeeDto CreatedByEmployee { get => _createdByEmployee; set { _createdByEmployee = value; OnPropertyChanged(); } }
        public CustomerDto Customer { get => _customer; set { _customer = value; OnPropertyChanged(); } }
        public DocumentCategoryDto DocumentCategory { get => _documentCategory; set { _documentCategory = value; OnPropertyChanged(); } }
        public DocumentTypeDto DocumentType { get => _documentType; set { _documentType = value; OnPropertyChanged(); } }
        public EmployeeDto Employee { get => _employee; set { _employee = value; OnPropertyChanged(); } }
        public EmployeeDto ModifiedByEmployee { get => _modifiedByEmployee; set { _modifiedByEmployee = value; OnPropertyChanged(); } }
        public RentalDto Rental { get => _rental; set { _rental = value; OnPropertyChanged(); } }
        public RentalPlaceDto RentalPlace { get => _rentalPlace; set { _rentalPlace = value; OnPropertyChanged(); } }
        public VehicleDto Vehicle { get => _vehicle; set { _vehicle = value; OnPropertyChanged(); } }
    }
}
