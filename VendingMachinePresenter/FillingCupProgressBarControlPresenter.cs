using System;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;
using VendingMachinePresenter.ViewInterfaces;

namespace VendingMachinePresenter
{
    public interface IFillingCupProgressBarControlPresenter : IDisposable
    {
        event RequestEventHandler<Product, Guid> GetProduct;
    }

    public sealed class FillingCupProgressBarControlPresenter : IFillingCupProgressBarControlPresenter
    {
        private readonly IFillingCupProgressBarControlView _view;
        private Queue<Product> _outputQueue = new Queue<Product>();
        public event RequestEventHandler<Product, Guid> GetProduct;

        public FillingCupProgressBarControlPresenter(IFillingCupProgressBarControlView view)
        {
            _view = view;
            _view.Shown += _view_Shown;
        }

        private void _view_Shown(object sender, EventArgs e)
        {
            _view.ProductGetOut += _view_ProductGetOut;
            _view.Dequeue += _view_Dequeue;
        }

        public void Dispose()
        {
            _view.Dequeue -= _view_Dequeue;
            _view.ProductGetOut -= _view_ProductGetOut;
            _view.Shown -= _view_Shown;
        }

        private void _view_Dequeue()
        {
            _outputQueue.Dequeue();
            _view.RefreshQueue(_outputQueue);
        }

        private void _view_ProductGetOut(Guid id)
        {
            if(GetProduct == null)
            {
                return;
            }
            Product drink = GetProduct(id);
            if (drink != null)
            {
                _outputQueue.Enqueue(drink);
                _view.RefreshQueue(_outputQueue);
            }
        }
    }
}
