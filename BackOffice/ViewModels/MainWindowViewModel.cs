using BackOffice.ViewModels.Other.Locations;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackOffice.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Properties & Fields
        private readonly Dictionary<string, object> _viewModelMappings;

        private double _sidebarWidth = 200;

        public double SidebarWidth
        {
            get => _sidebarWidth;
            set
            {
                if (_sidebarWidth != value)
                {
                    _sidebarWidth = value;
                    OnPropertyChanged(nameof(SidebarWidth));
                }
            }
        }

        private object _currentWorkspace;

        public object CurrentWorkspace
        {
            get => _currentWorkspace;
            set
            {
                _currentWorkspace = value;
                OnPropertyChanged(nameof(CurrentWorkspace));
            }
        }
        #endregion

        #region Commands
        public ICommand ToggleSidebarCommand { get; }
        public ICommand ChangeWorkspaceCommand { get; }

        #endregion

        public MainWindowViewModel()
        {
            // Initialize mappings between string and view models
            _viewModelMappings = new Dictionary<string, object>
            {
                { "VehicleBrandsViewModel", new VehicleBrandsViewModel() },
                { "LocationsViewModel", new LocationsViewModel() }
            };

            // Set default workspace
            CurrentWorkspace = _viewModelMappings["VehicleBrandsViewModel"];

            ToggleSidebarCommand = new RelayCommand(ToggleSidebar);
            ChangeWorkspaceCommand = new RelayCommand<object>(ChangeWorkspace);
        }

        private void ToggleSidebar()
        {
            SidebarWidth = SidebarWidth == 200 ? 52 : 200;
        }

        private void ChangeWorkspace(object parameter)
        {
            if (parameter is string viewModelKey && _viewModelMappings.ContainsKey(viewModelKey))
            {
                CurrentWorkspace = _viewModelMappings[viewModelKey];
            }
        }
    }
}
