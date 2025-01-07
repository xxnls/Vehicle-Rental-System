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
    /// Interaction logic for FilterOptionsControl.xaml
    /// </summary>
    public partial class FilterOptionsControl : UserControl
    {
        public FilterOptionsControl()
        {
            InitializeComponent();
        }

        public DateTime? CreatedBefore
        {
            get => (DateTime?)GetValue(CreatedBeforeProperty);
            set => SetValue(CreatedBeforeProperty, value);
        }

        public DateTime? CreatedAfter
        {
            get => (DateTime?)GetValue(CreatedAfterProperty);
            set => SetValue(CreatedAfterProperty, value);
        }
        public DateTime? ModifiedBefore
        {
            get => (DateTime?)GetValue(ModifiedBeforeProperty);
            set => SetValue(ModifiedBeforeProperty, value);
        }

        public DateTime? ModifiedAfter
        {
            get => (DateTime?)GetValue(ModifiedAfterProperty);
            set => SetValue(ModifiedAfterProperty, value);
        }

        public ICommand FilterCommand
        {
            get => (ICommand)GetValue(FilterCommandProperty);
            set => SetValue(FilterCommandProperty, value);
        }

        public static readonly DependencyProperty CreatedBeforeProperty =
            DependencyProperty.Register(nameof(CreatedBefore), typeof(DateTime?), typeof(FilterOptionsControl), new PropertyMetadata(null));

        public static readonly DependencyProperty CreatedAfterProperty =
            DependencyProperty.Register(nameof(CreatedAfter), typeof(DateTime?), typeof(FilterOptionsControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ModifiedBeforeProperty =
            DependencyProperty.Register(nameof(ModifiedBefore), typeof(DateTime?), typeof(FilterOptionsControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ModifiedAfterProperty =
            DependencyProperty.Register(nameof(ModifiedAfter), typeof(DateTime?), typeof(FilterOptionsControl), new PropertyMetadata(null));

        public static readonly DependencyProperty FilterCommandProperty =
            DependencyProperty.Register(nameof(FilterCommand), typeof(ICommand), typeof(FilterOptionsControl), new PropertyMetadata(null));

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            CreatedBefore = null;
            CreatedAfter = null;
            ModifiedBefore = null;
            ModifiedAfter = null;
        }
    }
}
