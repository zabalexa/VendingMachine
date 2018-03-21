using VendingMachinePresenter.ViewInterfaces;
using VendingMachineDataInjectionInterfaces;

namespace VendingMachinePresenter
{
    public interface IPresenterFactory
    {
        IGoodsButtonControlPresenter CreateGoodsButtonControlPresenter(IGoodsButtonControlView view, IGoods repository);
        ICustomerCashButtonControlPresenter CreateCustomerCashButtonControlPresenter(ICashButtonControlView view, ICustomerPurse repository);
        IVendingMachineChangeCashButtonControlPresenter CreateVendingMachineChangeCashButtonControlPresenter(ICashButtonControlView view, IVendingMachineChange repository);
        IFillingCupProgressBarControlPresenter CreateFillingCupProgressBarControlPresenter(IFillingCupProgressBarControlView view);
    }

    public class PresenterFactory : IPresenterFactory
    {
        public IGoodsButtonControlPresenter CreateGoodsButtonControlPresenter(IGoodsButtonControlView view, IGoods repository)
        {
            return new GoodsButtonControlPresenter(view, repository);
        }

        public ICustomerCashButtonControlPresenter CreateCustomerCashButtonControlPresenter(ICashButtonControlView view, ICustomerPurse repository)
        {
            return new CashButtonControlPresenter(view, repository);
        }

        public IVendingMachineChangeCashButtonControlPresenter CreateVendingMachineChangeCashButtonControlPresenter(ICashButtonControlView view, IVendingMachineChange repository)
        {
            return new CashButtonControlPresenter(view, repository);
        }

        public IFillingCupProgressBarControlPresenter CreateFillingCupProgressBarControlPresenter(IFillingCupProgressBarControlView view)
        {
            return new FillingCupProgressBarControlPresenter(view);
        }
    }
}
