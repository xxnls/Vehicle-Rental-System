using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BackOffice.Models;
using BackOffice.Models.Customers;
using BackOffice.Models.DTOs.Customers;
using BackOffice.Models.DTOs.Employees;
using BackOffice.Models.DTOs.FileSystem;
using BackOffice.Models.DTOs.Other;
using BackOffice.Models.DTOs.Rentals;
using BackOffice.Models.DTOs.Vehicles;
using BackOffice.ViewModels;
using BackOffice.ViewModels.Customers;
using BackOffice.ViewModels.Employees;
using BackOffice.ViewModels.Rentals;
using BackOffice.ViewModels.Vehicles;
using BackOffice.Views.Customers;
using BackOffice.Views.Employees;
using BackOffice.Views.Other;
using BackOffice.Views.Rentals;
using BackOffice.Views.Vehicles;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Xceed.Words.NET;

namespace BackOffice.ViewModels.Other
{
    public class ParametrizedDocumentViewModel : BaseListViewModel<DocumentDto>
    {
        #region Properties

        public List<Type> ModelTypes { get; set; } = [];

        private Type _selectedModelType;
        public Type SelectedModelType
        {
            get => _selectedModelType;
            set
            {
                _selectedModelType = value;
                OnPropertyChanged();
                ChangeParameters();
            }
        }

        private string _inputFilePath;
        public string InputFilePath
        {
            get => _inputFilePath;
            set
            {
                _inputFilePath = value;
                OnPropertyChanged();
                GenerateDocumentCommand.NotifyCanExecuteChanged();
            }
        }

        private string _outputFilePath;
        public string OutputFilePath
        {
            get => _outputFilePath;
            set
            {
                _outputFilePath = value;
                OnPropertyChanged();
                GenerateDocumentCommand.NotifyCanExecuteChanged();
            }
        }

        private SelectorDialogParameters _selectModelParameters;
        public SelectorDialogParameters SelectModelParameters
        {
            get => _selectModelParameters;
            set
            {
                _selectModelParameters = value;
                OnPropertyChanged();
            }
        }

        private CustomerDto _customerModel;
        public CustomerDto CustomerModel
        {
            get => _customerModel;
            set
            {
                _customerModel = value;
                OnPropertyChanged();
            }
        }

        private EmployeeDto _employeeDto;
        public EmployeeDto EmployeeDto
        {
            get => _employeeDto;
            set
            {
                _employeeDto = value;
                OnPropertyChanged();
            }
        }

        private AddressDto _addressDto;
        public AddressDto AddressDto
        {
            get => _addressDto;
            set
            {
                _addressDto = value;
                OnPropertyChanged();
            }
        }

        private RentalPlaceDto _rentalPlaceDto;
        public RentalPlaceDto RentalPlaceDto
        {
            get => _rentalPlaceDto;
            set
            {
                _rentalPlaceDto = value;
                OnPropertyChanged();
            }
        }

        private RentalDto _rentalDto;
        public RentalDto RentalDto
        {
            get => _rentalDto;
            set
            {
                _rentalDto = value;
                OnPropertyChanged();
            }
        }

        private VehicleDto _vehicleDto;
        public VehicleDto VehicleDto
        {
            get => _vehicleDto;
            set
            {
                _vehicleDto = value;
                OnPropertyChanged();
            }
        }

        #endregion


        // Commands
        public ICommand SelectInputFileCommand { get; }
        public ICommand SelectOutputFileCommand { get; }
        public RelayCommand GenerateDocumentCommand { get; }

        public ParametrizedDocumentViewModel() : base("FileSystem", "Parametrize Document")
        {
            // Initialize commands
            SelectInputFileCommand = new RelayCommand(SelectInputFile);
            SelectOutputFileCommand = new RelayCommand(SelectOutputFile);
            GenerateDocumentCommand = new RelayCommand(GenerateDocument, CanGenerateDocument);

            // Initialize model types
            ModelTypes.Add(typeof(CustomerDto));
            ModelTypes.Add(typeof(EmployeeDto));
            ModelTypes.Add(typeof(AddressDto));
            ModelTypes.Add(typeof(RentalPlaceDto));
            ModelTypes.Add(typeof(RentalDto));
            ModelTypes.Add(typeof(VehicleDto));
        }

