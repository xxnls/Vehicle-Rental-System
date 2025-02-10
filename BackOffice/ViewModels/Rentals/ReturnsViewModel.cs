using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.Models.DTOs.Employees;
using BackOffice.Models.DTOs.Rentals;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels.Rentals
{
    public class ReturnsViewModel : BaseListViewModel<RentalDto>, IBaseListViewModel
    {
        private PostRentalReportDto _postRentalReport = new PostRentalReportDto();
        public PostRentalReportDto PostRentalReport
        {
            get => _postRentalReport;
            set
            {
                if (_postRentalReport != value)
                {
                    _postRentalReport = value;
                    OnPropertyChanged();
                }
            }
        }



        public ReturnsViewModel() : base("Rentals",
            LocalizationHelper.GetString("Rentals", "DisplayNameReturns"))
        {
            LoadModelsCommand = new RelayCommand(async () => await LoadModelsAsync());

            MarkReturnCommand =
                new AsyncRelayCommand(() => MarkReturn(EditableModel), () => EditableModel != null);

            MarkReturnCommandWithIssues =
                new AsyncRelayCommand(() => MarkReturnWithIssues(EditableModel, PostRentalReport), () => EditableModel != null);

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.DepositDeduction), ValidateDepositDeduction }
            };
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
                    $"Rentals/inprogress?page={CurrentPage}&pageSize={PageSize}";

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

        private async Task<bool> MarkReturn(RentalDto rental)
        {
            try
            {
                if (EditableModel == null)
                    return false;

                if (SessionManager.Get("User") != null)
                {
                    rental.FinishedByEmployee = (EmployeeDto?)SessionManager.Get("User");
                }
                else
                {
                    return false;
                }

                if (EditableModel.RentalStatus != RentalStatus.InProgress.ToString())
                {
                    MessageBox.Show(
                        LocalizationHelper.GetString("Rentals", "ErrorMarkingReturnText"),
                        LocalizationHelper.GetString("Rentals", "ErrorMarkingReturn"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return false;
                }

                // Create PostRentalReportDto
                var postRentalReport = new PostRentalReportDto
                {
                    InspectorEmployee = rental.FinishedByEmployee,
                    IsCarDamaged = false,
                    IsCarRefueled = false,
                    IsCustomerLate = false
                };

                rental.PostRentalReport = postRentalReport;

                // Fully refund the deposit
                rental.DepositStatus = DepositStatus.FullyRefunded.ToString();
                rental.DepositRefundAmount = rental.DepositAmount;

                // Cost hasn't changed, so set it to the final cost
                rental.FinalCost = rental.Cost;

                // Mark the rental as picked up
                await ApiClient.PutAsync("Rentals/mark-return", rental);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    LocalizationHelper.GetString("Rentals", "ErrorMarkingReturn") +
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

        private async Task<bool> MarkReturnWithIssues(RentalDto rental, PostRentalReportDto report)
        {
            try
            {
                if (EditableModel == null)
                    return false;

                if (SessionManager.Get("User") != null)
                {
                    var user = (EmployeeDto?)SessionManager.Get("User");
                    rental.FinishedByEmployee = user;
                    report.InspectorEmployee = user;
                }
                else
                {
                    return false;
                }

                if (EditableModel.RentalStatus != RentalStatus.InProgress.ToString())
                {
                    MessageBox.Show(
                        LocalizationHelper.GetString("Rentals", "ErrorMarkingReturnText"),
                        LocalizationHelper.GetString("Rentals", "ErrorMarkingReturn"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return false;
                }

                // Deposit deduction and status Logic
                var actualDeduction = Math.Min(EditableModel.DepositDeduction, rental.DepositAmount);

                rental.DepositRefundAmount = rental.DepositAmount - actualDeduction;


                if (actualDeduction > 0) // There was a deduction
                {
                    if (actualDeduction == rental.DepositAmount) // All deposit taken
                    {
                        rental.DepositStatus = DepositStatus.FullyTaken.ToString();

                    }
                    else if (rental.DepositRefundAmount == 0)
                    {
                        rental.DepositStatus = DepositStatus.FullyRefunded.ToString(); // All deposit was returned.
                    }
                    else
                    {
                        rental.DepositStatus = DepositStatus.PartiallyRefunded.ToString();
                    }
                }
                else // No deposit deduction
                {
                    rental.DepositStatus = DepositStatus.FullyRefunded.ToString(); // All deposit was returned.
                    rental.DepositRefundAmount = rental.DepositAmount;
                }

                rental.PostRentalReport = report;
                rental.DamageFee ??= 0;

                rental.FinalCost = rental.Cost + rental.DamageFee + actualDeduction;

                // Mark the rental as returned
                await ApiClient.PutAsync("Rentals/mark-return", rental);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    LocalizationHelper.GetString("Rentals", "ErrorMarkingReturn") +
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
                SwitchViewMode(ViewMode.List);
            }
        }

        #endregion

        #region Validation

        // Validate deposit deduction
        private void ValidateDepositDeduction()
        {
            ClearErrors(nameof(EditableModel.DepositDeduction));
            if (EditableModel.DepositDeduction > EditableModel.DepositAmount)
            {
                AddError(nameof(EditableModel.DepositDeduction), LocalizationHelper.GetString("Rentals", "ErrorDepositDeduction1"));
            }
        }

        #endregion
    }
}
