using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachineViewModel
{
    public class CoinsViewModel : EventsAggregator
    {
        private readonly AccountType _type;
        private Display _display;
        private List<Coin> _coins;//ObservableCollection isn't need

        public CoinsViewModel(AccountType type)
        {
            _type = type;
        }

        public override void Initialize()
        {
            _coins = (_type == AccountType.Customer ? (ICoinsRepository)GetCustomerCashPurse().Repo : GetVendingMachineChange().Repo).GetAll().ToList();
        }

        public class Display : MVVM
        {
            public delegate R TemplateFn<R>();
            private TemplateFn<object> _fn;
            private object _toolTip;
            private int _payBalance = 0;

            public Display(TemplateFn<object> templateFn)
            {
                _fn = templateFn;
                Templates = new Dictionary<CoinType, Template>();
            }

            public class Template : MVVM
            {
                private TemplateFn<object> _fn;
                private object _coin;

                public Template(TemplateFn<object> fn, object btnLink)
                {
                    _fn = fn;
                    ButtonLink = btnLink;
                }

                public object Coin
                {
                    get
                    {
                        return _coin ?? (_coin = _fn?.Invoke());
                    }
                    set
                    {
                        _coin = value;
                        OnPropertyChanged(nameof(Coin));
                    }
                }

                public object ButtonLink { get; private set; }
            }

            public object ToolTip
            {
                get
                {
                    return _toolTip ?? (_toolTip = _fn?.Invoke());
                }
                set
                {
                    _toolTip = value;
                    OnPropertyChanged(nameof(ToolTip));
                }
            }

            public int PayBalance
            {
                get { return _payBalance; }
                set
                {
                    if (_payBalance != value)
                    {
                        _payBalance = value;
                        OnPropertyChanged(nameof(PayBalance));
                    }
                }
            }

            private int _draggingCoin;
            public int DraggingCoin
            {
                get { return _draggingCoin; }
                set
                {
                    if (_draggingCoin != value)
                    {
                        _draggingCoin = value;
                        OnPropertyChanged(nameof(DraggingCoin));
                    }
                }
            }

            public Dictionary<CoinType, Template> Templates { get; set; }
        }

        public Display PutCoinBox
        {
            get { return _display; }
        }

        public Dictionary<CoinType, int> ChangeSimulate()
        {
            int paid = PayBalance;
            return GetVendingMachineChange().GetChange(ref paid);
        }

        public void ReinitChangeButtonToolTip()
        {
            _display.ToolTip = null;
        }

        public void InitTemplate(Display.TemplateFn<object> templateFn, CoinType type = CoinType.unknown, object btnLink = null)
        {
            if (_display == null)
            {
                _display = new Display(templateFn);
            }
            else
            {
                _display.Templates.Add(type, new Display.Template(templateFn, btnLink));
            }
        }

        public void UpdateTemplates(AccountType type, CoinType coin)
        {
            if (type != _type)
            {
                _display.Templates[coin].Coin = null;
            }
        }

        public void UpdatePutCoinBox()
        {
            if (_type == AccountType.Customer)
            {
                _display.PayBalance = GetCustomerCashPurse().PayBalance;
            }
            else
            {
                _display.ToolTip = null;
            }
        }

        public ICommand ChangeCommand
        {
            get { return new ChangeCommand().BindCommandToModel(this); }
        }

        public ICommand PaymentCommand
        {
            get { return new PaymentCommand().BindCommandToModel(this); }
        }

        public ICommand FocusCommand
        {
            get { return new FocusCommand(); }
        }

        public Dictionary<CoinType, Coin> Coins
        {
            get { return _coins.ToDictionary(x => x.type, y => y); }
        }

        public int PayBalance
        {
            get { return _type == AccountType.Customer ? _display.PayBalance : GetCustomerCashPurse().PayBalance; }
        }

        private void UpdateCoinsTemplate(IEnumerable<CoinType> coins, Dictionary<CoinType, bool> preview = null)
        {
            foreach (CoinType coin in coins)
            {
                if ((_coins.FirstOrDefault(x => x.type == coin)?.quantity ?? 10) < 10)
                {
                    UpdateTemplates(AccountType.Unknown, coin);
                }
                if ((preview?[coin] ?? false) || (_type == AccountType.Customer ? (IBaseCoinsRepository)GetVendingMachineChange().Repo : GetCustomerCashPurse().Repo).Coins[coin].quantity <= 10)
                {
                    RefreshTemplates(_type, coin);
                }
            }
        }

        private Dictionary<CoinType, bool> NeedRefresh(IEnumerable<CoinType> coins)
        {
            Dictionary<CoinType, bool> refresh = new Dictionary<CoinType, bool>();
            Dictionary<CoinType, Coin> old = GetCustomerCashPurse().Coins;
            foreach (CoinType coin in coins)
            {
                refresh.Add(coin, false);
                if (old[coin].quantity < 10)
                {
                    refresh[coin] = true;
                }
            }
            return refresh;
        }

        public int GetPayBalance()
        {
            return PayBalance;
        }

        public bool PaymentBack()
        {
            int paid = PayBalance;
            Dictionary<CoinType, int> coins = GetVendingMachineChange().GetChange(ref paid);
            Dictionary<CoinType, bool> preview = NeedRefresh(coins.Keys);
            bool res = GetCustomerCashPurse().PaymentBack(GetVendingMachineChange().Repo, coins);
            GetCustomerCashPurse().ResetPayBalance(paid);
            UpdateCoinsTemplate(coins.Keys, preview);
            RefreshPutCoinBox();
            return res;
        }

        public int GetCoinsQty(CoinType type)
        {
            return Coins[type].quantity;
        }

        public bool Payment(Dictionary<CoinType, int> coins)
        {
            bool res = GetCustomerCashPurse().Payment(GetVendingMachineChange().Repo, coins);
            if (res)
            {
                UpdatePutCoinBox();
                UpdateCoinsTemplate(coins.Keys);
            }
            return res;
        }
    }
}
