using System.Windows.Forms;
using VendingMachinePresenter.ViewInterfaces;

namespace VendingMachine.WinForms
{
    public partial class VendingMachineForm : Form, IVendingMachineView
    {
        public VendingMachineForm()
        {
            InitializeComponent();
        }

        public ICashButtonControlView CustomerCashButtonControlView
        {
            get { return customerCashButtonControl; }
        }

        public ICashButtonControlView VendingMachineChangeCashButtonControlView
        {
            get { return vendingMachineChangeCashButtonControl; }
        }

        public IGoodsButtonControlView GoodsButtonControlView
        {
            get { return goodsButtonControl; }
        }

        public IFillingCupProgressBarControlView FillingCupProgressBarControlView
        {
            get { return fillingCupProgressBarControl; }
        }

        public void ThanksMessage()
        {
            timer1.Stop();
            display.Text = ResourceLoadHelper.GetLocalString("ThanksMessage");
            timer1.Start();
        }

        public void InsufficientMessage()
        {
            timer1.Stop();
            display.Text = ResourceLoadHelper.GetLocalString("InsufficientMessage");
            timer1.Start();
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            timer1.Stop();
            display.Text = "";
        }
    }
}
