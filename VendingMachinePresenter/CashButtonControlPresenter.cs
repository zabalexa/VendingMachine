using System;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces;
using VendingMachinePresenter.ViewInterfaces;

namespace VendingMachinePresenter
{
    public interface ICustomerCashButtonControlPresenter : IDisposable
    {
        event RequestEventHandler<IVendingMachineChange> VendingMachineRepositoryInterfaceRequest;
    }

    public interface IVendingMachineChangeCashButtonControlPresenter : IDisposable
    {
        event RequestEventHandler<int> GetPayBalance;
    }

    public sealed class CashButtonControlPresenter : ICustomerCashButtonControlPresenter, IVendingMachineChangeCashButtonControlPresenter
    {
        private readonly ICashButtonControlView _view;
        private readonly ICustomerPurse _customerCashRepository;
        private readonly IVendingMachineChange _vendingMachineChangeRepository;
        private readonly AccountType _controlKind;
        public event RequestEventHandler<IVendingMachineChange> VendingMachineRepositoryInterfaceRequest;
        public event RequestEventHandler<int> GetPayBalance;

        public CashButtonControlPresenter(ICashButtonControlView view, ICustomerPurse repository)
        {
            _controlKind = AccountType.Customer;
            _customerCashRepository = repository;
            _view = view;
            _view.Shown += _view_Shown;
        }

        private void _view_PutCoin(CoinType type)
        {
            if (VendingMachineRepositoryInterfaceRequest == null)
            {
                return;
            }
            Dictionary<CoinType, int> coins = new Dictionary<CoinType, int>();
            coins.Add(type, 1);
            _customerCashRepository.Payment(VendingMachineRepositoryInterfaceRequest().Current, coins);
            _view.RefreshBalance(_customerCashRepository.Coins, _customerCashRepository.PayBalance);
            _view.RefreshVendingMachineBalance();
        }

        public CashButtonControlPresenter(ICashButtonControlView view, IVendingMachineChange repository)
        {
            _controlKind = AccountType.VendingMachine;
            _vendingMachineChangeRepository = repository;
            _view = view;
            _view.Shown += _view_Shown;
        }

        private Dictionary<CoinType, int> _view_ChangeSimulate()
        {
            if (GetPayBalance == null)
            {
                return new Dictionary<CoinType, int>();
            }
            int paid = GetPayBalance();
            return _vendingMachineChangeRepository.GetChange(ref paid);
        }

        private void _view_Shown(object sender, EventArgs e)
        {
            _view.SetControlType(_controlKind);
            switch(_controlKind)
            {
                case AccountType.Customer:
                    _view.PutCoin += _view_PutCoin;
                    break;
                case AccountType.VendingMachine:
                    _view.ChangeSimulate += _view_ChangeSimulate;
                    break;
            }
            Refresh();
        }

        private void Refresh()
        {
            switch (_controlKind)
            {
                case AccountType.Customer:
                    _view.RefreshBalance(_customerCashRepository.Coins, _customerCashRepository.PayBalance);
                    break;
                case AccountType.VendingMachine:
                    _view.RefreshBalance(_vendingMachineChangeRepository.Coins);
                    break;
            }
        }

        public void Dispose()
        {
            switch (_controlKind)
            {
                case AccountType.Customer:
                    _view.PutCoin -= _view_PutCoin;
                    break;
                case AccountType.VendingMachine:
                    _view.ChangeSimulate -= _view_ChangeSimulate;
                    break;
            }
            _view.Shown -= _view_Shown;
        }
    }
}
