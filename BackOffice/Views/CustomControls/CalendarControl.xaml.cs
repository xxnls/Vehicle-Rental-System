using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for CalendarControl.xaml
    /// </summary>
    public partial class CalendarControl : UserControl
    {
        private DateTime _currentDate;

        public CalendarControl()
        {
            InitializeComponent();
            _currentDate = DateTime.Now;
            RenderCalendar(_currentDate);
        }

        private void RenderCalendar(DateTime date)
        {
            // Clear the calendar grid (except the first row with day labels)
            CalendarGrid.Children.Clear();
            for (int i = 1; i < CalendarGrid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < CalendarGrid.ColumnDefinitions.Count; j++)
                {
                    var textBlock = new TextBlock
                    {
                        Text = "",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    CalendarGrid.Children.Add(textBlock);
                    Grid.SetRow(textBlock, i);
                    Grid.SetColumn(textBlock, j);
                }
            }

            // Set the month and year
            MonthYearTextBlock.Text = date.ToString("MMMM yyyy");

            // Get the first day of the month
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);

            // Get the number of days in the month
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            // Get the starting day of the week (0 = Monday, 6 = Sunday)
            var startDay = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            // Calculate the starting position in the grid
            var startColumn = (int)firstDayOfMonth.DayOfWeek - startDay;
            if (startColumn < 0) startColumn += 7;

            // Populate the grid with days
            for (var day = 1; day <= daysInMonth; day++)
            {
                var row = (startColumn + day - 1) / 7 + 1;
                var column = (startColumn + day - 1) % 7;

                var button = new Button
                {
                    Content = day.ToString(),
                    Margin = new Thickness(2),
                    Tag = new DateTime(date.Year, date.Month, day) // Store the date in the button's Tag
                };
                button.Click += DayButton_Click;

                CalendarGrid.Children.Add(button);
                Grid.SetRow(button, row);
                Grid.SetColumn(button, column);
            }
        }

        private void DayButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is DateTime date)
            {
                MessageBox.Show($"Selected Date: {date.ToShortDateString()}");
            }
        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddMonths(-1);
            RenderCalendar(_currentDate);
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddMonths(1);
            RenderCalendar(_currentDate);
        }
    }
}
