using System.Linq;
using System.Windows.Media;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachineViewModel
{
    public class QueueViewModel: EventsAggregator
    {
        private readonly QueueMVVM<ProductEx> _outputQueue = new QueueMVVM<ProductEx>();
        private int _couponRef;
        public event VoidEventHandler<Product> Queue;
        
        public QueueMVVM<ProductEx> QItems
        {
            get { return _outputQueue; }
        }

        public void Visual_Enqueue(Product drink)
        {
            if (!_outputQueue.Any())
            {
                _couponRef = 0;
            }
            _outputQueue.Enqueue(drink.CloneX(++_couponRef % 3 == 0));
            Queue?.Invoke(drink);
        }

        public void Visual_Dequeue()
        {
            _outputQueue.Dequeue();
        }

        public Color Visual_GetColor()
        {
            return _outputQueue.Any() ? _outputQueue.Peek().ColorTranslate() : Colors.Transparent;
        }
    }
}
