using BackOffice.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BackOffice.Models.Other;
using BackOffice.Helpers;
using CommunityToolkit.Mvvm.Messaging;
using System.Net;
using BackOffice.Models.Employees.DTOs;
using BackOffice.Models.Vehicles.VehicleBrands.DTOs;

namespace BackOffice.ViewModels.Other
{
    public class LoginViewModel : BaseListViewModel<RVehicleBrandDTO>
    {
        #region Properties & Fields

        private readonly ApiClient ApiClient;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                    ValidateUsername();
                    ((IRelayCommand)LoginCommand).NotifyCanExecuteChanged();
                }
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                    ValidatePassword();
                    ((IRelayCommand)LoginCommand).NotifyCanExecuteChanged();
                }
            }
        }

        #endregion

        // Commands
        public ICommand LoginCommand { get; }

        // Constructor
        public LoginViewModel() : base("EmployeeAuth", "Login")
        {
            ApiClient = new ApiClient();

            LoginCommand = new RelayCommand(async () => await LoginAsync(), CanLogin);
        }

        // Methods

        public async Task LoginAsync()
        {
            IsBusy = true;

            try
            {
                var loginData = new LoginCredentials
                {
                    Username = Username,
                    Password = Password
                };

                var response = await ApiClient.PostAsync<LoginCredentials, LoginResponse>($"{EndPointName}/login", loginData);

                // Store the token
                if (!string.IsNullOrEmpty(response.Token))
                {
                    ApiClient.SetAuthorizationHeader(response.Token);

                    // Store the user in the session
                    var user = await ApiClient.GetAsync<REmployeeDTO>($"Employees/{response.UserId}");
                    SessionManager.Set("User", user);

                    // Notify successful login
                    WeakReferenceMessenger.Default.Send(new Messenger("LoginSuccessful"));
                }
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("401"))
            {
                MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #region Validation

        private void ValidateUsername()
        {
            ValidateProperty(
                nameof(Username),
                () => !string.IsNullOrWhiteSpace(Username),
                "Username cannot be empty."
            );
        }

        private void ValidatePassword()
        {
            ValidateProperty(
                nameof(Password),
                () => !string.IsNullOrWhiteSpace(Password) && Password.Length >= 3,
                "Password must be at least 3 characters long."
            );
        }

        private bool CanLogin()
        {
            // Allow login only when there are no validation errors and there is no active process
            return !HasErrors && !IsBusy;
        }

        #endregion
    }
}
