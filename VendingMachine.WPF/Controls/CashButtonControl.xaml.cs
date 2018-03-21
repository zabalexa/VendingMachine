using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using VendingMachineDataInjectionInterfaces;
using VendingMachineViewModel;

namespace VendingMachine.WPF
{
    /// <summary>
    /// Логика взаимодействия для CashButtonControl.xaml
    /// </summary>
    public partial class CashButtonControl : UserControl, IDisposable
    {
        private AccountType _controlType;
        private Dictionary<CoinType, int> _drawnCoins;

        public CashButtonControl()
        {
            InitializeComponent();
            DropCoins.Header = ResourceLoadHelper.GetLocalString("PayGroup");
            DropCurrency.Content = ResourceLoadHelper.GetLocalString("PayCurrency");
            ChangeButton.Content = ResourceLoadHelper.GetLocalString("Change");
        }

        public void SetControlType(AccountType type)
        {
            CoinsViewModel model = new CoinsViewModel(_controlType = type).BindToMainModel();
            model.InitTemplate(() => {
                if (model.PayBalance > 0)
                {
                    int h = 40 * _drawnCoins.Count;
                    StringBuilder template = new StringBuilder();
                    template.AppendFormat("<ToolTip xmlns ='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'><ToolTip.Style><Style TargetType=\"ToolTip\"><Setter Property=\"OverridesDefaultStyle\" Value=\"True\" /><Setter Property=\"Template\"><Setter.Value><ControlTemplate TargetType=\"ToolTip\"><Border BorderBrush=\"Black\" BorderThickness=\"1\"><Grid Height=\"{0}\" Width=\"140\" Background=\"AntiqueWhite\"><Canvas Margin=\"5,5,5,5\">", h);
                    foreach (KeyValuePair<CoinType, int> coin in _drawnCoins)
                    {
                        template.AppendFormat("<Image x:Name=\"ibar{0}\" Stretch=\"None\" Width=\"100\" Height=\"30\" VerticalAlignment=\"Top\" HorizontalAlignment=\"Left\">", (int)coin.Key);
                        template.Append("<Image.Style><Style TargetType=\"{x:Type Image}\"></Style></Image.Style></Image>");
                        template.AppendFormat("<Label x:Name=\"lbl{0}\" VerticalAlignment=\"Top\" HorizontalAlignment=\"Right\" FontWeight=\"Normal\" FontStyle=\"Italic\">x{1}</Label>", (int)coin.Key, coin.Value);
                    }
                    template.Append("</Canvas></Grid></Border></ControlTemplate></Setter.Value></Setter></Style></ToolTip.Style></ToolTip>");
                    ToolTip tooltip = (ToolTip)XamlReader.Parse(template.ToString());
                    tooltip.AddHandler(LoadedEvent, new RoutedEventHandler((object o, RoutedEventArgs e) => {
                        int i = _drawnCoins.Count - 1;
                        ToolTip tt = o as ToolTip;
                        foreach (KeyValuePair<CoinType, int> coin in _drawnCoins)
                        {
                            Image img = tt.Template.FindName(string.Format("ibar{0}", (int)coin.Key), tt) as Image;
                            Style simg = new Style(typeof(Image));
                            simg.Setters.Add(new Setter(Image.SourceProperty, ResourceLoadHelper.GetCoinsProgressBarLineImage(AccountType.Customer, (int)coin.Key, new Size(100, 30), coin.Value, 10, (Color)ColorConverter.ConvertFromString("Gold"), (Color)ColorConverter.ConvertFromString("Chocolate")).Source));
                            img.Style = simg;
                            Label lbl = tt.Template.FindName(string.Format("lbl{0}", (int)coin.Key), tt) as Label;
                            Canvas.SetTop(lbl, 10 + i * 40);
                            Canvas.SetRight(lbl, 0);
                            Canvas.SetTop(img, i-- * 40);
                        }
                        _drawnCoins.Clear();
                    }));
                    return tooltip;
                }
                return null;
            });
            DataContext = model;
            switch (_controlType)
            {
                case AccountType.Customer:
                    ChangeButton.Visibility = Visibility.Collapsed;
                    Canvas.SetTop(ProgressBar1, 0);
                    Canvas.SetTop(ProgressBar2, 40);
                    Canvas.SetTop(ProgressBar5, 80);
                    Canvas.SetTop(ProgressBar10, 120);
                    break;
                case AccountType.VendingMachine:
                    DropCoins.Visibility = Visibility.Collapsed;
                    Canvas.SetLeft(ProgressBar1, 0);
                    Canvas.SetLeft(ProgressBar2, 40);
                    Canvas.SetLeft(ProgressBar5, 80);
                    Canvas.SetLeft(ProgressBar10, 120);
                    break;
                default:
                    break;
            }
            ProgressBar1.SetControlType(type, CoinType.coin1);
            ProgressBar2.SetControlType(type, CoinType.coin2);
            ProgressBar5.SetControlType(type, CoinType.coin5);
            ProgressBar10.SetControlType(type, CoinType.coin10);
            Binding toolTip = new Binding("PutCoinBox.ToolTip");
            toolTip.Source = model;
            toolTip.Mode = BindingMode.OneWay;
            ChangeButton.SetBinding(Button.ToolTipProperty, toolTip);
            if (_controlType == AccountType.Customer)
            {
                model.DisposeRequest += Dispose;
            }
        }

