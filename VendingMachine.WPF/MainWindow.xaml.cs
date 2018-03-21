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
using System.Windows.Threading;
using VendingMachineDataInjectionInterfaces;

namespace VendingMachine.WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer = new DispatcherTimer();
        VendingMachineViewModel.VendingMachineViewModel _model;

        public MainWindow()
        {
            InitializeComponent();
            _model = VendingMachineViewModel.VendingMachineViewModel.Current;
            _timer.Interval = new TimeSpan(0, 0, 3);
            _timer.Tick += _timer_Tick;
            _model.MessageToCustomer.PropertyChanged += MessageToCustomer_PropertyChanged;
            DataContext = _model;
            Customer.SetControlType(AccountType.Customer);
            VendingMachine.SetControlType(AccountType.VendingMachine);
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            _model.MessageToCustomer.Text = "";
        }

        private void MessageToCustomer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_model.MessageToCustomer.Text))
            {
                string text = ResourceLoadHelper.GetLocalString(_model.MessageToCustomer.Text);
                if (!string.IsNullOrEmpty(text))
                {
                    if (_timer.IsEnabled)
                    {
                        _timer.Stop();
                    }
                    _model.MessageToCustomer.Text = text;
                    _timer.Start();
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _model.MessageToCustomer.PropertyChanged -= MessageToCustomer_PropertyChanged;
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
            _model.Dispose();
        }
    }
}
