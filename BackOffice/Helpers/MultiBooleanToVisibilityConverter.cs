using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BackOffice.Converters
{
    public class MultiBooleanToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is bool sidebarCollapsed && values[1] is bool isUserAdmin)
            {
                // If the sidebar is collapsed, hide the expander
                if (!sidebarCollapsed)
                    return Visibility.Collapsed;

                // If the user is not an admin, hide the expander
                if (!isUserAdmin)
                    return Visibility.Collapsed;

                // Otherwise, show the expander
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}