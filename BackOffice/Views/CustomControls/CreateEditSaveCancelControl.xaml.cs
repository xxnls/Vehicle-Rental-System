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
    /// Interaction logic for CreateEditSaveCancelControl.xaml
    /// </summary>
    public partial class CreateEditSaveCancelControl : UserControl
    {
        public CreateEditSaveCancelControl()
        {
            InitializeComponent();
        }

        public ICommand SaveCommand
        {
            get => (ICommand)GetValue(SaveCommandProperty);
            set => SetValue(SaveCommandProperty, value);
        }

        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.Register(nameof(SaveCommand), typeof(ICommand), typeof(CreateEditSaveCancelControl));


        public ICommand CancelCommand
        {
            get => (ICommand)GetValue(CancelCommandProperty);
            set => SetValue(CancelCommandProperty, value);
        }

        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(CreateEditSaveCancelControl));

        public bool HasErrors
        {
            get => (bool)GetValue(HasErrorsProperty);
            set => SetValue(HasErrorsProperty, value);
        }

        public static readonly DependencyProperty HasErrorsProperty =
            DependencyProperty.Register(nameof(HasErrors), typeof(bool), typeof(CreateEditSaveCancelControl));
    }
}
