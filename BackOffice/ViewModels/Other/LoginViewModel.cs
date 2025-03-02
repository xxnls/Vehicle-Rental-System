﻿using BackOffice.Services;
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
using BackOffice.Models.DTOs.Employees;
using BackOffice.Models.Vehicles.VehicleBrands.DTOs;

namespace BackOffice.ViewModels.Other
{
    public class LoginViewModel : BaseListViewModel<VehicleBrandDto>
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

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(Username), ValidateUsername },
                { nameof(Password), ValidatePassword }
            };
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
                    var user = await ApiClient.GetAsync<EmployeeDto>($"Employees/{response.UserId}");
                    SessionManager.Set("User", user);

                    // Store user roles in the session
                    var roles = await ApiClient.GetAsync<List<string>>($"EmployeeRoles/user/{response.UserId}/roles");
                    SessionManager.Set("Roles", roles);

                    // Notify successful login
                    WeakReferenceMessenger.Default.Send(new Messenger("LoginSuccessful"));
                }
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("401"))
            {
                MessageBox.Show(LocalizationHelper.GetString("LoginWindow", "ErrorAuth1"),
                    LocalizationHelper.GetString("LoginWindow", "ErrorAuth2"), MessageBoxButton.OK, MessageBoxImage.Error);
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
                LocalizationHelper.GetString("LoginWindow", "ErrorUsername"));
        }

        private void ValidatePassword()
        {
            ValidateProperty(
                nameof(Password),
                () => !string.IsNullOrWhiteSpace(Password) && Password.Length >= 3,
                LocalizationHelper.GetString("LoginWindow", "ErrorPassword"));
        }

        private bool CanLogin()
        {
            // Allow login only when there are no validation errors and there is no active process
            return !HasErrors && !IsBusy;
        }

        #endregion
    }
}
