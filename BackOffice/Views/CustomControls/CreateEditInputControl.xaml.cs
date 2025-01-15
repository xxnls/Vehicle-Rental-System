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
    /// Interaction logic for CreateEditInputControl.xaml
    /// </summary>
    public partial class CreateEditInputControl : UserControl
    {
        public CreateEditInputControl()
        {
            InitializeComponent();
        }

        // Label Text Property
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(nameof(LabelText), typeof(string), typeof(CreateEditInputControl));

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        // Text Property for the TextBox input
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(CreateEditInputControl));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        // Validation Property Name for ValidationHelper
        public static readonly DependencyProperty ValidationPropertyNameProperty =
            DependencyProperty.Register(nameof(ValidationPropertyName), typeof(string), typeof(CreateEditInputControl));

        public string ValidationPropertyName
        {
            get => (string)GetValue(ValidationPropertyNameProperty);
            set => SetValue(ValidationPropertyNameProperty, value);
        }
    }
}
