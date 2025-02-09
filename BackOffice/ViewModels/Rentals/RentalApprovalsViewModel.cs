using System.Windows;
using System.Windows.Input;
using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.Models.DTOs.Rentals;
using BackOffice.Models.DTOs.Vehicles;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels.Rentals
{
    public class RentalApprovalsViewModel : BaseListViewModel<RentalRequestDto>, IBaseListViewModel
    {
        public RentalApprovalsViewModel() : base("RentalRequests",
            LocalizationHelper.GetString("RentalRequests", "DisplayName"))
        {
            LoadModelsCommand = new RelayCommand(async () => await LoadModelsAsync());
            ApproveRequestCommand =
                new AsyncRelayCommand(() => UpdateRequestStatusAsync(EditableModel, RentalRequestStatus.Approved),
                    () => EditableModel != null);
            RejectRequestCommand =
                new AsyncRelayCommand(() => UpdateRequestStatusAsync(EditableModel, RentalRequestStatus.Rejected),
                    () => EditableModel != null);
            CancelRequestCommand =
                new AsyncRelayCommand(() => UpdateRequestStatusAsync(EditableModel, RentalRequestStatus.Cancelled),
                    () => EditableModel != null);

            ValidationRules = new Dictionary<string, Action>();
        }

        //private async Task UpdateRequestStatusAsync(RentalRequestDto? request, RentalRequestStatus status)
        //{
        //    if (request == null)
        //        return;

        //    request.RequestStatus = status.ToString();
        //    await UpdateModelAsync(request.RentalRequestId, request);
        //}

        private async Task UpdateRequestStatusAsync(RentalRequestDto? request, RentalRequestStatus status)
        {
            if (request == null)
                return;

            if (status == RentalRequestStatus.Approved)
            {
                try
                {
                    if (!await CanApproveRentalRequestAsync(request))
                    {
                        // Show dialog using MessageBox
                        MessageBox.Show(
                            LocalizationHelper.GetString("RentalRequests", "CannotApprove"),
                            LocalizationHelper.GetString("RentalRequests", "ErrorCheckingAvailability"),
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }

                    // Approve the request
                    await ApiClient.PutAsync($"RentalRequests/{request.RentalRequestId}/approve");
                }
                catch (Exception ex)
                {
                    // Show error dialog
                    MessageBox.Show(
                        LocalizationHelper.GetString("RentalRequests", "ErrorCheckingAvailability") +
                        $" {ex.Message}", // Localized message
                        LocalizationHelper.GetString("Generic", "Error"), // Localized title
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                // Reject or cancel the request
                request.RequestStatus = status.ToString();
                await UpdateModelAsync(request.RentalRequestId, request);
            }

            // Refresh the list
            await LoadModelsAsync(CurrentSearchInput);
        }

        private async Task<bool> CanApproveRentalRequestAsync(RentalRequestDto request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                IsBusy = true;

                // Get the vehicle
                var vehicle = await ApiClient.GetAsync<VehicleDto>($"Vehicles/{request.VehicleId}");

                if (vehicle == null)
                {
                    return false;
                }

                // Check both IsAvailableForRent and VehicleStatusId
                return vehicle is { IsAvailableForRent: true, VehicleStatus.StatusName: "Available" };
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override async Task LoadModelsAsync(string? searchInput = null)
        {
            try
            {
                IsBusy = true;

                CurrentSearchInput = searchInput;

                _ = !string.IsNullOrWhiteSpace(searchInput) ? CurrentPage = 1 : CurrentPage;


                var endpoint =
                    $"RentalRequests/pending?page={CurrentPage}&pageSize={PageSize}";

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
                    await ApiClient.GetAsync<PaginatedResult<RentalRequestDto>>(endpoint);

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
    }
}
