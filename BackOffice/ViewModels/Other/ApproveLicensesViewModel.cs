using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.Models.DTOs.Employees;
using BackOffice.Models.DTOs.FileSystem;
using BackOffice.Models.DTOs.Other;
using BackOffice.Models.DTOs.Rentals;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels.Other
{
    public class ApproveLicensesViewModel : BaseListViewModel<LicenseApprovalRequestsDto>, IBaseListViewModel
    {
        public ICommand ViewFileContentCommand { get; set; }
        public ApproveLicensesViewModel() : base("LicenseApprovalRequests", LocalizationHelper.GetString("LicenseApprovalRequests", "DisplayName"))
        {
            LoadModelsCommand = new RelayCommand(async () => await LoadModelsAsync());
            ApproveRequestCommand =
                new AsyncRelayCommand(() => UpdateRequestStatusAsync(EditableModel, RequestStatus.Approved),
                    () => EditableModel != null);
            RejectRequestCommand =
                new AsyncRelayCommand(() => UpdateRequestStatusAsync(EditableModel, RequestStatus.Rejected),
                    () => EditableModel != null);
            CancelRequestCommand =
                new AsyncRelayCommand(() => UpdateRequestStatusAsync(EditableModel, RequestStatus.Cancelled),
                    () => EditableModel != null);

            ViewFileContentCommand = new AsyncRelayCommand<int>(FileHelper.ViewFile);

            ValidationRules = new Dictionary<string, Action>();
        }

        private async Task UpdateRequestStatusAsync(LicenseApprovalRequestsDto request, RequestStatus status)
        {
            var user = new EmployeeDto();
            if (SessionManager.Get("User") != null)
            {
                user = (EmployeeDto?)SessionManager.Get("User");
                request.ApprovedByEmployee = user;
                request.ApprovedByEmployeeId = user.Id;
            }
            else
            {
                return;
            }

            switch (status)
            {
                case RequestStatus.Approved:
                    await ApiClient.PutAsync($"LicenseApprovalRequests/approve/{request.LicenseApprovalRequestId}/{request.ApprovedByEmployeeId}");
                    await LoadModelsAsync(CurrentSearchInput);
                    return;
                case RequestStatus.Rejected:
                    request.RequestStatus = RequestStatus.Rejected.ToString();
                    break;
                case RequestStatus.Cancelled:
                    request.RequestStatus = RequestStatus.Cancelled.ToString();
                    break;
            }

            var resultFront = await ApiClient.GetAsync<DocumentDto>($"FileSystem", request.DocumentFront.DocumentId);
            var resultBack = await ApiClient.GetAsync<DocumentDto>($"FileSystem", request.DocumentBack.DocumentId);
            resultFront.CreatedByEmployeeId = user.Id;
            resultFront.CreatedByEmployee = user;
            resultBack.CreatedByEmployeeId = user.Id;
            resultBack.CreatedByEmployee = user;
            request.DocumentFront = resultFront;
            request.DocumentBack = resultBack;
            await UpdateModelAsync(request.LicenseApprovalRequestId, request);

            // Refresh the list
            await LoadModelsAsync(CurrentSearchInput);
        }

        protected override async Task LoadModelsAsync(string? searchInput = null)
        {
            try
            {
                IsBusy = true;

                CurrentSearchInput = searchInput;

                _ = !string.IsNullOrWhiteSpace(searchInput) ? CurrentPage = 1 : CurrentPage;


                var endpoint =
                    $"LicenseApprovalRequests/pending?page={CurrentPage}&pageSize={PageSize}";

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
                    await ApiClient.GetAsync<PaginatedResult<LicenseApprovalRequestsDto>>(endpoint);

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
