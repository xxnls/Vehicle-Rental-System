using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BackOffice.Helpers
{
    public static class ValidationHelper
    {
        public static string GetPropertyName(DependencyObject obj)
        {
            return (string)obj.GetValue(PropertyNameProperty);
        }

        public static void SetPropertyName(DependencyObject obj, string value)
        {
            obj.SetValue(PropertyNameProperty, value);
        }

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.RegisterAttached(
                "PropertyName",
                typeof(string),
                typeof(ValidationHelper),
                new PropertyMetadata(null, PropertyNameChanged));

        private static void PropertyNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                element.Loaded += (sender, args) =>
                {
                    if (element.DataContext is INotifyDataErrorInfo errorInfo)
                    {
                        errorInfo.ErrorsChanged += (s, eventArgs) =>
                        {
                            if (eventArgs.PropertyName == e.NewValue.ToString())
                            {
                                UpdateErrorText(element, errorInfo);
                            }
                        };
                    }
                };
            }
        }

        private static void UpdateErrorText(FrameworkElement element, INotifyDataErrorInfo errorInfo)
        {
            var propertyName = GetPropertyName(element);
            var errors = errorInfo.GetErrors(propertyName).Cast<string>();
            SetErrorText(element, string.Join(Environment.NewLine, errors));
        }

        public static string GetErrorText(DependencyObject obj)
        {
            return (string)obj.GetValue(ErrorTextProperty);
        }

        public static void SetErrorText(DependencyObject obj, string value)
        {
            obj.SetValue(ErrorTextProperty, value);
        }

        public static readonly DependencyProperty ErrorTextProperty =
            DependencyProperty.RegisterAttached(
                "ErrorText",
                typeof(string),
                typeof(ValidationHelper),
                new PropertyMetadata(string.Empty));
    }
}
