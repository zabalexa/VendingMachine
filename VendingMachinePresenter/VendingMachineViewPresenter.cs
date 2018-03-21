using System;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;
using VendingMachineModel;
using VendingMachinePresenter.ViewInterfaces;

namespace VendingMachinePresenter
{
    public sealed class VendingMachineViewPresenter : IDisposable
    {
        private readonly IVendingMachineView _view;
        private readonly IPresenterFactory _presenterFactory = new PresenterFactory();
        private readonly IModelFactory _repositoryFactory;
        private readonly ICustomerPurse _customerCashPurseRepository;
        private readonly ICustomerCashButtonControlPresenter _customerCachButtonControlPresenter;
        private readonly IVendingMachineChange _vendingMachineChangeRepository;
        private readonly IVendingMachineChangeCashButtonControlPresenter _vendingMachineChangeCashButtonControlPresenter;
        private readonly IGoods _goodsRepository;
        private readonly IGoodsButtonControlPresenter _goodsButtonControlPresenter;
        private readonly IFillingCupProgressBarControlPresenter _fillingCupProgressBarControlPresenter;
        private bool _payBalanceSuccess = false;

        public VendingMachineViewPresenter(IVendingMachineView view, IModelFactory model)
        {
            _view = view;
            _repositoryFactory = model;
            _customerCashPurseRepository = _repositoryFactory.CustomerPurseProxy;
            _customerCachButtonControlPresenter = _presenterFactory.CreateCustomerCashButtonControlPresenter(_view.CustomerCashButtonControlView, _customerCashPurseRepository);
            _vendingMachineChangeRepository = _repositoryFactory.VendingMachineChangeProxy;
            _vendingMachineChangeCashButtonControlPresenter = _presenterFactory.CreateVendingMachineChangeCashButtonControlPresenter(_view.VendingMachineChangeCashButtonControlView, _vendingMachineChangeRepository);
            _goodsRepository = _repositoryFactory.GoodsProxy;
            _goodsButtonControlPresenter = _presenterFactory.CreateGoodsButtonControlPresenter(_view.GoodsButtonControlView, _goodsRepository);
            _fillingCupProgressBarControlPresenter = _presenterFactory.CreateFillingCupProgressBarControlPresenter(_view.FillingCupProgressBarControlView);
            _view.Shown += _view_Shown;
        }

        private void _view_Shown(object sender, EventArgs e)
        {
            _view.CustomerCashButtonControlView.Activate(e);
            _view.VendingMachineChangeCashButtonControlView.Activate(e);
            _view.GoodsButtonControlView.Activate(e);
            _view.FillingCupProgressBarControlView.Activate(e);
            _view.GoodsButtonControlView.Purchase += Purchase;
            _view.GoodsButtonControlView.ShippingRequest += ShippingRequest;
            _view.CustomerCashButtonControlView.RefreshVendingMachine += RefreshVendingMachine;
            _view.VendingMachineChangeCashButtonControlView.Change += Change;
            _fillingCupProgressBarControlPresenter.GetProduct += GetProduct;
            _customerCachButtonControlPresenter.VendingMachineRepositoryInterfaceRequest += VendingMachineRepositoryInterfaceRequest;
            _vendingMachineChangeCashButtonControlPresenter.GetPayBalance += GetPayBalance;
        }

        public void Dispose()
        {
            _vendingMachineChangeCashButtonControlPresenter.GetPayBalance -= GetPayBalance;
            _customerCachButtonControlPresenter.VendingMachineRepositoryInterfaceRequest -= VendingMachineRepositoryInterfaceRequest;
            _fillingCupProgressBarControlPresenter.GetProduct -= GetProduct;
            _view.VendingMachineChangeCashButtonControlView.Change -= Change;
            _view.CustomerCashButtonControlView.RefreshVendingMachine -= RefreshVendingMachine;
            _view.GoodsButtonControlView.ShippingRequest -= ShippingRequest;
            _view.GoodsButtonControlView.Purchase -= Purchase;
            _view.Shown -= _view_Shown;
            _customerCachButtonControlPresenter.Dispose();
            _vendingMachineChangeCashButtonControlPresenter.Dispose();
            _goodsButtonControlPresenter.Dispose();
            _fillingCupProgressBarControlPresenter.Dispose();
        }

        private int GetPayBalance()
        {
            return _customerCashPurseRepository.PayBalance;
        }

        private void Change()
        {
            int paid = _customerCashPurseRepository.PayBalance;
            if (paid == 0)
            {
                _view.InsufficientMessage();
                return;
            }
            _customerCashPurseRepository.PaymentBack(_vendingMachineChangeRepository.Current, _vendingMachineChangeRepository.GetChange(ref paid));
            _customerCashPurseRepository.ResetPayBalance(paid);
            _view.VendingMachineChangeCashButtonControlView.RefreshBalance(_vendingMachineChangeRepository.Coins);
            _view.CustomerCashButtonControlView.RefreshBalance(_customerCashPurseRepository.Coins, paid);
            if (paid > 0)
            {
                _view.InsufficientMessage();
            }
        }

        private void RefreshVendingMachine()
        {
            _view.VendingMachineChangeCashButtonControlView.RefreshBalance(_vendingMachineChangeRepository.Coins);
        }

        private IVendingMachineChange VendingMachineRepositoryInterfaceRequest()
        {
            return _vendingMachineChangeRepository;
        }

        private bool ShippingRequest(int price)
        {
            if (_payBalanceSuccess = _customerCashPurseRepository.Shipping(price))
            {
                _view.CustomerCashButtonControlView.RefreshBalance(null, _customerCashPurseRepository.PayBalance);
                _view.ThanksMessage();
            }
            else
            {
                _view.InsufficientMessage();
            }
            return _payBalanceSuccess;
        }

        private Product GetProduct(Guid id)
        {
            return _goodsRepository.GetProduct(id);
        }

        private void Purchase(Guid id)
        {
            if (_payBalanceSuccess)
            {
                _view.FillingCupProgressBarControlView.Purchase(id);
            }
        }
    }
}
