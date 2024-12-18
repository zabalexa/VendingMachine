using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces;
using VendingMachinePresenter.ViewInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachine.WinForms
{
    public partial class CashButtonControl : UserControl, ICashButtonControlView
    {
        private AccountType _controlType;
        private bool _notClick = false;
        private bool _hitTestPass = false;
        private bool _drawn = false;
        private Dictionary<CoinType, int> _drawnCoins;
        public event EventHandler Shown;
        public event VoidEventHandler<CoinType> PutCoin;
        public event VoidEventHandler RefreshVendingMachine;
        public event VoidEventHandler Change;
        public event RequestEventHandler<Dictionary<CoinType, int>> ChangeSimulate;

        public CashButtonControl()
        {
            InitializeComponent();
            groupBox1.Text = ResourceLoadHelper.GetLocalString("PayGroup");
            label1.Text = ResourceLoadHelper.GetLocalString("PayCurrency");
            changeButton.Text = ResourceLoadHelper.GetLocalString("Change");
        }

        public void Activate(EventArgs e)
        {
            if (Shown == null)
            {
                return;
            }
            Shown(this, e);
        }

        public void SetControlType(AccountType type)
        {
            switch(_controlType = type)
            {
                case AccountType.Customer:
                    coinsInfo1.Visible = true;
                    coinsInfo2.Visible = true;
                    coinsInfo5.Visible = true;
                    coinsInfo10.Visible = true;
                    coins1.SetControlType(_controlType);
                    coins1.Visible = true;
                    coins2.SetControlType(_controlType);
                    coins2.Visible = true;
                    coins5.SetControlType(_controlType);
                    coins5.Visible = true;
                    coins10.SetControlType(_controlType);
                    coins10.Visible = true;
                    groupBox1.Visible = true;
                    break;
                case AccountType.VendingMachine:
                    coinsInfoVM1.Visible = true;
                    coinsInfoVM2.Visible = true;
                    coinsInfoVM5.Visible = true;
                    coinsInfoVM10.Visible = true;
                    coinsVM1.SetControlType(_controlType);
                    coinsVM1.Visible = true;
                    coinsVM2.SetControlType(_controlType);
                    coinsVM2.Visible = true;
                    coinsVM5.SetControlType(_controlType);
                    coinsVM5.Visible = true;
                    coinsVM10.SetControlType(_controlType);
                    coinsVM10.Visible = true;
                    changeButton.Visible = true;
                    descriptionToolTips.Active = true;
                    int y = 0;
                    foreach (CoinType i in new List<CoinType> { CoinType.coin1, CoinType.coin2, CoinType.coin5, CoinType.coin10 })
                    {
                        CoinsBar cb = new CoinsBar() { Left = 8, Top = y++ * 36 + 8, Width = 100, Height = 30, Minimum = 0, Maximum = 10, BackColor = Color.Gold, ForeColor = Color.Chocolate, CoinValue = (int)i, Value = 0, Visible = false };
                        cb.SetControlType(AccountType.Customer);
                        descriptionToolTips.Container.Add(cb);
                    }
                    descriptionToolTips.SetToolTip(changeButton, " ");
                    break;
            }
        }

        public void RefreshBalance(Dictionary<CoinType, Coin> coins, int paid)
        {
            switch (_controlType)
            {
                case AccountType.Customer:
                    payBalance.Text = paid.ToString();
                    if (coins != null)
                    {
                        coinsInfo1.Text = coins[CoinType.coin1].quantity.ToString();
                        coinsInfo2.Text = coins[CoinType.coin2].quantity.ToString();
                        coinsInfo5.Text = coins[CoinType.coin5].quantity.ToString();
                        coinsInfo10.Text = coins[CoinType.coin10].quantity.ToString();
                        coins1.Value = coins[CoinType.coin1].quantity > coins1.Maximum ? coins1.Maximum : coins[CoinType.coin1].quantity;
                        coins2.Value = coins[CoinType.coin2].quantity > coins2.Maximum ? coins2.Maximum : coins[CoinType.coin2].quantity;
                        coins5.Value = coins[CoinType.coin5].quantity > coins5.Maximum ? coins5.Maximum : coins[CoinType.coin5].quantity;
                        coins10.Value = coins[CoinType.coin10].quantity > coins10.Maximum ? coins10.Maximum : coins[CoinType.coin10].quantity;
                    }
                    break;
                case AccountType.VendingMachine:
                    if (coins != null)
                    {
                        coinsInfoVM1.Text = coins[CoinType.coin1].quantity.ToString();
                        coinsInfoVM2.Text = coins[CoinType.coin2].quantity.ToString();
                        coinsInfoVM5.Text = coins[CoinType.coin5].quantity.ToString();
                        coinsInfoVM10.Text = coins[CoinType.coin10].quantity.ToString();
                        coinsVM1.Value = coins[CoinType.coin1].quantity > coinsVM1.Maximum ? coinsVM1.Maximum : coins[CoinType.coin1].quantity;
                        coinsVM2.Value = coins[CoinType.coin2].quantity > coinsVM2.Maximum ? coinsVM2.Maximum : coins[CoinType.coin2].quantity;
                        coinsVM5.Value = coins[CoinType.coin5].quantity > coinsVM5.Maximum ? coinsVM5.Maximum : coins[CoinType.coin5].quantity;
                        coinsVM10.Value = coins[CoinType.coin10].quantity > coinsVM10.Maximum ? coinsVM10.Maximum : coins[CoinType.coin10].quantity;
                    }
                    break;
            }
        }

        public void RefreshVendingMachineBalance()
        {
            if (RefreshVendingMachine != null)
            {
                RefreshVendingMachine();
            }
        }

        private void coins_Click(object sender, EventArgs e)
        {
            if ((PutCoin != null) && _hitTestPass)
            {
                PutCoin((CoinType)(sender as CoinsBar).CoinValue);
            }
        }

        private void noFocus(object sender, EventArgs e)
        {
            if (_controlType == AccountType.Customer)
            {
                groupBox1.Focus();
            }
            else
            {
                changeButton.Focus();
            }
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            if (Change != null)
            {
                Change();
            }
        }

        private void changeButton_MouseEnter(object sender, EventArgs e)
        {
            int y = 0;
            if (ChangeSimulate != null)
            {
                _drawnCoins = ChangeSimulate();
                foreach (Component c in descriptionToolTips.Container.Components)
                {
                    if (c is CoinsBar)
                    {
                        CoinsBar cb = c as CoinsBar;
                        CoinType i = (CoinType)cb.CoinValue;
                        if (cb.Visible = _drawnCoins.Keys.Contains(i))
                        {
                            cb.Top = y++ * 36 + 8;
                            cb.Value = _drawnCoins[i] > cb.Maximum ? cb.Maximum : _drawnCoins[i];
                        }
                    }
                }
            }
            if (_drawn = y > 0)
            {
                if (y++ > 2)
                {
                    y *= 2;
                }
                else
                {
                    y += --y;
                }
                descriptionToolTips.RemoveAll();
                descriptionToolTips.SetToolTip(changeButton, string.Join("\n", (string[])Array.CreateInstance(typeof(string), y)) + "                                        ");
            }
        }

        private void descriptionToolTips_Popup(object sender, PopupEventArgs e)
        {
            e.Cancel = !_drawn;
        }

        private void descriptionToolTips_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            Rectangle rect = e.Bounds;
            Bitmap bmp = new Bitmap(ResourceLoadHelper.GetBlankImage(new Size(rect.Width, rect.Height), descriptionToolTips.BackColor));
            foreach (Component c in descriptionToolTips.Container.Components)
            {
                if (c is CoinsBar)
                {
                    CoinsBar cb = c as CoinsBar;
                    if (cb.Visible)
                    {
                        rect = cb.Bounds;
                        cb.DrawToBitmap(bmp, rect);
                        e.Graphics.DrawImage(bmp, rect, rect, GraphicsUnit.Pixel);
                        e.Graphics.DrawString(string.Format("x{0}", _drawnCoins[(CoinType)cb.CoinValue]), new Font("Microsoft Sans Serif", 7, FontStyle.Italic), new SolidBrush(Color.Black), rect.Right, rect.Bottom - 14);
                    }
                }
            }
            e.DrawBorder();
        }

        #region Drag-And-Drop

        private void coins_MouseDown(object sender, MouseEventArgs e)
        {
            CoinsBar s = sender as CoinsBar;
            if (_hitTestPass = (e.Location.X - 15 <= s.Value * (s.Width - 14) / (s.Maximum - s.Minimum)))
            {
                _notClick = true;
            }
        }

        private void coins_MouseUp(object sender, MouseEventArgs e)
        {
            _notClick = false;
        }

        private void coins_MouseMove(object sender, MouseEventArgs e)
        {
            if (_notClick)
            {
                _notClick = false;
                CoinsBar s = sender as CoinsBar;
                s.DoDragDrop(s.CoinValue, DragDropEffects.Move | DragDropEffects.Link);
            }
        }

        private void coins_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            int sz = 30;
            switch (e.Effect)
            {
                case DragDropEffects.Move:
                    e.UseDefaultCursors = false;
                    Cursor.Current = ResourceLoadHelper.GetCoinCursor(new Size(sz, sz), ((CoinsBar)sender).BackColor, ((CoinsBar)sender).ForeColor, ((CoinsBar)sender).Parent.Font, ((CoinsBar)sender).CoinValue);
                    break;
                case DragDropEffects.Link:
                    e.UseDefaultCursors = false;
                    Cursor.Current = ResourceLoadHelper.GetPutCoinCursor(new Size(10, sz), ((CoinsBar)sender).BackColor, ((CoinsBar)sender).ForeColor);
                    break;
            }
        }

        private void CashButtonControl_DragEnter(object sender, DragEventArgs e)
        {
            if (_controlType == AccountType.Customer)
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void putCoins_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
            putCoins.BackColor = Color.DarkOliveGreen;
        }

        private void putCoins_DragLeave(object sender, EventArgs e)
        {
            putCoins.BackColor = putCoins.Parent.BackColor;
        }

        private void putCoins_DragDrop(object sender, DragEventArgs e)
        {
            putCoins.BackColor = putCoins.Parent.BackColor;
            if ((PutCoin != null) && e.Data.GetDataPresent(typeof(int)))
            {
                PutCoin((CoinType)e.Data.GetData(typeof(int)));
            }
        }

        #endregion

    }

    public class CoinsBar : ProgressBar
    {
        private AccountType _controlType;

        public CoinsBar()
        {
            Style = ProgressBarStyle.Continuous;
            Minimum = 0;
            Maximum = 10;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        public int CoinValue { get; set; }

        public void SetControlType(AccountType type)
        {
            _controlType = type;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams param = base.CreateParams;
                if (_controlType == AccountType.VendingMachine)
                {
                    param.Style |= 0x04;
                }
                return param;
            }
        }

        protected override void OnPaint(PaintEventArgs eventArgs)
        {
            Rectangle rect = eventArgs.ClipRectangle;
            rect.Inflate(-1, -1);
            if (ProgressBarRenderer.IsSupported && (_controlType == AccountType.Unknown))
            {
                rect.Width = Convert.ToInt32(Math.Round((1.0M * rect.Width * Value) / (Maximum - Minimum)));
                ProgressBarRenderer.DrawHorizontalChunks(eventArgs.Graphics, rect);
            }
            else
            {
                Rectangle r = Rectangle.Empty;
                int w = ((_controlType == AccountType.VendingMachine ? rect.Height : rect.Width) - 15) / (Maximum - Minimum);
                for (int i = 0; i < Value; i++)
                {
                    r = _controlType == AccountType.VendingMachine ? new Rectangle(0, i * w, rect.Width, 25) : new Rectangle(i * w, 0, 25, rect.Height);
                    eventArgs.Graphics.FillEllipse(new SolidBrush(BackColor), r);
                    eventArgs.Graphics.DrawEllipse(new Pen(ForeColor), r);
                }
                if (!r.IsEmpty)
                {
                    string s = CoinValue.ToString();
                    if (_controlType == AccountType.VendingMachine)
                    {
                        r.Inflate(s.Length > 1 ? -1 : - 6, -3);
                    }
                    else
                    {
                        r.Inflate(s.Length > 1 ? 0 : -5, -4);
                    }
                    float sz = 12;
                    Font f = Parent == null ? new Font("Microsoft Sans Serif", sz, FontStyle.Bold) : new Font(Parent.Font.FontFamily, sz, Parent.Font.Style | FontStyle.Bold);
                    while (s.Length > 1 && eventArgs.Graphics.MeasureString(s, f, r.Size).ToSize().Width > r.Width)
                    {
                        sz = f.Size - 1;
                        f = Parent == null ? new Font("Microsoft Sans Serif", sz, FontStyle.Bold) : new Font(Parent.Font.FontFamily, sz, Parent.Font.Style | FontStyle.Bold);
                    }
                    eventArgs.Graphics.DrawString(s, f, new SolidBrush(ForeColor), r);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs eventArgs)
        {
            Rectangle rect = eventArgs.ClipRectangle;
            if (ProgressBarRenderer.IsSupported && (_controlType == AccountType.Unknown))
            {
                ProgressBarRenderer.DrawHorizontalBar(eventArgs.Graphics, rect);
            }
            else
            {
                if (_controlType == AccountType.Unknown)
                {
                    eventArgs.Graphics.FillRectangle(new SolidBrush(Parent.BackColor), rect);
                    eventArgs.Graphics.DrawRectangle(new Pen(Color.Black, 2), rect);
                }
                else
                {
                    eventArgs.Graphics.Clear(Parent == null ? SystemColors.Info : Parent.BackColor);
                }
            }
        }
    }
}
