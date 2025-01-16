using BackOffice.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using BackOffice.Helpers;

namespace BackOffice.Views
{
    /// <summary>
    /// Interaction logic for DetailedInfoWindow.xaml
    /// </summary>
    public partial class DetailedInfoWindow : Window
    {
        public class ColumnValuePair
        {
            public string Header { get; set; }
            public string Value { get; set; }
            // public bool IsVisible { get; set; }
        }

        public ObservableCollection<ColumnValuePair> ColumnValuePairs { get; set; }

        /// <summary>
        /// Initializes a new instance of the DetailedInfoWindow class.
        /// </summary>
        /// <param name="dataGrid">
        /// The data grid to get the selected item from.
        /// </param>
        public DetailedInfoWindow(DataGrid dataGrid)
        {
            InitializeComponent();
            DataContext = this;
            ColumnValuePairs = new ObservableCollection<ColumnValuePair>();

            var selectedItem = dataGrid.SelectedItem;
            if (selectedItem != null)
            {
                foreach (var column in dataGrid.Columns.OfType<DataGridColumn>())
                {
                    if (column is DataGridTextColumn textColumn)
                    {
                        var binding = (textColumn.Binding as Binding)?.Path.Path;
                        if (binding != null)
                        {
                            var value = selectedItem.GetType().GetProperty(binding)?.GetValue(selectedItem)?.ToString() ?? LocalizationHelper.GetString("Generic", "NoData");
                            var header = (textColumn.Header as string) ??
                                       (textColumn.Header as System.Windows.Controls.ContentControl)?.Content?.ToString() ??
                                       binding;

                            // var isVisible = true;

                            ColumnValuePairs.Add(new ColumnValuePair
                            {
                                Header = header,
                                Value = value,
                                // IsVisible = isVisible
                            });
                        }
                    }
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
