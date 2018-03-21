using System;
using VendingMachineDataInjectionInterfaces;
using VendingMachinePresenter.ViewInterfaces;

namespace VendingMachinePresenter
{
    public interface IGoodsButtonControlPresenter : IDisposable
    {

    }

    public sealed class GoodsButtonControlPresenter : IGoodsButtonControlPresenter
    {
        private readonly IGoodsButtonControlView _view;
        private readonly IGoods _repository;

        public GoodsButtonControlPresenter(IGoodsButtonControlView view, IGoods repository)
        {
            _repository = repository;
            _view = view;
            _view.Shown += _view_Shown;
        }

        private void _view_Shown(object sender, EventArgs e)
        {
            _view.Purchase += _view_Purchase;
            _view.RefreshGoods(_repository.Drinks);
        }

        public void Dispose()
        {
            _view.Purchase -= _view_Purchase;
            _view.Shown -= _view_Shown;
        }

        private void _view_Purchase(Guid id)
        {
            if (_view.Shipping(_repository.GetProduct(id).price))
            {
                _repository.Purchase(id);
                _view.RefreshGoods(_repository.Drinks);
            }
        }
    }
}
