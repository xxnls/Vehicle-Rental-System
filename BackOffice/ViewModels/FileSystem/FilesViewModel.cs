using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.ViewModels.Other;
using BackOffice.Views.Other;
using BackOffice.Views.FileSystem;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BackOffice.Models.DTOs.Customers;
using BackOffice.Models.DTOs.Employees;
using BackOffice.Models.DTOs.FileSystem;
using BackOffice.Models.DTOs.Other;
using BackOffice.Models.DTOs.Rentals;
using BackOffice.Models.DTOs.Vehicles;
using BackOffice.ViewModels.Customers;
using BackOffice.ViewModels.Employees;
using BackOffice.ViewModels.Rentals;
using BackOffice.ViewModels.Vehicles;
using BackOffice.Views.Customers;
using BackOffice.Views.Employees;
using BackOffice.Views.Rentals;
using BackOffice.Views.Vehicles;
using System.Windows;

namespace BackOffice.ViewModels.FileSystem
{
    public class FilesViewModel : BaseListViewModel<DocumentDto>, IListViewModel
    {
        public ICommand ViewFileContentCommand { get; set; }
        public ICommand DownloadFileContentCommand { get; set; }
        public ICommand UploadFileCommand { get; set; }
        public SelectorDialogParameters SelectDocumentCategoryParameters { get; set; }
        public SelectorDialogParameters SelectDocumentTypeParameters { get; set; }
        public SelectorDialogParameters SelectCustomerParameters { get; set; }
        public SelectorDialogParameters SelectEmployeeParameters { get; set; }
        public SelectorDialogParameters SelectVehicleParameters { get; set; }
        public SelectorDialogParameters SelectRentalPlaceParameters { get; set; }
        public SelectorDialogParameters SelectRentalParameters { get; set; }

        public FilesViewModel() : base("FileSystem", LocalizationHelper.GetString("Files", "DisplayName"))
        {
            // Initialize selector parameters for related entities
            SelectDocumentCategoryParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(DocumentCategoriesViewModel),
                SelectorView = new DocumentCategoriesView(),
                TargetProperty = result =>
                {
                    EditableModel.DocumentCategory ??= new DocumentCategoryDto();
                    EditableModel.DocumentCategory = (DocumentCategoryDto)result;
                },
                Title = LocalizationHelper.GetString("Files", "SelectDocumentCategory")
            };

            SelectDocumentTypeParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(DocumentTypesViewModel),
                SelectorView = new DocumentTypesView(),
                TargetProperty = result =>
                {
                    EditableModel.DocumentType ??= new DocumentTypeDto();
                    EditableModel.DocumentType = (DocumentTypeDto)result;
                },
                Title = LocalizationHelper.GetString("Files", "SelectType")
            };

            SelectCustomerParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(CustomersViewModel),
                SelectorView = new CustomersView(),
                TargetProperty = result =>
                {
                    EditableModel.Customer ??= new CustomerDto();
                    EditableModel.Customer = (CustomerDto)result;
                },
                Title = LocalizationHelper.GetString("Files", "SelectCustomer")
            };

            SelectEmployeeParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(EmployeesViewModel),
                SelectorView = new EmployeesView(),
                TargetProperty = result =>
                {
                    EditableModel.Employee ??= new EmployeeDto();
                    EditableModel.Employee = (EmployeeDto)result;
                },
                Title = LocalizationHelper.GetString("Files", "SelectEmployee")
            };

            SelectVehicleParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(VehiclesViewModel),
                SelectorView = new VehiclesView(),
                TargetProperty = result =>
                {
                    EditableModel.Vehicle ??= new VehicleDto();
                    EditableModel.Vehicle = (VehicleDto)result;
                },
                Title = LocalizationHelper.GetString("Files", "SelectVehicle")
            };

            SelectRentalPlaceParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(RentalPlacesViewModel),
                SelectorView = new RentalPlacesView(),
                TargetProperty = result =>
                {
                    EditableModel.RentalPlace ??= new RentalPlaceDto();
                    EditableModel.RentalPlace = (RentalPlaceDto)result;
                },
                Title = LocalizationHelper.GetString("Files", "SelectRentalPlace")
            };

            SelectRentalParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(RentalsViewModel),
                SelectorView = new RentalsView(),
                TargetProperty = result =>
                {
                    EditableModel.Rental ??= new RentalDto();
                    EditableModel.Rental = (RentalDto)result;
                },
                Title = LocalizationHelper.GetString("Files", "SelectRental")
            };

            // Commands
            CreateModelCommand = new AsyncRelayCommand(() => CreateModelAsync(EditableModel));
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.DocumentId, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.DocumentId),
                () => EditableModel != null
            );

            ViewFileContentCommand = new AsyncRelayCommand<int>(FileHelper.ViewFile);
            DownloadFileContentCommand = new AsyncRelayCommand<int>(FileHelper.DownloadFileWithDialog);
            UploadFileCommand = new AsyncRelayCommand(UploadFileAsync);

            // Validation rules
            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.Title), ValidateTitle },
                { nameof(EditableModel.Description), ValidateDescription },
                // { nameof(EditableModel.FileContent), ValidateFileContent },
                { nameof(EditableModel.DocumentType), ValidateDocumentType },
                { nameof(EditableModel.DocumentCategory), ValidateDocumentCategory }
            };
        }

        protected async Task UpdateModelAsync(int id, DocumentDto model)
        {
            model.RentalPlace = null;
            model.Rental = null;
            model.Vehicle = null;
            model.Customer = null;
            model.Employee = null;

            //model.RentalPlaceId = EditableModel.RentalPlaceId;
            //model.RentalId = EditableModel.RentalId;
            //model.VehicleId = EditableModel.VehicleId;
            //model.CustomerId = EditableModel.CustomerId;
            //model.EmployeeId = EditableModel.EmployeeId;

            var user = (EmployeeDto)SessionManager.Get("User");
            model.ModifiedByEmployeeId = user.Id;

            await base.UpdateModelAsync(id, model);
        }

        protected override async Task RestoreModelAsync(int id, DocumentDto model)
        {
            if (model == null)
            {
                UpdateStatus(LocalizationHelper.GetString("Generic", "USRestoreSelect"));
                return;
            }

            model.DeletedDate = null;
            model.IsActive = true;

            await UpdateModelAsync(id, model);
        }

        private new async Task CreateModelAsync(DocumentDto model)
        {
            try
            {
                IsBusy = true;

                UpdateStatus(LocalizationHelper.GetString("Files", "FileUploadMessage"));

                if (EditableModel == null)
                {
                    UpdateStatus(LocalizationHelper.GetString("Generic", "USCreateError1"));
                    return;
                }

                var currentUser = (EmployeeDto)SessionManager.Get("User")!;

                var fileUploadDto = new FileUploadDto
                {
                    FileContent = model.FileContent,
                    DocumentTypeId = model.DocumentType.DocumentTypeId,
                    DocumentCategoryId = model.DocumentCategory.DocumentCategoryId,
                    VehicleId = model.Vehicle?.VehicleId,
                    EmployeeId = model.Employee?.Id,
                    CustomerId = model.Customer?.Id,
                    RentalPlaceId = model.RentalPlace?.RentalPlaceId,
                    RentalId = model.Rental?.RentalId,
                    Title = model.Title,
                    FileName = model.FileName,
                    Description = model.Description,
                    CreatedByEmployeeId = currentUser.Id
                };

                await ApiClient.PostAsync<FileUploadDto, DocumentDto>($"{EndPointName}/upload", fileUploadDto);
                UpdateStatus(DisplayName + LocalizationHelper.GetString("Generic", "USCreateSuccess"));

                await LoadModelsAsync();
            }
            catch (Exception ex)
            {
                UpdateStatus(LocalizationHelper.GetString("Generic", "USCreateError2") + $" {DisplayName}: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                SwitchViewMode(ViewMode.List);
            }
        }

        private async Task UploadFileAsync()
        {
            try
            {
                // Open a file dialog to select a file
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "All Files (*.*)|*.*", // Allow all file types
                    Title = LocalizationHelper.GetString("Files", "UploadFileTitle")
                };

                // Show the dialog and check if the user selected a file
                if (openFileDialog.ShowDialog() == true)
                {
                    // Read the file content
                    var filePath = openFileDialog.FileName;
                    var fileName = Path.GetFileName(filePath);
                    var fileContent = await File.ReadAllBytesAsync(filePath);

                    // Set the file content and name in the EditableModel
                    EditableModel.FileContent = fileContent;
                    EditableModel.FileName = fileName;

                    // Notify the UI that the file has been selected
                    OnPropertyChanged(nameof(EditableModel.FileName));
                    OnPropertyChanged(nameof(EditableModel.FileContent));

                    // Clear any previous validation errors
                    ClearErrors(nameof(EditableModel.FileContent));
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file access errors)
                MessageBox.Show($"An error occurred while selecting the file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Validation
        private void ValidateTitle()
        {
            ClearErrors(nameof(EditableModel.Title));

            if (string.IsNullOrWhiteSpace(EditableModel.Title))
            {
                AddError(nameof(EditableModel.Title), LocalizationHelper.GetString("Files", "ErrorTitle1"));
            }
            else if (EditableModel.Title.Length > 200)
            {
                AddError(nameof(EditableModel.Title), LocalizationHelper.GetString("Files", "ErrorTitle2"));
            }
        }

        private void ValidateDescription()
        {
            ClearErrors(nameof(EditableModel.Description));

            if (!string.IsNullOrWhiteSpace(EditableModel.Description) && EditableModel.Description.Length > 400)
            {
                AddError(nameof(EditableModel.Description), LocalizationHelper.GetString("Files", "ErrorDescription1"));
            }
        }

        //private void ValidateFileContent()
        //{
        //    ClearErrors(nameof(EditableModel.FileContent));

        //    if (EditableModel.FileContent == null || EditableModel.FileContent.Length == 0 && IsCreating)
        //    {
        //        AddError(nameof(EditableModel.FileContent), LocalizationHelper.GetString("Files", "ErrorFileContent1"));
        //    }
        //}

        private void ValidateDocumentType()
        {
            ClearErrors(nameof(EditableModel.DocumentType));

            if (EditableModel.DocumentType == null)
            {
                AddError(nameof(EditableModel.DocumentType), LocalizationHelper.GetString("Files", "ErrorDocumentType1"));
            }
        }

        private void ValidateDocumentCategory()
        {
            ClearErrors(nameof(EditableModel.DocumentCategory));

            if (EditableModel.DocumentCategory == null)
            {
                AddError(nameof(EditableModel.DocumentCategory), LocalizationHelper.GetString("Files", "ErrorDocumentCategory1"));
            }
        }
        #endregion
    }
}