        private void ChangeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            CoinsViewModel model = DataContext as CoinsViewModel;
            if (model.PayBalance > 0)
            {
                _drawnCoins = model.ChangeSimulate();
                model.ReinitChangeButtonToolTip();
            }
        }

        public void Dispose()
        {
            if (_controlType == AccountType.Customer)
            {
                ProgressBar1.Dispose();
                ProgressBar2.Dispose();
                ProgressBar5.Dispose();
                ProgressBar10.Dispose();
                CoinsViewModel model = DataContext as CoinsViewModel;
                //if (model != null)
                //{
                    model.DisposeRequest -= Dispose;
                //}
            }
        }
    }

    public class CoinsBar : ProgressBar, IDisposable
    {
        private AccountType _type = AccountType.Unknown;
        private CoinType _coinType = CoinType.unknown;
        private Size _sz;
        private Cursor _mainCursor;
        private bool _notClick = false;

        public CoinsBar()
        {
            Minimum = 0;
            Maximum = int.MaxValue;
        }

        public Cursor MainCursor
        {
            get { return _mainCursor; }
        }

        public int CoinValue
        {
            get { return (int)_coinType; }
            set { _coinType = (CoinType)value; }
        }

        public bool NotClick
        {
            get { return _notClick; }
        }

        public void SetControlType(AccountType type, CoinType coin)
        {
            _type = type;
            _coinType = coin;
            string pastButtonOpenTags = string.Empty, pastButtonCloseTags = string.Empty, pastMouseOverTriggerTags = string.Empty;
            if (type == AccountType.Customer)
            {
                pastButtonOpenTags = "<Button x:Name=\"btn\" Background=\"Transparent\" BorderBrush=\"{x:Null}\" Margin=\"0,0,0,0\" Padding=\"0,0,0,0\">";
                pastButtonCloseTags = "<Button.Style><Style TargetType=\"{x:Type Button}\"><Setter Property=\"Width\" Value=\"{Binding ElementName=ibar, Path=Width}\" /><Setter Property=\"Height\" Value=\"{Binding ElementName=ibar, Path=Height}\" /><Setter Property=\"Template\"><Setter.Value><ControlTemplate TargetType=\"{x:Type Button}\"><ContentPresenter /></ControlTemplate></Setter.Value></Setter><Style.Triggers><Trigger Property=\"IsMouseOver\" Value=\"true\" /></Style.Triggers></Style></Button.Style></Button>";
                pastMouseOverTriggerTags = "<ControlTemplate.Triggers><Trigger Property=\"IsMouseOver\" Value=\"true\"><Setter TargetName=\"tbar\" Property=\"BorderBrush\" Value=\"Cyan\" /><Setter TargetName=\"tbar\" Property=\"Background\" Value=\"Brown\" /><Setter TargetName=\"tbar\" Property=\"Foreground\" Value=\"White\" /></Trigger></ControlTemplate.Triggers>";
            }
            StringBuilder template = new StringBuilder("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' TargetType=\"ProgressBar\"><Grid>");
            template.Append(pastButtonOpenTags);
            template.Append("<Image x:Name=\"ibar\" Stretch=\"None\" VerticalAlignment=\"Top\" HorizontalAlignment=\"Left\"><Image.Style><Style TargetType=\"{x:Type Image}\"></Style></Image.Style></Image>");
            template.Append(pastButtonCloseTags);
            template.Append("<TextBox x:Name=\"tbar\" VerticalAlignment=\"Top\" HorizontalAlignment=\"Left\" Width=\"20\" FontSize=\"8\" TextAlignment=\"Right\" TextWrapping=\"NoWrap\" IsReadOnly=\"true\" Background=\"#FFE8E8E8\" /></Grid>");
            template.Append(pastMouseOverTriggerTags);
            template.Append("</ControlTemplate>");
            int left = 14, top = 8, txtmarginleft = 14, txtmargintop = 8;
            CoinsViewModel model = DataContext as CoinsViewModel;
            switch (type)
            {
                case AccountType.Customer:
                    Height = 30;
                    _sz = new Size(100, 30);
                    top = 0;
                    txtmargintop = -9;
                    break;
                case AccountType.VendingMachine:
                    Orientation = Orientation.Vertical;
                    Width = 30;
                    _sz = new Size(30, 100);
                    left = 0;
                    txtmarginleft = -5;
                    break;
                default:
                    _sz = new Size();
                    break;
            }
            AddHandler(LoadedEvent, new RoutedEventHandler((object o, RoutedEventArgs e) => {
                CoinsBar bar = o as CoinsBar;
                switch (type)
                {
                    case AccountType.Customer:
                        Button btn = bar.Template.FindName("btn", bar) as Button;
                        model.InitTemplate(() => { return ResourceLoadHelper.GetCoinsProgressBarLineImage(_type, (int)_coinType, _sz, (int)Value, 10, (Color)ColorConverter.ConvertFromString("Gold"), (Color)ColorConverter.ConvertFromString("Chocolate")).Source; }, _coinType, btn);
                        model.PutCoinBox.Templates[_coinType].PropertyChanged += Template_PropertyChanged;
                        btn.CommandParameter = (int)_coinType;
                        Binding cmd = new Binding();
                        cmd.Source = DataContext;
                        cmd.Path = new PropertyPath("PaymentCommand");
                        btn.SetBinding(Button.CommandProperty, cmd);
                        break;
                    case AccountType.VendingMachine:
                        model.InitTemplate(() => { return ResourceLoadHelper.GetCoinsProgressBarLineImage(_type, (int)_coinType, _sz, (int)Value, 10, (Color)ColorConverter.ConvertFromString("Gold"), (Color)ColorConverter.ConvertFromString("Chocolate")).Source; }, _coinType);
                        break;
                }
                TextBox text = bar.Template.FindName("tbar", bar) as TextBox;
                text.Margin = new Thickness(left - txtmarginleft, top - txtmargintop, 0, 0);
                Binding quantityInfo = new Binding("Value");
                quantityInfo.Source = this;
                quantityInfo.Mode = BindingMode.OneWay;
                text.SetBinding(TextBox.TextProperty, quantityInfo);
                Image img = bar.Template.FindName("ibar", bar) as Image;
                Style simg = new Style(typeof(Image));
                Binding imgSource = new Binding(string.Format("PutCoinBox.Templates[{0}].Coin", (int)_coinType));
                imgSource.Source = model;
                imgSource.Mode = BindingMode.OneWay;
                simg.Setters.Add(new Setter(Image.SourceProperty, imgSource));
                simg.Setters.Add(new Setter(Image.MarginProperty, new Thickness(left, top, 0, 0)));
                img.Style = simg;
            }));
            Template = (ControlTemplate)XamlReader.Parse(template.ToString());
        }

        private void Template_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CoinsViewModel model;
            if (e.PropertyName == "Coin" && (model = DataContext as CoinsViewModel)?.PutCoinBox.Templates[_coinType] != null)
            {
                int i = model.Coins[_coinType].quantity;
                Button b = model.PutCoinBox.Templates[_coinType].ButtonLink as Button;
                if (i <= 10 || b.Visibility == Visibility.Collapsed)
                {
                    b.Visibility = i > 0 ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        public void Dispose()
        {
            if (_type == AccountType.Customer)
            {
                CoinsViewModel model = DataContext as CoinsViewModel;
                //if (model != null)
                //{
                model.PutCoinBox.Templates[_coinType].PropertyChanged -= Template_PropertyChanged;
                //}
            }
        }
    }
}
