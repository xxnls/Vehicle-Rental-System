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
    /// Interaction logic for PaginationControl.xaml
    /// </summary>
    public partial class PaginationControl : UserControl
    {
        public PaginationControl()
        {
            InitializeComponent();
        }

        // CurrentPage Dependency Property
        public int CurrentPage
        {
            get => (int)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(int), typeof(PaginationControl), new PropertyMetadata(1));

        // TotalItemCount Dependency Property
        public int TotalItemCount
        {
            get => (int)GetValue(TotalItemCountProperty);
            set => SetValue(TotalItemCountProperty, value);
        }
        public static readonly DependencyProperty TotalItemCountProperty =
            DependencyProperty.Register(nameof(TotalItemCount), typeof(int), typeof(PaginationControl), new PropertyMetadata(0));

        // ModelsPerPage Dependency Property
        public int ModelsPerPage
        {
            get => (int)GetValue(ModelsPerPageProperty);
            set => SetValue(ModelsPerPageProperty, value);
        }

        public static readonly DependencyProperty ModelsPerPageProperty =
            DependencyProperty.Register(nameof(ModelsPerPage), typeof(int), typeof(PaginationControl), new PropertyMetadata(null));

        // CanLoadPreviousPage Dependency Property
        public bool CanLoadPreviousPage
        {
            get => (bool)GetValue(CanLoadPreviousPageProperty);
            set => SetValue(CanLoadPreviousPageProperty, value);
        }
        public static readonly DependencyProperty CanLoadPreviousPageProperty =
            DependencyProperty.Register(nameof(CanLoadPreviousPage), typeof(bool), typeof(PaginationControl), new PropertyMetadata(false));

        // CanLoadNextPage Dependency Property
        public bool CanLoadNextPage
        {
            get => (bool)GetValue(CanLoadNextPageProperty);
            set => SetValue(CanLoadNextPageProperty, value);
        }
        public static readonly DependencyProperty CanLoadNextPageProperty =
            DependencyProperty.Register(nameof(CanLoadNextPage), typeof(bool), typeof(PaginationControl), new PropertyMetadata(false));

        // PreviousPageCommand Dependency Property
        public ICommand PreviousPageCommand
        {
            get => (ICommand)GetValue(PreviousPageCommandProperty);
            set => SetValue(PreviousPageCommandProperty, value);
        }
        public static readonly DependencyProperty PreviousPageCommandProperty =
            DependencyProperty.Register(nameof(PreviousPageCommand), typeof(ICommand), typeof(PaginationControl), new PropertyMetadata(null));

        // NextPageCommand Dependency Property
        public ICommand NextPageCommand
        {
            get => (ICommand)GetValue(NextPageCommandProperty);
            set => SetValue(NextPageCommandProperty, value);
        }
        public static readonly DependencyProperty NextPageCommandProperty =
            DependencyProperty.Register(nameof(NextPageCommand), typeof(ICommand), typeof(PaginationControl), new PropertyMetadata(null));
    }
}
