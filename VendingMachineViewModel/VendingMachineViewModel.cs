using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using VendingMachineModel;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachineViewModel
{
    public interface IMultiEventsAggregator//Visitor
    {
        event VoidEventHandler UpdatePutCoinBoxRequest, DisposeRequest;
        event VoidEventHandler<AccountType, CoinType> UpdateTemplatesRequest;
        void UpdatePutCoinBox();
        void UpdateTemplates(AccountType type, CoinType coin);
        void VisualEnqueue(Product drink);
        void ThanksMessage();
        void InsufficientMessage();
        ICustomerPurse GetCustomerCashPurse();
        IVendingMachineChange GetVendingMachineChange();
        IGoods GetGoods();
        DrinksViewModel Subscribe(DrinksViewModel subscriber);
        CoinsViewModel Subscribe(CoinsViewModel subscriber);
        QueueViewModel Subscribe(QueueViewModel subscriber);
    }

    public interface IPaymentEventsPublsher//Visitor
    {
        event RequestEventHandler<int> GetPayBalanceRequest;
        event RequestEventHandler<bool> PaymentBackRequest;
        event RequestEventHandler<int, CoinType> GetCoinsRequest;
        event RequestEventHandler<bool, Dictionary<CoinType, int>> PaymentRequest;
        ICommand Subscribe(CoinsViewModel subscriber);
    }

    public interface IPurchaseEventsPublsher//Visitor
    {
        event RequestEventHandler<int, Guid> GetDrinksQtyRequest, GetDrinksPriceRequest;
        event RequestEventHandler<bool, int> ShippingRequest;
        event VoidEventHandler<Guid> PurchaseRequest, DrinksButtonHideRequest;
        event VoidEventHandler ThanksMessageRequest, InsufficientMessageRequest;
        ICommand Subscribe(DrinksViewModel subscriber);
    }

    public abstract class EventsAggregator: IDisposable
    {
        public event RequestEventHandler<ICustomerPurse> GetCustomerCashPurseRequest;
        public event RequestEventHandler<IVendingMachineChange> GetVendingMachineChangeRequest;
        public event RequestEventHandler<IGoods> GetGoodsRequest;
        public event VoidEventHandler<Product> VisualEnqueueRequest;
        public event VoidEventHandler RefreshPutCoinBoxRequest, ThanksMessageRequest, InsufficientMessageRequest, DisposeRequest;
        public event VoidEventHandler<AccountType, CoinType> RefreshTemplatesRequest;

        public virtual void Initialize()
        {
        }

        public void Publish(BaseCommand subscriber)
        {
            DisposeRequest += subscriber.Dispose;
        }

        public void Publish(IMultiEventsAggregator subscriber)
        {
            GetCustomerCashPurseRequest += subscriber.GetCustomerCashPurse;
            GetVendingMachineChangeRequest += subscriber.GetVendingMachineChange;
            GetGoodsRequest += subscriber.GetGoods;
            RefreshPutCoinBoxRequest += subscriber.UpdatePutCoinBox;
            RefreshTemplatesRequest += subscriber.UpdateTemplates;
            VisualEnqueueRequest += subscriber.VisualEnqueue;
            ThanksMessageRequest += subscriber.ThanksMessage;
            InsufficientMessageRequest += subscriber.InsufficientMessage;
        }

        protected ICustomerPurse GetCustomerCashPurse()
        {
            return GetCustomerCashPurseRequest?.Invoke();
        }

        protected IVendingMachineChange GetVendingMachineChange()
        {
            return GetVendingMachineChangeRequest?.Invoke();
        }

        protected IGoods GetGoods()
        {
            return GetGoodsRequest?.Invoke();
        }

        protected void VisualEnqueue(Product drink)
        {
            VisualEnqueueRequest?.Invoke(drink);
        }

        protected void RefreshPutCoinBox()
        {
            RefreshPutCoinBoxRequest?.Invoke();
        }

        protected void RefreshTemplates(AccountType type, CoinType coin)
        {
            RefreshTemplatesRequest?.Invoke(type, coin);
        }

        protected void PostThanksMessage()
        {
            ThanksMessageRequest?.Invoke();
        }

        protected void PostInsufficientMessage()
        {
            InsufficientMessageRequest?.Invoke();
        }

        #region IDisposable Support
        public void Dispose()
        {
            if (GetCustomerCashPurseRequest != null)
            {
                GetCustomerCashPurseRequest.GetInvocationList().ToList().ForEach(x => GetCustomerCashPurseRequest -= (RequestEventHandler<ICustomerPurse>)x);
            }
            if (GetVendingMachineChangeRequest != null)
            {
                GetVendingMachineChangeRequest.GetInvocationList().ToList().ForEach(x => GetVendingMachineChangeRequest -= (RequestEventHandler<IVendingMachineChange>)x);
            }
            if (GetGoodsRequest != null)
            {
                GetGoodsRequest.GetInvocationList().ToList().ForEach(x => GetGoodsRequest -= (RequestEventHandler<IGoods>)x);
            }
            if (VisualEnqueueRequest != null)
            {
                VisualEnqueueRequest.GetInvocationList().ToList().ForEach(x => VisualEnqueueRequest -= (VoidEventHandler<Product>)x);
            }
            if (RefreshPutCoinBoxRequest != null)
            {
                RefreshPutCoinBoxRequest.GetInvocationList().ToList().ForEach(x => RefreshPutCoinBoxRequest -= (VoidEventHandler)x);
            }
            if (RefreshTemplatesRequest != null)
            {
                RefreshTemplatesRequest.GetInvocationList().ToList().ForEach(x => RefreshTemplatesRequest -= (VoidEventHandler<AccountType, CoinType>)x);
            }
            if (DisposeRequest != null)
            {
                DisposeRequest();
                DisposeRequest?.GetInvocationList().ToList().ForEach(x => DisposeRequest -= (VoidEventHandler)x);
            }
        }
        #endregion
    }

    public abstract class BaseCommand: ICommand, IPaymentEventsPublsher, IPurchaseEventsPublsher, IDisposable
    {
        public event RequestEventHandler<int> GetPayBalanceRequest;
        public event RequestEventHandler<bool> PaymentBackRequest;
        public event RequestEventHandler<int, CoinType> GetCoinsRequest;
        public event RequestEventHandler<bool, Dictionary<CoinType, int>> PaymentRequest;
        public event RequestEventHandler<int, Guid> GetDrinksQtyRequest, GetDrinksPriceRequest;
        public event RequestEventHandler<bool, int> ShippingRequest;
        public event VoidEventHandler<Guid> PurchaseRequest, DrinksButtonHideRequest;
        public event VoidEventHandler ThanksMessageRequest, InsufficientMessageRequest;
        public event RequestEventHandler<Product, Guid> GetProduct;

        #region ICommand Support
        public abstract void Execute(object parameter);

        public abstract bool CanExecute(object parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion

        #region IPaymentEventsPublsher Support
        public ICommand Subscribe(CoinsViewModel subscriber)
        {
            GetPayBalanceRequest += subscriber.GetPayBalance;
            PaymentBackRequest += subscriber.PaymentBack;
            GetCoinsRequest += subscriber.GetCoinsQty;
            PaymentRequest += subscriber.Payment;
            subscriber.Publish(this);
            return this;
        }

        protected int GetPayBalance()
        {
            return GetPayBalanceRequest?.Invoke() ?? 0;
        }

        protected bool PaymentBack()
        {
            return PaymentBackRequest?.Invoke() ?? false;
        }

        protected int GetCoinsQty(CoinType type)
        {
            return GetCoinsRequest?.Invoke(type) ?? 0;
        }

        protected bool Payment(Dictionary<CoinType, int> coins)
        {
            return PaymentRequest?.Invoke(coins) ?? false;
        }
        #endregion

        #region IPurchaseEventsPublsher Support
        public ICommand Subscribe(DrinksViewModel subscriber)
        {
            GetDrinksQtyRequest += subscriber.GetDrinksQty;
            ShippingRequest += subscriber.Shipping;
            GetDrinksPriceRequest += subscriber.GetDrinksPrice;
            PurchaseRequest += subscriber.Purchase;
            ThanksMessageRequest += subscriber.ThanksMessage;
            DrinksButtonHideRequest += subscriber.DrinksButtonHide;
            InsufficientMessageRequest += subscriber.InsufficientMessage;
            subscriber.Publish(this);
            return this;
        }

        protected int GetDrinksQty(Guid pid)
        {
            return GetDrinksQtyRequest?.Invoke(pid) ?? 0;
        }

        protected bool Shipping(int price)
        {
            return ShippingRequest?.Invoke(price) ?? false;
        }

        protected int GetDrinksPrice(Guid pid)
        {
            return GetDrinksPriceRequest?.Invoke(pid) ?? 0;
        }

        protected void Purchase(Guid pid)
        {
            PurchaseRequest?.Invoke(pid);
        }

        protected void ThanksMessage()
        {
            ThanksMessageRequest?.Invoke();
        }

        protected void DrinksButtonHide(Guid pid)
        {
            DrinksButtonHideRequest?.Invoke(pid);
        }

        protected void InsufficientMessage()
        {
            InsufficientMessageRequest?.Invoke();
        }
        #endregion

        #region IDisposable Support
        public void Dispose()
        {
            if (GetPayBalanceRequest != null)
            {
                GetPayBalanceRequest.GetInvocationList().ToList().ForEach(x => GetPayBalanceRequest -= (RequestEventHandler<int>)x);
            }
            if (PaymentBackRequest != null)
            {
                PaymentBackRequest.GetInvocationList().ToList().ForEach(x => PaymentBackRequest -= (RequestEventHandler<bool>)x);
            }
            if (GetCoinsRequest != null)
            {
                GetCoinsRequest.GetInvocationList().ToList().ForEach(x => GetCoinsRequest -= (RequestEventHandler<int, CoinType>)x);
            }
            if (PaymentRequest != null)
            {
                PaymentRequest.GetInvocationList().ToList().ForEach(x => PaymentRequest -= (RequestEventHandler<bool, Dictionary<CoinType, int>>)x);
            }
            if (GetDrinksQtyRequest != null)
            {
                GetDrinksQtyRequest.GetInvocationList().ToList().ForEach(x => GetDrinksQtyRequest -= (RequestEventHandler<int, Guid>)x);
            }
            if (ShippingRequest != null)
            {
                ShippingRequest.GetInvocationList().ToList().ForEach(x => ShippingRequest -= (RequestEventHandler<bool, int>)x);
            }
            if (GetDrinksPriceRequest != null)
            {
                GetDrinksPriceRequest.GetInvocationList().ToList().ForEach(x => GetDrinksPriceRequest -= (RequestEventHandler<int, Guid>)x);
            }
            if (PurchaseRequest != null)
            {
                PurchaseRequest.GetInvocationList().ToList().ForEach(x => PurchaseRequest -= (VoidEventHandler<Guid>)x);
            }
            if (ThanksMessageRequest != null)
            {
                ThanksMessageRequest.GetInvocationList().ToList().ForEach(x => ThanksMessageRequest -= (VoidEventHandler)x);
            }
            if (InsufficientMessageRequest != null)
            {
                InsufficientMessageRequest.GetInvocationList().ToList().ForEach(x => InsufficientMessageRequest -= (VoidEventHandler)x);
            }
        }
        #endregion
    }

    public class VendingMachineViewModel: IMultiEventsAggregator, IDisposable
    {
        private static VendingMachineViewModel _current;
        private readonly IModelFactory _repositoryFactory;
        private readonly ICustomerPurse _customerCashPurse;
        private readonly IVendingMachineChange _vendingMachineChange;
        private readonly IGoods _goods;
        private readonly Display _display = new Display();
        public event VoidEventHandler UpdatePutCoinBoxRequest, DisposeRequest;
        public event VoidEventHandler<AccountType, CoinType> UpdateTemplatesRequest;
        public event VoidEventHandler<Product> VisualEnqueueRequest;

        public VendingMachineViewModel()
        {
            _repositoryFactory = new ModelFactory(null);
            _customerCashPurse = _repositoryFactory.CustomerPurseProxy;
            _vendingMachineChange = _repositoryFactory.VendingMachineChangeProxy;
            _goods = _repositoryFactory.GoodsProxy;
        }

        public static VendingMachineViewModel Current
        {
            get
            {
                lock ("Initialize")
                {
                    return _current ?? (_current = new VendingMachineViewModel());
                }
            }
        }

        public class Display : MVVM
        {
            private string _text = string.Empty;
            public string Text
            {
                get
                {
                    return _text;
                }
                set
                {
                    if (_text != value)
                    {
                        _text = value;
                        OnPropertyChanged(nameof(Text));
                    }
                }
            }
        }

        public Display MessageToCustomer
        {
            get { return _display; }
        }

        #region IMultiEventAggregator Support
        public DrinksViewModel Subscribe(DrinksViewModel subscriber)
        {
            DisposeRequest += subscriber.Dispose;
            subscriber.Publish(this);
            subscriber.Initialize();
            return subscriber;
        }

        public CoinsViewModel Subscribe(CoinsViewModel subscriber)
        {
            UpdatePutCoinBoxRequest += subscriber.UpdatePutCoinBox;
            UpdateTemplatesRequest += subscriber.UpdateTemplates;
            DisposeRequest += subscriber.Dispose;
            subscriber.Publish(this);
            subscriber.Initialize();
            return subscriber;
        }

        public QueueViewModel Subscribe(QueueViewModel subscriber)
        {
            DisposeRequest += subscriber.Dispose;
            VisualEnqueueRequest += subscriber.Visual_Enqueue;
            subscriber.Publish(this);
            subscriber.Initialize();
            return subscriber;
        }

        public ICustomerPurse GetCustomerCashPurse()
        {
            return _customerCashPurse;
        }

        public IVendingMachineChange GetVendingMachineChange()
        {
            return _vendingMachineChange;
        }

        public IGoods GetGoods()
        {
            return _goods;
        }

        public void UpdatePutCoinBox()
        {
            UpdatePutCoinBoxRequest?.Invoke();
        }

        public void UpdateTemplates(AccountType type, CoinType coin)
        {
            UpdateTemplatesRequest?.Invoke(type, coin);
        }

        public void VisualEnqueue(Product drink)
        {
            VisualEnqueueRequest?.Invoke(drink);
        }

        public void ThanksMessage()
        {
            _display.Text = "ThanksMessage";
        }

        public void InsufficientMessage()
        {
            _display.Text = "InsufficientMessage";
        }
        #endregion

        #region IDisposable Support
        public void Dispose()
        {
            if (UpdatePutCoinBoxRequest != null)
            {
                UpdatePutCoinBoxRequest.GetInvocationList().ToList().ForEach(x => UpdatePutCoinBoxRequest -= (VoidEventHandler)x);
            }
            if (UpdateTemplatesRequest != null)
            {
                UpdateTemplatesRequest.GetInvocationList().ToList().ForEach(x => UpdateTemplatesRequest -= (VoidEventHandler<AccountType, CoinType>)x);
            }
            if (VisualEnqueueRequest != null)
            {
                VisualEnqueueRequest.GetInvocationList().ToList().ForEach(x => VisualEnqueueRequest -= (VoidEventHandler<Product>)x);
            }
            if (DisposeRequest != null)
            {
                DisposeRequest();
                DisposeRequest?.GetInvocationList().ToList().ForEach(x => DisposeRequest -= (VoidEventHandler)x);
            }
        }
        #endregion
    }
}
