using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;
using VendingMachineViewModel;

namespace VendingMachine.WPF
{
    /// <summary>
    /// Логика взаимодействия для FillingCupProgressBarControl.xaml
    /// </summary>
    public partial class FillingCupProgressBarControl : UserControl, IDisposable
    {
        private readonly Display _scrollableContainerParams;
        private bool _disposed = false;
        private int _animationRef = 0; 

        public FillingCupProgressBarControl()
        {
            _scrollableContainerParams = new Display(204);
            InitializeComponent();
            VisualCoupon.Initialize(false);
            Queue.Header = ResourceLoadHelper.GetLocalString("Queue");
            QueueViewModel model = new QueueViewModel().BindToMainModel();
            model.Queue += Model_Queue;
            model.DisposeRequest += Dispose;
            DataContext = model;
        }

        private ImageSource GetImage()
        {
            ProductEx item = (DataContext as QueueViewModel)?.QItems.LastOrDefault();
            return ResourceLoadHelper.GetDrinksButtonImage(new Size(65, 65), (Color)ColorConverter.ConvertFromString(item?.color ?? "Blue"), item?.type ?? DrinkType.None).Source;
        }

        public ImageSource QImage
        {
            get
            {
                return GetImage();
            }
        }

        public string Coupon
        {
            get
            {
                string local = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                return local == "ru" ? "Купон" : "Coupon";
            }
        }

        private void Model_Queue(Product drink)
        {
            if (_animationRef++ < 1)
            {
                QueueViewModel model = DataContext as QueueViewModel;
                if (model != null)
                {
                    model.QItems.First().IsSelected = true;
                    CupControl.Reset(model.Visual_GetColor());
                    VisualCoupon.Reset(model.QItems.First().IsCurrentCuopon);
                }
                BorderFlash.FlashAndFillCupBegin();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                QueueViewModel model = DataContext as QueueViewModel;
                if (model != null)
                {
                    model.Queue -= Model_Queue;
                    model.DisposeRequest -= Dispose;
                }
            }
        }

        public class Display : MVVM
        {
            private double _width;

            public Display(double width)
            {
                _width = width;
            }

            public double ScrollableWidth
            {
                get { return _width; }
                set
                {
                    if (_width != value)
                    {
                        _width = value;
                        OnPropertyChanged(nameof(ScrollableWidth));
                    }
                }
            }
        }

        public Display ScrollableContainerParams { get { return _scrollableContainerParams; } }

        private void Timeline_OnStarted(object sender, EventArgs e)
        {
            CupControl.FillEmptyCupBegin();
        }

        private void Timeline_OnFinished(object sender, EventArgs e)
        {
            CupControl.Reset();
            QueueViewModel model = DataContext as QueueViewModel;
            if (model != null)
            {
                model.Visual_Dequeue();
            }
            if (--_animationRef > 0)
            {
                if (model != null)
                {
                    model.QItems.First().IsSelected = true;
                    CupControl.Reset(model.Visual_GetColor());
                    VisualCoupon.Reset(model.QItems.First().IsCurrentCuopon);
                }
                BorderFlash.FlashAndFillCupBegin();
            }
        }
    }
}
