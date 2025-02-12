using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.ViewModels.Other;
using BackOffice.Views.Other;
using BackOffice.Views.FileSystem;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

namespace BackOffice.ViewModels.FileSystem
{
    public class FilesViewModel : BaseListViewModel<DocumentDto>, IListViewModel
    {
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
                Title = LocalizationHelper.GetString("Files", "SelectDocumentType")
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

            // Validation rules
            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.Title), ValidateTitle },
                { nameof(EditableModel.Description), ValidateDescription },
                { nameof(EditableModel.FileName), ValidateFileName },
                { nameof(EditableModel.FileContent), ValidateFileContent },
                { nameof(EditableModel.DocumentType), ValidateDocumentType },
                { nameof(EditableModel.DocumentCategory), ValidateDocumentCategory },
                { nameof(EditableModel.Customer), ValidateCustomer },
                { nameof(EditableModel.Employee), ValidateEmployee },
                { nameof(EditableModel.Vehicle), ValidateVehicle },
                { nameof(EditableModel.RentalPlace), ValidateRentalPlace },
                { nameof(EditableModel.Rental), ValidateRental }
            };
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

        private void ValidateFileName()
        {
            ClearErrors(nameof(EditableModel.FileName));

            if (string.IsNullOrWhiteSpace(EditableModel.FileName))
            {
                AddError(nameof(EditableModel.FileName), LocalizationHelper.GetString("Files", "ErrorFileName1"));
            }
            else if (EditableModel.FileName.Length > 200)
            {
                AddError(nameof(EditableModel.FileName), LocalizationHelper.GetString("Files", "ErrorFileName2"));
            }
        }

        private void ValidateFileContent()
        {
            ClearErrors(nameof(EditableModel.FileContent));

            if (EditableModel.FileContent == null || EditableModel.FileContent.Length == 0)
            {
                AddError(nameof(EditableModel.FileContent), LocalizationHelper.GetString("Files", "ErrorFileContent1"));
            }
        }

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

        private void ValidateCustomer()
        {
            ClearErrors(nameof(EditableModel.Customer));

            if (EditableModel.Customer == null && EditableModel.CustomerId.HasValue)
            {
                AddError(nameof(EditableModel.Customer), LocalizationHelper.GetString("Files", "ErrorCustomer1"));
            }
        }

        private void ValidateEmployee()
        {
            ClearErrors(nameof(EditableModel.Employee));

            if (EditableModel.Employee == null && EditableModel.EmployeeId.HasValue)
            {
                AddError(nameof(EditableModel.Employee), LocalizationHelper.GetString("Files", "ErrorEmployee1"));
            }
        }

        private void ValidateVehicle()
        {
            ClearErrors(nameof(EditableModel.Vehicle));

            if (EditableModel.Vehicle == null && EditableModel.VehicleId.HasValue)
            {
                AddError(nameof(EditableModel.Vehicle), LocalizationHelper.GetString("Files", "ErrorVehicle1"));
            }
        }

        private void ValidateRentalPlace()
        {
            ClearErrors(nameof(EditableModel.RentalPlace));

            if (EditableModel.RentalPlace == null && EditableModel.RentalPlaceId.HasValue)
            {
                AddError(nameof(EditableModel.RentalPlace), LocalizationHelper.GetString("Files", "ErrorRentalPlace1"));
            }
        }

        private void ValidateRental()
        {
            ClearErrors(nameof(EditableModel.Rental));

            if (EditableModel.Rental == null && EditableModel.RentalId.HasValue)
            {
                AddError(nameof(EditableModel.Rental), LocalizationHelper.GetString("Files", "ErrorRental1"));
            }
        }
        #endregion
    }
}