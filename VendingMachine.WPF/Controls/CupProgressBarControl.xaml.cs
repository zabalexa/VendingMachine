using System.Windows.Controls;
using System.Windows.Media;

namespace VendingMachine.WPF
{
    /// <summary>
    /// Interaction logic for CupProgressBarControl.xaml
    /// </summary>
    public partial class CupProgressBarControl : UserControl
    {
        public void FillEmptyCupBegin()
        {
            FillCup.FillEmptyCupBegin();
        }

        public void Reset()
        {
            FillCup.Reset();
        }

        public void Reset(Color c)
        {
            FillCup.Foreground = new SolidColorBrush(c);
        }

        public CupProgressBarControl()
        {
            InitializeComponent();
        }
    }
}
