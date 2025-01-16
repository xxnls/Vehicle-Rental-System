using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BackOffice.Views.CustomControls
{
    /// <summary>
    /// Interaction logic for CRUDButtonsControl.xaml
    /// </summary>
    public partial class CRUDButtonsControl : UserControl
    {
        public CRUDButtonsControl()
        {
            InitializeComponent();
        }

        // ReloadCommand Dependency Property
        public ICommand LoadCommand
        {
            get => (ICommand)GetValue(LoadCommandProperty);
            set => SetValue(LoadCommandProperty, value);
        }

        public static readonly DependencyProperty LoadCommandProperty =
            DependencyProperty.Register(nameof(LoadCommand), typeof(ICommand), typeof(CRUDButtonsControl));

        // AddCommand Dependency Property
        public ICommand AddCommand
        {
            get => (ICommand)GetValue(AddCommandProperty);
            set => SetValue(AddCommandProperty, value);
        }

        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register(nameof(AddCommand), typeof(ICommand), typeof(CRUDButtonsControl));

        // EditCommand Dependency Property
        public ICommand EditCommand
        {
            get => (ICommand)GetValue(EditCommandProperty);
            set => SetValue(EditCommandProperty, value);
        }

        public static readonly DependencyProperty EditCommandProperty =
            DependencyProperty.Register(nameof(EditCommand), typeof(ICommand), typeof(CRUDButtonsControl));

        // DeleteCommand Dependency Property
        public ICommand DeleteCommand
        {
            get => (ICommand)GetValue(DeleteCommandProperty);
            set => SetValue(DeleteCommandProperty, value);
        }

        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(CRUDButtonsControl));

        // DeleteVisibility Dependency Property
        public bool DeleteVisibility
        {
            get => (bool)GetValue(DeleteVisibilityProperty);
            set => SetValue(DeleteVisibilityProperty, value);
        }

        public static readonly DependencyProperty DeleteVisibilityProperty =
            DependencyProperty.Register(nameof(DeleteVisibility), typeof(bool), typeof(CRUDButtonsControl));

        // RestoreCommand Dependency Property
        public ICommand RestoreCommand
        {
            get => (ICommand)GetValue(RestoreCommandProperty);
            set => SetValue(RestoreCommandProperty, value);
        }

        public static readonly DependencyProperty RestoreCommandProperty =
            DependencyProperty.Register(nameof(RestoreCommand), typeof(ICommand), typeof(CRUDButtonsControl));

        // RestoreCommandParameter Dependency Property
        public int RestoreCommandParameter
        {
            get => (int)GetValue(RestoreCommandParameterProperty);
            set => SetValue(RestoreCommandParameterProperty, value);
        }

        public static readonly DependencyProperty RestoreCommandParameterProperty =
            DependencyProperty.Register(nameof(RestoreCommandParameter), typeof(int), typeof(CRUDButtonsControl));

        // RestoreVisibility Dependency Property
        public bool RestoreVisibility
        {
            get => (bool)GetValue(RestoreVisibilityProperty);
            set => SetValue(RestoreVisibilityProperty, value);
        }

        public static readonly DependencyProperty RestoreVisibilityProperty =
            DependencyProperty.Register(nameof(RestoreVisibility), typeof(bool), typeof(CRUDButtonsControl));
    }
}
