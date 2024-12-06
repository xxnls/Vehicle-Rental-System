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
    }
}
