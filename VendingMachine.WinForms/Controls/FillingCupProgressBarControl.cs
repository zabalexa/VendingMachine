using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces;
using VendingMachinePresenter.ViewInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachine.WinForms
{
    public partial class FillingCupProgressBarControl : UserControl, IFillingCupProgressBarControlView
    {
        private FlashingCup _flashingCup = new FlashingCup();
        private Color _fillColor;
        public event EventHandler Shown;
        public event VoidEventHandler<Guid> ProductGetOut;
        public event VoidEventHandler Dequeue;

        public FillingCupProgressBarControl()
        {
            InitializeComponent();
            groupBox1.Text = ResourceLoadHelper.GetLocalString("Queue");
            fillingCupIndicator.Value = 0;
            _fillColor = fillingCupIndicator.ForeColor;
            if (fillingCupIndicator.SkinImage == null)
            {
                if ((fillingCupIndicator.SkinImage = ResourceLoadHelper.GetImage(ResourceLoadHelper.GetImageName(DrinkType.None))) == null)
                {
                    fillingCupIndicator.SkinExceptRectangleForProgressBar = new Rectangle(15, 36, 52, 105);
                }
                else
                {
                    fillingCupIndicator.SkinExceptRectangleForProgressBar = fillingCupIndicator.DisplayRectangle;
                }
            }
            queueImages.Images.Add(ResourceLoadHelper.GetBlankImage(new Size(66, 66), Color.Empty));
            queueImages.Images.Add(ResourceLoadHelper.GetImage(ResourceLoadHelper.GetImageName(DrinkType.Tea)) ?? ResourceLoadHelper.GetBlankImage(new Size(66, 66), Color.Empty));
            queueImages.Images.Add(ResourceLoadHelper.GetImage(ResourceLoadHelper.GetImageName(DrinkType.Coffee)) ?? ResourceLoadHelper.GetBlankImage(new Size(66, 66), Color.Empty));
            queueImages.Images.Add(ResourceLoadHelper.GetImage(ResourceLoadHelper.GetImageName(DrinkType.Juice)) ?? ResourceLoadHelper.GetBlankImage(new Size(66, 66), Color.Empty));
            _flashingCup.NextOperation += NextOperation;
            _flashingCup.FillCupComplete += FillCupComplete;
            _flashingCup.HideCup += HideCup;
            _flashingCup.ShowCup += ShowCup;
            _flashingCup.FillCupProgress += FillCupProgress;
        }

        public void Activate(EventArgs e)
        {
            if (Shown != null)
            {
                Shown(this, e);
            }
        }

        public void Purchase(Guid id)
        {
            if (ProductGetOut != null)
            {
                ProductGetOut(id);
            }
        }

        public void RefreshQueue(Queue<Product> queue)
        {
            if (_flashingCup.OperationKind == OperationType.None)
            {
                fillingCupIndicator.Value = 0;
            }
            if (_flashingCup.OperationKind != OperationType.Get)
            {
                _flashingCup.Reset();
            }
            if (queue.Count > 0)
            {
                _fillColor = queue.Peek().parsedColor;
                List<ListViewItem> items = new List<ListViewItem>();
                foreach(Product product in queue)
                {
                    items.Add(new ListViewItem(string.Empty, (int)product.type) { BackColor = product.parsedColor });
                }
                taskQueue.Items.Clear();
                taskQueue.Items.AddRange(items.ToArray());
                if (_flashingCup.OperationKind == OperationType.None)
                {
                    _flashingCup.FlashCup(new TimeSpan(0, 0, 4), false);
                }
            }
            else
            {
                taskQueue.Items.Clear();
            }
        }

        private void FillCupProgress(int param)
        {
            fillingCupIndicator.Value = param;
        }

        private void NextOperation()
        {
            fillingCupIndicator_Click(this, null);
        }

        private void FillCupComplete()
        {
            if (Dequeue != null)
            {
                Dequeue();
            }
        }

        private void HideCup()
        {
            fillingCupIndicator.Hide();
        }

        private void ShowCup()
        {
            fillingCupIndicator.Show();
        }

        private void fillingCupIndicator_Click(object sender, EventArgs e)
        {
            switch (_flashingCup.OperationKind)
            {
                case OperationType.Put:
                    fillingCupIndicator.ForeColor = _fillColor;
                    _flashingCup.FillCup();
                    break;
                case OperationType.Fill:
                    _flashingCup.FlashCup(new TimeSpan(0, 0, 4), fillingCupIndicator.Visible);
                    break;
                case OperationType.Get:
                    _flashingCup.FlashCup(new TimeSpan(0, 0, 0), true);
                    HideCup();
                    break;
            }
        }

        private void taskQueue_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            Rectangle rect = e.Bounds;
            e.Graphics.FillRectangle(new SolidBrush(e.Item.BackColor), rect);
            e.Graphics.DrawImage(ResourceLoadHelper.GetImage(ResourceLoadHelper.GetImageName((DrinkType)e.Item.ImageIndex)), rect);
            if (e.Item.Index == 0)
            {
                Pen pen = new Pen(taskQueue.ForeColor, 2);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                rect.Inflate(-1, -1);
                e.Graphics.DrawRectangle(pen, rect);
            }
        }
    }

    public enum OperationType
    {
        None,
        Put,
        Fill,
        Get
    }

    public class FlashingCup : IDisposable
    {
        private BackgroundWorker _flashWorker = new BackgroundWorker();
        private AutoResetEvent _timer = new AutoResetEvent(false);
        private bool _breakSignal = false;
        public event VoidEventHandler NextOperation;
        public event VoidEventHandler FillCupComplete;
        public event VoidEventHandler HideCup;
        public event VoidEventHandler ShowCup;
        public event VoidEventHandler<int> FillCupProgress;

        private OperationType _operationType = OperationType.None;
        public OperationType OperationKind
        {
            get
            {
                lock(this)
                {
                    return _operationType;
                }
            }
        }

        public FlashingCup()
        {
            _flashWorker.WorkerReportsProgress = true;
            _flashWorker.ProgressChanged += _flashWorker_ProgressChanged;
            _flashWorker.RunWorkerCompleted += _flashWorker_RunWorkerCompleted;
            _flashWorker.DoWork += _flashWorker_DoWork;
        }

        private void ReInit()
        {
            _flashWorker.DoWork -= _flashWorker_DoWork;
            _flashWorker.RunWorkerCompleted -= _flashWorker_RunWorkerCompleted;
            _flashWorker.ProgressChanged -= _flashWorker_ProgressChanged;
            _flashWorker.Dispose();
            lock(this)
            {
                _breakSignal = false;
            }
            _flashWorker = new BackgroundWorker();
            _flashWorker.WorkerReportsProgress = true;
            _flashWorker.ProgressChanged += _flashWorker_ProgressChanged;
            _flashWorker.RunWorkerCompleted += _flashWorker_RunWorkerCompleted;
            _flashWorker.DoWork += _flashWorker_DoWork;
        }

        private void _flashWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (_operationType == OperationType.Fill)
            {
                _operationType = OperationType.Get;
            }
            switch (e.ProgressPercentage)
            {
                case -1:
                    if (_operationType != OperationType.Get)
                    {
                        _operationType = OperationType.Put;
                    }
                    if (!_breakSignal && (ShowCup != null))
                    {
                        ShowCup();
                    }
                    break;
                case -2:
                    if (_operationType != OperationType.Get)
                    {
                        _operationType = OperationType.Put;
                    }
                    if (!_breakSignal && (HideCup != null))
                    {
                        HideCup();
                    }
                    break;
                default:
                    _operationType = OperationType.Fill;
                    if (!_breakSignal && (FillCupProgress != null) && (e != null))
                    {
                        FillCupProgress(e.ProgressPercentage);
                    }
                    break;
            }
        }

        private void _flashWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument == null)
            {
                if (FillCupProgress != null)
                {
                    for (int percent = 0; percent < 101; percent++)
                    {
                        if (_breakSignal)
                        {
                            e.Result = percent;
                            break;
                        }
                        _flashWorker.ReportProgress(percent);
                        _timer.WaitOne(100);
                    }
                }
                _breakSignal = false;
                return;
            }
            else
            {
                if (!(e.Argument is KeyValuePair<TimeSpan, bool>))
                {
                    return;
                }
            }
            TimeSpan flashingDuration = ((KeyValuePair<TimeSpan, bool>)e.Argument).Key;
            bool showedCup = ((KeyValuePair<TimeSpan, bool>)e.Argument).Value;
            int wait = flashingDuration.Days;
            if (wait > 0)
            {
                flashingDuration = flashingDuration.Add(new TimeSpan(-wait, 0, 0, 0));
                _timer.WaitOne(wait);
            }
            bool subscribedEvent = (HideCup != null) && (ShowCup != null);
            if (subscribedEvent)
            {
                if (showedCup)
                {
                    _flashWorker.ReportProgress(-2);
                }
                else
                {
                    _flashWorker.ReportProgress(-1);
                }
            }
            while (flashingDuration.TotalMilliseconds > 0)
            {
                if (_breakSignal)
                {
                    e.Result = flashingDuration;
                    break;
                }
                _timer.WaitOne(showedCup ? 300 : 700);
                if (subscribedEvent)
                {
                    if (_breakSignal)
                    {
                        e.Result = flashingDuration;
                        break;
                    }
                    if (showedCup)
                    {
                        _flashWorker.ReportProgress(-1);
                    }
                    else
                    {
                        _flashWorker.ReportProgress(-2);
                    }
                }
                if (_breakSignal)
                {
                    e.Result = flashingDuration;
                    break;
                }
                _timer.WaitOne(showedCup ? 700 : 300);
                if (subscribedEvent)
                {
                    if (_breakSignal)
                    {
                        e.Result = flashingDuration;
                        break;
                    }
                    if (showedCup)
                    {
                        _flashWorker.ReportProgress(-2);
                    }
                    else
                    {
                        _flashWorker.ReportProgress(-1);
                    }
                }
                flashingDuration = flashingDuration.Add(new TimeSpan(0, 0, 0, -1));
            }
            _breakSignal = false;
        }

        private void _flashWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Reset(false);
        }

        public void FlashCup(TimeSpan flashingDuration, bool flashingCupShowed)
        {
            if ((flashingCupShowed && (flashingDuration.TotalMilliseconds == 0)) || (_flashWorker.IsBusy))
            {
                Breaking();
            }
            if (flashingDuration.TotalMilliseconds > 0)
            {
                bool waitable = false;
                if (_operationType == OperationType.None)
                {
                    if (HideCup != null)
                    {
                        HideCup();
                    }
                    waitable = true;
                }
                _flashWorker.RunWorkerAsync(new KeyValuePair<TimeSpan, bool>(waitable ? flashingDuration.Add(new TimeSpan(1000, 0, 0, 0)) : flashingDuration, flashingCupShowed));
            }
        }

        public void FillCup()
        {
            FlashCup(new TimeSpan(0, 0, 0), false);
            _flashWorker.RunWorkerAsync();
        }

        private void Breaking()
        {
            if (_flashWorker.IsBusy)
            {
                lock (this)
                {
                    _breakSignal = true;
                }
                _timer.Set();
                while (_breakSignal)
                {
                    _timer.WaitOne(100);
                }
            }
            ReInit();
            Reset();
        }

        public void Reset(bool manual = true)
        {
            if ((FillCupComplete != null) && (_operationType == OperationType.Get))
            {
                _operationType = OperationType.None;
                FillCupComplete();
            }
            else
            {
                if (!manual && (NextOperation != null) && (_operationType != OperationType.None))
                {
                    NextOperation();
                }
            }
        }

        public void Dispose()
        {
            _flashWorker.DoWork += _flashWorker_DoWork;
            _flashWorker.RunWorkerCompleted += _flashWorker_RunWorkerCompleted;
            _flashWorker.ProgressChanged += _flashWorker_ProgressChanged;
            _flashWorker.Dispose();
            _timer.Dispose();
        }
    }

    public class FillingCup : ProgressBar
    {
        public FillingCup()
        {
            Style = ProgressBarStyle.Continuous;
            Minimum = 0;
            Maximum = 100;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        public Image SkinImage { get; set; }

        public Rectangle SkinExceptRectangleForProgressBar { get; set; }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams param = base.CreateParams;
                param.Style |= 0x04;
                return param;
            }
        }

        protected override void OnPaint(PaintEventArgs eventArgs)
        {
            Rectangle rect = SkinExceptRectangleForProgressBar;
            rect.Inflate(-1, -1);
            rect.Y += rect.Height;
            rect.Height = Convert.ToInt32(Math.Round((1.0M * rect.Height * Value) / (Maximum - Minimum)));
            rect.Y -= rect.Height;
            if (ProgressBarRenderer.IsSupported && (SkinImage == null))
            {
                ProgressBarRenderer.DrawVerticalChunks(eventArgs.Graphics, rect);
            }
            else
            {
                eventArgs.Graphics.FillRectangle(new SolidBrush(ForeColor), rect);
                eventArgs.Graphics.DrawImage(SkinImage, eventArgs.ClipRectangle);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs eventArgs)
        {
            Rectangle rect = SkinExceptRectangleForProgressBar;
            if (ProgressBarRenderer.IsSupported && (SkinImage == null))
            {
                ProgressBarRenderer.DrawVerticalBar(eventArgs.Graphics, rect);
            }
            else
            {
                if (SkinImage == null)
                {
                    eventArgs.Graphics.FillRectangle(new SolidBrush(Parent.BackColor), rect);
                    eventArgs.Graphics.DrawRectangle(new Pen(Color.Black, 2), rect);
                }
                else
                {
                    eventArgs.Graphics.Clear(Parent.BackColor);
                }
            }
        }
    }
}
