using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.ViewModels.Other;
using BackOffice.Views.Other;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using BackOffice.Models.DTOs.Other;
using BackOffice.Models.DTOs.Customers;
using BackOffice.Models.DTOs.Employees;
using BackOffice.ViewModels.Customers;
using BackOffice.ViewModels.Employees;
using BackOffice.Views.Customers;
using BackOffice.Views.Employees;
using System.Windows;
using BackOffice.Models.DTOs.FileSystem;
using System.Windows.Input;
using RTools_NTS.Util;

namespace BackOffice.ViewModels.Other
{
    public class LicenseApprovalRequestsViewModel : BaseListViewModel<LicenseApprovalRequestsDto>, IListViewModel
    {
        public SelectorDialogParameters SelectCustomerParameters { get; set; }
        public ICommand UploadFileFrontCommand { get; set; }
        public ICommand UploadFileBackCommand { get; set; }
        public ICommand ViewFileContentCommand { get; set; }

        private FileUploadDto _licenseFront = null!;
        public FileUploadDto LicenseFront
        {
            get => _licenseFront;
            set
            {
                _licenseFront = value;
                OnPropertyChanged();
            }
        }

        private FileUploadDto _licenseBack = null!;
        public FileUploadDto LicenseBack
        {
            get => _licenseBack;
            set
            {
                _licenseBack = value;
                OnPropertyChanged();
            }
        }

        public LicenseApprovalRequestsViewModel() : base("LicenseApprovalRequests", LocalizationHelper.GetString("LicenseApprovalRequests", "DisplayName"))
        {
            SelectCustomerParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(CustomersViewModel),
                SelectorView = new CustomersView(),
                TargetProperty = result =>
                {
                    EditableModel.Customer ??= new CustomerDto();
                    EditableModel.Customer = (CustomerDto)result;
                },
                Title = LocalizationHelper.GetString("LicenseApprovalRequests", "SelectCustomer")
            };


            RestoreModelCommand = new AsyncRelayCommand<int>(
                async id => await RestoreModelAsync(id, EditableModel), id => EditableModel != null);

            CreateModelCommand = new AsyncRelayCommand(() => CreateModelAsync(EditableModel));
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.LicenseApprovalRequestId, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.LicenseApprovalRequestId),
                () => EditableModel != null
            );
            UploadFileFrontCommand = new AsyncRelayCommand<string>(UploadFileAsync);
            UploadFileBackCommand = new AsyncRelayCommand<string>(UploadFileAsync);
            ViewFileContentCommand = new AsyncRelayCommand<int>(FileHelper.ViewFile);

            ValidationRules = new Dictionary<string, Action>();
        }

        private new async Task CreateModelAsync(LicenseApprovalRequestsDto model)
        {
            try
            {
                var resultFront = await ApiClient.PostAsync<FileUploadDto, DocumentDto>($"FileSystem/upload", LicenseFront);
                var resultBack = await ApiClient.PostAsync<FileUploadDto, DocumentDto>($"FileSystem/upload", LicenseBack);
                var user = (EmployeeDto)SessionManager.Get("User");
                resultFront.CreatedByEmployeeId = user.Id;
                resultFront.CreatedByEmployee = user;
                resultBack.CreatedByEmployeeId = user.Id;
                resultBack.CreatedByEmployee = user;
                EditableModel.DocumentFront = resultFront;
                EditableModel.DocumentBack = resultBack;

                await base.CreateModelAsync(model);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private new async Task UpdateModelAsync(int id, LicenseApprovalRequestsDto model)
        {
            try
            {
                var resultFront = await ApiClient.GetAsync<DocumentDto>($"FileSystem", model.DocumentFront.DocumentId);
                var resultBack = await ApiClient.GetAsync<DocumentDto>($"FileSystem", model.DocumentBack.DocumentId);
                var user = (EmployeeDto)SessionManager.Get("User");
                resultFront.CreatedByEmployeeId = user.Id;
                resultFront.CreatedByEmployee = user;
                resultBack.CreatedByEmployeeId = user.Id;
                resultBack.CreatedByEmployee = user;
                model.DocumentFront = resultFront;
                model.DocumentBack = resultBack;
                await base.UpdateModelAsync(id, model);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private new async Task RestoreModelAsync(int id, LicenseApprovalRequestsDto model)
        {
            model.DeletedDate = null;
            model.IsActive = true;

            await UpdateModelAsync(id, model);
        }

        private async Task UploadFileAsync(string type)
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

                    var user = (EmployeeDto)SessionManager.Get("User");

                    if (type == "front")
                    {
                        LicenseFront = new FileUploadDto
                        {
                            FileContent = fileContent,
                            FileName = fileName,
                            Title = $"LicenseFront_{EditableModel.Customer.FirstName}_{EditableModel.Customer.LastName}",
                            DocumentCategoryId = 3,
                            DocumentTypeId = 2,
                            CustomerId = EditableModel.Customer.Id,
                            CreatedByEmployeeId = user.Id,
                        };
                    }
                    else
                    {
                        LicenseBack = new FileUploadDto
                        {
                            FileContent = fileContent,
                            FileName = fileName,
                            Title = $"LicenseBack_{EditableModel.Customer.FirstName}_{EditableModel.Customer.LastName}",
                            DocumentCategoryId = 3,
                            DocumentTypeId = 2,
                            CustomerId = EditableModel.Customer.Id,
                            CreatedByEmployeeId = user.Id,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file access errors)
                MessageBox.Show($"An error occurred while selecting the file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}