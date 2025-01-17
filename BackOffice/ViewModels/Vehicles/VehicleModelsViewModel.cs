using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models.DTOs.Vehicles;
using BackOffice.Models.Vehicles.VehicleBrands;
using BackOffice.Models.Vehicles.VehicleBrands.DTOs;
using BackOffice.Views;
using BackOffice.Views.CustomControls;
using BackOffice.Views.Vehicles;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels.Vehicles
{
    public class VehicleModelsViewModel : BaseListViewModel<VehicleModelDto>, IListViewModel
    {
        public ICommand ShowSelectorDialogCommand { get; }

        public VehicleModelsViewModel() : base("VehicleModels", LocalizationHelper.GetString("VehicleModels", "DisplayName"))
        {
            CreateModelCommand = new AsyncRelayCommand(CreateModelAsync);
            UpdateModelCommand = new AsyncRelayCommand(UpdateModelAsync);
            DeleteModelCommand = new AsyncRelayCommand(() => DeleteModelAsync(EditableModel.VehicleModelId));
            ShowSelectorDialogCommand = new RelayCommand(ShowSelectorDialog);

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.Name), ValidateName },
                { nameof(EditableModel.Description), ValidateDescription},
                { nameof(EditableModel.EngineSize), ValidateEngineSize },
                { nameof(EditableModel.HorsePower), ValidateHorsePower },
                { nameof(EditableModel.FuelType), ValidateFuelType },
                { nameof(EditableModel.VehicleBrandId), ValidateVehicleBrandId }
            };
        }

        #region Methods

        public async Task CreateModelAsync()
        {
            var model = new VehicleModelDto
            {
                VehicleBrandId = EditableModel.VehicleBrandId,
                Name = EditableModel.Name,
                Description = EditableModel.Description,
                EngineSize = EditableModel.EngineSize,
                HorsePower = EditableModel.HorsePower,
                FuelType = EditableModel.FuelType
            };

            await CreateModelAsync(model);
        }

        public async Task UpdateModelAsync()
        {
            var id = EditableModel.VehicleModelId;

            var model = new VehicleModelDto
            {
                VehicleModelId = EditableModel.VehicleModelId,
                VehicleBrandId = EditableModel.VehicleBrandId,
                Name = EditableModel.Name,
                Description = EditableModel.Description,
                EngineSize = EditableModel.EngineSize,
                HorsePower = EditableModel.HorsePower,
                FuelType = EditableModel.FuelType
            };
            await UpdateModelAsync(id, model);
        }

        private void ShowSelectorDialog()
        {
            var dialog = new DialogWindow();
            var viewModel = new VehicleBrandsViewModel();
            var view = new VehicleBrandsView();
            dialog.DataContext = viewModel;

            var originalGrid = view.FindName("MainDataGrid") as DataGrid;
            if (originalGrid == null) return;

            var newGrid = new DataGrid
            {
                ItemsSource = originalGrid.ItemsSource,
                Style = originalGrid.Style,
                AutoGenerateColumns = originalGrid.AutoGenerateColumns
            };

            foreach (var column in originalGrid.Columns)
            {
                var newColumn = new DataGridTextColumn
                {
                    Header = column.Header,
                    Binding = ((DataGridTextColumn)column).Binding
                };
                newGrid.Columns.Add(newColumn);
            }

            newGrid.MouseDoubleClick += (sender, e) =>
            {
                if (newGrid.SelectedItem != null)
                {
                    EditableModel.VehicleBrandId = (int)newGrid.SelectedItem.GetType()
                        .GetProperty("VehicleBrandId")
                        .GetValue(newGrid.SelectedItem);

                    dialog.Close();
                }
            };

            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(viewModel.CurrentPage))
                {
                    // Reload the content of the dialog when the page changes
                    // RefreshDialog(dialog, newGrid, viewModel);
                }
            };

            // Create and set the PaginationControl, if necessary
            //var paginationControl = new PaginationControl();
            //paginationControl.DataContext = viewModel;  // Set the same DataContext to bind commands and properties
            //paginationControl.CurrentPage = viewModel.CurrentPage;
            //paginationControl.TotalItemCount = viewModel.TotalItemCount;
            //paginationControl.ModelsPerPage = viewModel.PageSize;
            //paginationControl.CanLoadPreviousPage = viewModel.CanLoadPreviousPage;
            //paginationControl.CanLoadNextPage = viewModel.CanLoadNextPage;
            //paginationControl.PreviousPageCommand = viewModel.LoadPreviousPageCommand;
            //paginationControl.NextPageCommand = viewModel.LoadNextPageCommand;


            // Set ContentPresenter content to both DataGrid and PaginationControl
            //var stackPanel = new StackPanel();
            //stackPanel.Children.Add(newGrid);
            //stackPanel.Children.Add(paginationControl);

            dialog.ContentPresenter.Content = newGrid;
            dialog.ShowDialog();
        }

        #endregion

        #region Validation
        private void ValidateName()
        {
            ValidateProperty(nameof(EditableModel.Name),
                () => !string.IsNullOrWhiteSpace(EditableModel.Name) && EditableModel.Name.Length >= 3,
                LocalizationHelper.GetString("VehicleModels", "ErrorName"));
        }

        private void ValidateDescription()
        {
            ValidateProperty(nameof(EditableModel.Description),
                () => string.IsNullOrWhiteSpace(EditableModel.Description) || EditableModel.Description.Length <= 250,
                LocalizationHelper.GetString("VehicleModels", "ErrorDescription"));
        }

        private void ValidateEngineSize()
        {
            //ValidateProperty(nameof(EditableModel.EngineSize),
            //    () =>
            //    {
            //        // Check if EngineSize is a valid number and greater than 0
            //        return EditableModel.EngineSize.HasValue && EditableModel.EngineSize.Value > 0;
            //    },
            //    LocalizationHelper.GetString("VehicleModels", "ErrorEngineSize"));
        }

        private void ValidateHorsePower()
        {
            //ValidateProperty(nameof(EditableModel.HorsePower),
            //    () =>
            //    {
            //        // Check if HorsePower is a valid number and greater than 0
            //        return EditableModel.HorsePower.HasValue && EditableModel.HorsePower.Value > 0;
            //    },
            //    LocalizationHelper.GetString("VehicleModels", "ErrorHorsePower"));
        }

        private void ValidateFuelType()
        {
            ValidateProperty(nameof(EditableModel.FuelType),
                () => !string.IsNullOrWhiteSpace(EditableModel.FuelType) && EditableModel.FuelType.Length <= 10,
                LocalizationHelper.GetString("VehicleModels", "ErrorFuelType"));
        }

        private void ValidateVehicleBrandId()
        {
            ValidateProperty(nameof(EditableModel.VehicleBrandId),
                () => EditableModel.VehicleBrandId > 0,
                LocalizationHelper.GetString("VehicleModels", "ErrorVehicleBrandId"));
        }


        #endregion
    }
}
