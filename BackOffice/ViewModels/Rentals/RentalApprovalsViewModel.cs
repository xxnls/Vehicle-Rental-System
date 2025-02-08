using System.Windows.Input;
using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.Models.DTOs.Rentals;
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

        private async Task UpdateRequestStatusAsync(RentalRequestDto? request, RentalRequestStatus status)
        {
            if (request == null)
                return;

            request.RequestStatus = status.ToString();
            await UpdateModelAsync(request.RentalRequestId, request);
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
