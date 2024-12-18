using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachineViewModel
{
    public class DrinksViewModel : EventsAggregator
    {
        private readonly string _templateButtonToolTip = string.Empty;
        private readonly Dictionary<Guid, Display> _drinkButtons = new Dictionary<Guid, Display>();
        private List<Product> _drinks;//ObservableCollection isn't need

        public DrinksViewModel(string templateButtonToolTip)
        {
            _templateButtonToolTip = templateButtonToolTip;
        }

        public override void Initialize()
        {
            _drinkButtons.Add(Guid.Empty, null);
            _drinks = GetGoods().Repo.GetAll().ToList();
        }

        public class Display : MVVM
        {
            private string _toolTip = string.Empty;
            private Visibility _visibility = Visibility.Visible;
            private object _template;

            public Display(Guid id, object btnLink, string name, int price)
            {
                ID = id;
                ButtonLink = btnLink;
                TemplateName = name;
                TemplatePrice = price;
            }

            public string ToolTip
            {
                get { return _toolTip; }
                set
                {
                    if (_toolTip != value)
                    {
                        _toolTip = value;
                        OnPropertyChanged(nameof(ToolTip));
                    }
                }
            }

            public Visibility Visibile
            {
                get { return _visibility; }
                set
                {
                    if (_visibility != value)
                    {
                        _visibility = value;
                        OnPropertyChanged(nameof(Visibile));
                    }
                }
            }

            public object Template
            {
                get { return _template; }
                set
                {
                    _template = value;
                    OnPropertyChanged(nameof(Template));
                }
            }

            public Guid ID { get; private set; }

            public object ButtonLink { get; private set; }

            public string TemplateName { get; private set; }

            public int TemplatePrice { get; private set; }
        }

        public Dictionary<Guid, Display> DrinkButton
        {
            get { return _drinkButtons; }
        }

        public void SetToolTip(Guid id, int quantity, int price = 0, string name = "", object btnLink = null)
        {
            if (!_drinkButtons.ContainsKey(id))
            {
                _drinkButtons.Add(id, new Display(id, btnLink, name, price));
            }
            _drinkButtons[id].ToolTip = string.Format(_templateButtonToolTip, _drinkButtons[id].TemplateName, _drinkButtons[id].TemplatePrice, quantity);
        }

        public void SetTemplate(Guid id, object template)
        {
            _drinkButtons[id].Template = template;
        }

        public List<Product> Drinks
        {
            get { return _drinks; }
        }

        public ICommand PurchaseCommand
        {
            get { return new PurchaseCommand().BindCommandToModel(this); }
        }

        public int GetDrinksQty(Guid pid)
        {
            return (_drinks.FirstOrDefault(x => x.id == pid)?.quantity ?? 0);
        }

        public bool Shipping(int price)
        {
            return GetCustomerCashPurse().Shipping(price);
        }

        public int GetDrinksPrice(Guid pid)
        {
            return (_drinks.FirstOrDefault(x => x.id == pid)?.price ?? -1);
        }

        public void Purchase(Guid pid)
        {
            Product p = _drinks.FirstOrDefault(x => x.id == pid);
            GetGoods().Purchase(pid);
            RefreshPutCoinBox();
            SetToolTip(pid, GetDrinksQty(pid));
            VisualEnqueue(p);
        }

        public void ThanksMessage()
        {
            PostThanksMessage();
        }

        public void DrinksButtonHide(Guid pid)
        {
            _drinkButtons[pid].Visibile = Visibility.Collapsed;
            _drinkButtons[pid].Template = null;
        }

        public void InsufficientMessage()
        {
            PostInsufficientMessage();
        }
    }
}
