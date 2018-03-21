using System;

namespace VendingMachineDataInjectionInterfaces.DomainMappedObjects
{
    public class Coin: MVVM, ICloneable
    {
        private CoinType _type;
        public CoinType type
        {
            get
            {
                return _type;
            }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged(nameof(type));
                }
            }
        }

        private int _quantity;
        public int quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(quantity));
                }
            }
        }

        public object Clone()
        {
            return new Coin() { type = type, quantity = quantity };
        }
    }
}
