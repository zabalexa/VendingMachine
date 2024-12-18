using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;
using VendingMachineViewModel;

namespace VendingMachine.WPF
{
    /// <summary>
    /// Логика взаимодействия для GoodsButtonControl.xaml
    /// </summary>
    public partial class GoodsButtonControl : UserControl, IDisposable
    {
        private readonly ControlTemplate _template;
        private readonly Display _scrollableContainerParams;
        private bool _disposed = false;

        public GoodsButtonControl()
        {
            _template = (ControlTemplate)XamlReader.Parse("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' TargetType=\"Button\"><Border Name=\"brdr\" BorderBrush=\"Black\" BorderThickness=\"1\"><Grid><Image x:Name=\"ibtn\" Stretch=\"None\" VerticalAlignment=\"Center\" HorizontalAlignment=\"Center\"><Image.Style><Style TargetType=\"{x:Type Image}\"></Style></Image.Style></Image><Label x:Name=\"tbtn\" VerticalAlignment=\"Top\" HorizontalAlignment=\"Right\" Margin=\"0,-4,-1,0\" /></Grid></Border><ControlTemplate.Triggers><Trigger Property=\"IsMouseOver\" Value=\"true\"><Setter TargetName=\"brdr\" Property=\"BorderThickness\" Value=\"2\" /><Setter TargetName=\"brdr\" Property=\"BorderBrush\" Value=\"Cyan\" /><Setter TargetName=\"tbtn\" Property=\"Margin\" Value=\"0,-5,-2,0\" /></Trigger></ControlTemplate.Triggers></ControlTemplate>");
            DrinksViewModel model = new DrinksViewModel(ResourceLoadHelper.GetLocalString("GoodsDescription")).BindToMainModel();
            int count = model.Drinks.Count();
            _scrollableContainerParams = new Display((count > 6 ? Math.Ceiling((double)count / 2) : 3) * 102);
            InitializeComponent();
            Drinks.Header = ResourceLoadHelper.GetLocalString("DrinksGroup");
            DataContext = model;
            foreach (Product p in model.Drinks)
            {
                Button b = new Button();
                model.SetToolTip(p.id, p.quantity, p.price, ResourceLoadHelper.GetLocalName(p.locals), b);
                model.SetTemplate(p.id, _template);
                model.DrinkButton[p.id].PropertyChanged += Template_PropertyChanged;
                Binding toolTip = new Binding(string.Format("DrinkButton[{0}].ToolTip", p.id));
                toolTip.Source = model;
                toolTip.Mode = BindingMode.OneWay;
                b.SetBinding(Button.ToolTipProperty, toolTip);
                /*Binding Visible = new Binding(string.Format("DrinkButton[{0}].Visible", p.id));
                Visible.Source = model;
                Visible.Mode = BindingMode.OneWay;
                b.SetBinding(Button.VisibilityProperty, Visible);*/
                Binding Template = new Binding(string.Format("DrinkButton[{0}].Template", p.id));
                Template.Source = model;
                Template.Mode = BindingMode.OneWay;
                b.SetBinding(Button.TemplateProperty, Template);
                b.CommandParameter = p.id;
                Binding cmd = new Binding();
                cmd.Source = model;
                cmd.Path = new PropertyPath("PurchaseCommand");
                b.SetBinding(Button.CommandProperty, cmd);
                b.Width = 100;
                b.Height = 100;
                b.Margin = new Thickness(0, 0, 2, 2);
                Image rimg = ResourceLoadHelper.GetDrinksButtonImage(new Size(96, 96), (Color)ColorConverter.ConvertFromString(p.color), p.type);
                b.AddHandler(LoadedEvent, new RoutedEventHandler((object s, RoutedEventArgs e) => {
                    Button btn = s as Button;
                    if (btn.Template != null)
                    {
                        Label text = btn.Template.FindName("tbtn", btn) as Label;
                        text.Foreground = new SolidColorBrush(p.parsedColor.R + p.parsedColor.G + p.parsedColor.B > 381 ? (Color)ColorConverter.ConvertFromString("Black") : (Color)ColorConverter.ConvertFromString("White"));
                        text.FontWeight = FontWeights.Bold;
                        Binding quantityInfo = new Binding("quantity");
                        quantityInfo.Source = p;
                        quantityInfo.Mode = BindingMode.OneWay;
                        text.SetBinding(Label.ContentProperty, quantityInfo);
                        Image img = btn.Template.FindName("ibtn", btn) as Image;
                        Style simg = new Style(typeof(Image));
                        img.Style.Setters.ToList().ForEach(x => simg.Setters.Add(x));
                        simg.Setters.Add(new Setter(Image.SourceProperty, rimg.Source));
                        img.Style = simg;
                    }
                }));
                DrinksContainer.Children.Add(b);
            }
            model.DisposeRequest += Dispose;
        }

        private void Template_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            DrinksViewModel model;
            Guid id = Guid.Empty;
            if (e.PropertyName == "Template" && (model = DataContext as DrinksViewModel)?.DrinkButton[id = (sender as DrinksViewModel.Display)?.ID ?? Guid.Empty] != null && model.DrinkButton[id].Template == null)
            {
                int count = DrinksContainer.Children.Count - 1;
                _scrollableContainerParams.ScrollableWidth = (count > 6 ? Math.Ceiling((double)count / 2) : 3) * 102;
                DrinksContainer.Children.Remove(model.DrinkButton[id].ButtonLink as Button);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                DrinksViewModel model = DataContext as DrinksViewModel;
                if (model != null)
                {
                    model.DrinkButton.ToList().ForEach(x => { if (x.Value != null) x.Value.PropertyChanged -= Template_PropertyChanged; });
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

        public Display ScrollableContainerParams { get { return _scrollableContainerParams; }}
    }
}
