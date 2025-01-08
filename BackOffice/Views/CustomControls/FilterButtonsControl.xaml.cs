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
    /// Interaction logic for FilterButtonsControl.xaml
    /// </summary>
    public partial class FilterButtonsControl : UserControl
    {
        public FilterButtonsControl()
        {
            InitializeComponent();
        }

        public ICommand ShowFilterOptionsCommand
        {
            get => (ICommand)GetValue(ShowFilterOptionsCommandProperty);
            set => SetValue(ShowFilterOptionsCommandProperty, value);
        }

        public ICommand ShowDeletedModelsCommand
        {
            get => (ICommand)GetValue(ShowDeletedModelsCommandProperty);
            set => SetValue(ShowDeletedModelsCommandProperty, value);
        }

        public static readonly DependencyProperty ShowFilterOptionsCommandProperty =
            DependencyProperty.Register(nameof(ShowFilterOptionsCommand), typeof(ICommand), typeof(FilterButtonsControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ShowDeletedModelsCommandProperty =
            DependencyProperty.Register(nameof(ShowDeletedModelsCommand), typeof(ICommand), typeof(FilterButtonsControl), new PropertyMetadata(null));
    }
}
