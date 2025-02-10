using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.Models.DTOs.Vehicles;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels.Vehicles
{
    public class VehicleMaintenanceViewModel : BaseListViewModel<VehicleDto>, IBaseListViewModel
    {
        public VehicleMaintenanceViewModel() : base("Vehicles", LocalizationHelper.GetString("Vehicles", "DisplayNameMaintenance"))
        {
            LoadModelsCommand = new RelayCommand(async () => await LoadModelsAsync());
            MarkMaintainedCommand = new AsyncRelayCommand(() => ChangeVehicleStatus(1));
            SendToServiceCommand = new AsyncRelayCommand(() => ChangeVehicleStatus(4));

            ValidationRules = new Dictionary<string, Action>();
        }

        #region Methods

        protected override async Task LoadModelsAsync(string? searchInput = null)
        {
            try
            {
                IsBusy = true;

                CurrentSearchInput = searchInput;

                _ = !string.IsNullOrWhiteSpace(searchInput) ? CurrentPage = 1 : CurrentPage;


                var endpoint =
                    $"Vehicles/maintenance?page={CurrentPage}&pageSize={PageSize}";

                if (!string.IsNullOrWhiteSpace(searchInput))
                {
                    endpoint += $"&search={CurrentSearchInput}";
                }

                if (CreatedBefore.HasValue)
                    endpoint += $"&createdBefore={CreatedBefore.Value:yyyy-MM-dd}";
                if (CreatedAfter.HasValue)
                    endpoint += $"&createdAfter={CreatedAfter.Value:yyyy-MM-dd}";
                if (ModifiedBefore.HasValue)
                    endpoint += $"&modifiedBefore={ModifiedBefore.Value:yyyy-MM-dd}";
                if (ModifiedAfter.HasValue)
                    endpoint += $"&modifiedAfter={ModifiedAfter.Value:yyyy-MM-dd}";


                var results =
                    await ApiClient.GetAsync<PaginatedResult<VehicleDto>>(endpoint);

                Models.Clear();
                if (results?.Items != null)
                {
                    foreach (var result in results.Items)
                    {
                        Models.Add(result);
                    }
                }

                TotalItemCount = results?.TotalItemCount ?? 0;

            }
            catch (Exception ex)
            {
                UpdateStatus(string.IsNullOrWhiteSpace(searchInput)
                    ? LocalizationHelper.GetString("Generic", "USLoadError1") + $"{ex.Message}"
                    : LocalizationHelper.GetString("Generic", "USLoadError2") + $"{ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ChangeVehicleStatus(int statusId)
        {
            try
            {
                await ApiClient.PutAsync($"Vehicles/status/{EditableModel.VehicleId}/{statusId}");
                await LoadModelsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while changing vehicle status.");
            }
        }

        #endregion

    }
}
