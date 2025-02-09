using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.Models.DTOs.Rentals;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels.Rentals
{
    public class PickupsViewModel : BaseListViewModel<RentalDto>, IBaseListViewModel
    {
        public PickupsViewModel() : base("Rentals",
            LocalizationHelper.GetString("Rentals", "DisplayNamePickups"))
        {
            LoadModelsCommand = new RelayCommand(async () => await LoadModelsAsync());

            MarkPickupCommand =
                new AsyncRelayCommand(() => MarkPickup(EditableModel),
                    () => EditableModel != null);

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
                    $"Rentals/awaiting?page={CurrentPage}&pageSize={PageSize}";

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
                    await ApiClient.GetAsync<PaginatedResult<RentalDto>>(endpoint);

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

        private async Task<bool> MarkPickup(RentalDto rental)
        {
            try
            {
                if (EditableModel == null)
                    return false;

                // Check if the rental is awaiting pickup
                if (EditableModel.RentalStatus != RentalStatus.AwaitingPickup.ToString())
                {
                    MessageBox.Show(
                        LocalizationHelper.GetString("Rentals", "ErrorMarkingPickupText"),
                        LocalizationHelper.GetString("Rentals", "ErrorMarkingPickup"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return false;
                }

                // Check if the payment is completed
                if (EditableModel.PaymentStatus != PaymentStatus.Completed.ToString())
                {
                    MessageBox.Show(
                        LocalizationHelper.GetString("Rentals", "ErrorMarkingPickupPaymentText"),
                        LocalizationHelper.GetString("Rentals", "ErrorMarkingPickup"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return false;
                }

                // Mark the rental as picked up
                await ApiClient.PutAsync("Rentals/mark-pickup", rental);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    LocalizationHelper.GetString("Rentals", "ErrorMarkingPickup") +
                    $" {e.Message}",
                    LocalizationHelper.GetString("Generic", "Error"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
            finally
            {
                // Refresh the list
                await LoadModelsAsync(CurrentSearchInput);
            }

        }

        #endregion

    }
}
