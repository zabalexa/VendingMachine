using System.Windows.Controls;
using VendingMachineViewModel;

namespace VendingMachine.WPF
{
    /// <summary>
    /// Логика взаимодействия для FillingCupProgressBarControl.xaml
    /// </summary>
    public partial class FillingCupProgressBarControl : UserControl
    {
        public FillingCupProgressBarControl()
        {
            InitializeComponent();
            Queue.Header = ResourceLoadHelper.GetLocalString("Queue");
            DataContext = new QueueViewModel().BindToMainModel();
        }
    }
}