        private void ChangeParameters()
        {
            if (SelectedModelType == typeof(CustomerDto))
            {
                SelectModelParameters = new SelectorDialogParameters
                {
                    SelectorViewModelType = typeof(CustomersViewModel),
                    SelectorView = new CustomersView(),
                    TargetProperty = result => CustomerModel = (CustomerDto)result,
                    Title = "Select Customer"
                };
            }
            if (SelectedModelType == typeof(EmployeeDto))
            {
                SelectModelParameters = new SelectorDialogParameters
                {
                    SelectorViewModelType = typeof(EmployeesViewModel),
                    SelectorView = new EmployeesView(),
                    TargetProperty = result => EmployeeDto = (EmployeeDto)result,
                    Title = "Select Employee"
                };
            }
            if (SelectedModelType == typeof(AddressDto))
            {
                SelectModelParameters = new SelectorDialogParameters
                {
                    SelectorViewModelType = typeof(AddressesViewModel),
                    SelectorView = new AddressesView(),
                    TargetProperty = result => AddressDto = (AddressDto)result,
                    Title = "Select Address"
                };
            }
            if (SelectedModelType == typeof(RentalPlaceDto))
            {
                SelectModelParameters = new SelectorDialogParameters
                {
                    SelectorViewModelType = typeof(RentalPlacesViewModel),
                    SelectorView = new RentalPlacesView(),
                    TargetProperty = result => RentalPlaceDto = (RentalPlaceDto)result,
                    Title = "Select Rental Place"
                };
            }
            if (SelectedModelType == typeof(RentalDto))
            {
                SelectModelParameters = new SelectorDialogParameters
                {
                    SelectorViewModelType = typeof(RentalsViewModel),
                    SelectorView = new RentalsView(),
                    TargetProperty = result => RentalDto = (RentalDto)result,
                    Title = "Select Rental"
                };
            }
            if (SelectedModelType == typeof(VehicleDto))
            {
                SelectModelParameters = new SelectorDialogParameters
                {
                    SelectorViewModelType = typeof(VehiclesViewModel),
                    SelectorView = new VehiclesView(),
                    TargetProperty = result => VehicleDto = (VehicleDto)result,
                    Title = "Select Vehicle"
                };
            }
        }

        private void SelectInputFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Word Documents|*.docx",
                Title = "Select a Template File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                InputFilePath = openFileDialog.FileName;
                OnPropertyChanged(nameof(InputFilePath));
            }
        }

        private void SelectOutputFile()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Documents|*.docx",
                Title = "Save the Generated Document"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                OutputFilePath = saveFileDialog.FileName;
                OnPropertyChanged(nameof(OutputFilePath));
            }
        }

        /// <summary>
        /// Generate a document using the selected model and the input and output files.
        /// </summary>
        private void GenerateDocument()
        {
            if (string.IsNullOrEmpty(InputFilePath) || string.IsNullOrEmpty(OutputFilePath))
            {
                MessageBox.Show("Please select input and output files.");
                return;
            }

            try
            {
                using (var document = DocX.Load(InputFilePath))
                {
                    // Get the selected model
                    var selectedModel = GetSelectedModel();

                    if (selectedModel == null)
                    {
                        MessageBox.Show("No model selected.");
                        return;
                    }

                    // Get the properties and values from the selected model
                    var properties = GetPropertyValues(selectedModel);

                    foreach (var property in properties)
                    {
                        document.ReplaceText($"<{property.Key}>", property.Value);
                    }

                    document.SaveAs(OutputFilePath);
                    MessageBox.Show("Document generated successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating document: {ex.Message}");
            }
        }

        private object GetSelectedModel()
        {
            if (CustomerModel != null) return CustomerModel;
            if (EmployeeDto != null) return EmployeeDto;
            if (AddressDto != null) return AddressDto;
            if (RentalPlaceDto != null) return RentalPlaceDto;
            if (RentalDto != null) return RentalDto;
            if (VehicleDto != null) return VehicleDto;

            return null;
        }


        // Get property names and values from an object
        private Dictionary<string, string> GetPropertyValues(object obj)
        {
            var properties = new Dictionary<string, string>();

            if (obj == null) return properties; // Handle null object

            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                var value = propertyInfo.GetValue(obj);

                // Handle null values and convert to string (or appropriate format)
                var stringValue = value?.ToString() ?? ""; // Use null-coalescing operator

                properties[propertyInfo.Name] = stringValue;
            }

            return properties;
        }


        private bool CanGenerateDocument()
        {
            return !string.IsNullOrEmpty(InputFilePath) && !string.IsNullOrEmpty(OutputFilePath);
        }

    }
}

