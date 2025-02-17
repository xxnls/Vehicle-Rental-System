using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Properties;
using System.Windows;
using BackOffice.Helpers;

namespace BackOffice.ViewModels.Other
{
    public class SettingsViewModel : BaseViewModel
    {
        // Property for ModelsPerPage
        public int ModelsPerPage
        {
            get => Settings.Default.ModelsPerPage;
            set
            {
                if (Settings.Default.ModelsPerPage != value)
                {
                    Settings.Default.ModelsPerPage = value;
                    OnPropertyChanged();
                }
            }
        }

        // Command to save settings
        public RelayCommand SaveSettingsCommand { get; }
        public RelayCommand<string> ChangeLanguageCommand { get; }

        private Visibility _saveConfirmationVisibility = Visibility.Collapsed;
        public Visibility SaveConfirmationVisibility
        {
            get => _saveConfirmationVisibility;
            set
            {
                _saveConfirmationVisibility = value;
                OnPropertyChanged();
            }
        }

        public SettingsViewModel()
        {
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            ChangeLanguageCommand = new RelayCommand<string>(LocalizationHelper.SetLanguage);
        }

        private async void SaveSettings()
        {
            Settings.Default.Save();
            ShowSaveConfirmation();
        }


        private async void ShowSaveConfirmation()
        {
            SaveConfirmationVisibility = Visibility.Visible;
            await Task.Delay(3000); // Show for 3 seconds
            SaveConfirmationVisibility = Visibility.Collapsed;
        }

    }
}
