using API.Models.DTOs.Customers;
using API.Models.DTOs.FileSystem;
using API.Models.DTOs.Other;
using API.Services.Customers;
using API.Services.Employees;
using API.Services.FileSystem;
using API.Services.Other;

namespace API.BusinessLogic
{
    public class LicenseProcessing : ILicenseProcessing
    {
        private readonly FileSystemService _fileSystemService;
        private readonly LicenseApprovalRequestsService _licenseApprovalRequestsService;
        private readonly CustomersService _customersService;
        private readonly EmployeesService _employeesService;

        public LicenseProcessing(
            FileSystemService fileSystemService,
            LicenseApprovalRequestsService licenseApprovalRequestsService,
            CustomersService customersService,
            EmployeesService employeesService)
        {
            _fileSystemService = fileSystemService;
            _licenseApprovalRequestsService = licenseApprovalRequestsService;
            _customersService = customersService;
            _employeesService = employeesService;
        }

        /// <summary>
        /// Upload license and create license approval request
        /// </summary>
        /// <param name="request">
        /// Upload license request
        /// </param>
        public async Task UploadLicense(UploadLicenseDto request)
        {
            try
            {
                // Get customer by id
                var customer = await _customersService.GetByIdAsync(request.CustomerId);

                // Upload license front
                var uploadResultFront = await _fileSystemService.UploadFileAsync(new FileUploadDto
                {
                    FileContent = request.FileContentFront,
                    DocumentTypeId = 2,
                    DocumentCategoryId = 3,
                    CustomerId = request.CustomerId,
                    Title = $"LicenseFront_{request.LicenseType}_{customer.FirstName}_{customer.LastName}",
                    FileName = request.FileNameFront,
                    CreatedByEmployeeId = 1
                });

                var uploadResultBack = await _fileSystemService.UploadFileAsync(new FileUploadDto
                {
                    FileContent = request.FileContentBack,
                    DocumentTypeId = 2,
                    DocumentCategoryId = 3,
                    CustomerId = request.CustomerId,
                    Title = $"LicenseBack_{request.LicenseType}_{customer.FirstName}_{customer.LastName}",
                    FileName = request.FileNameBack,
                    CreatedByEmployeeId = 1
                });

                // Create license approval request
                await _licenseApprovalRequestsService.CreateAsync(new LicenseApprovalRequestsDto
                {
                    CustomerId = request.CustomerId,
                    DocumentFrontId = uploadResultFront.DocumentId,
                    DocumentBackId = uploadResultBack.DocumentId,
                    Customer = customer,
                    DocumentFront = uploadResultFront,
                    DocumentBack = uploadResultBack,
                    LicenseType = request.LicenseType
                });
            }
            catch (Exception e)
            {
                throw new Exception("Error while uploading license.", e);
            }
        }

        public async Task ApproveLicenseAsync(int requestId, int emplyoeeId)
        {
            try
            {
                var licenseApprovalRequest = await _licenseApprovalRequestsService.GetByIdAsync(requestId);
                licenseApprovalRequest.RequestStatus = "Approved";
                licenseApprovalRequest.ModifiedDate = DateTime.Now;
                licenseApprovalRequest.ApprovedByEmployeeId = emplyoeeId;

                var customer = await _customersService.GetByIdAsync(licenseApprovalRequest.CustomerId);

                switch (licenseApprovalRequest.LicenseType)
                {
                    case "A":
                        customer.ApprovedA = true;
                        break;
                    case "B":
                        customer.ApprovedB = true;
                        break;
                    case "C":
                        customer.ApprovedC = true;
                        break;
                }

                await _customersService.UpdateAsync(customer.Id, customer);
                await _licenseApprovalRequestsService.UpdateAsync(licenseApprovalRequest.LicenseApprovalRequestId, licenseApprovalRequest);
            }
            catch (Exception e)
            {
                throw new Exception("Error while approving license.", e);
            }
        }
    }

    public interface ILicenseProcessing
    {
        public Task UploadLicense(UploadLicenseDto request);
        public Task ApproveLicenseAsync(int requestId, int employeeid);
    }
}
