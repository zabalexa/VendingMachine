using System.Linq;
using System.Windows.Media;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachineViewModel
{
    public class ProductEx: Product
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    if (_isSelected && _isCoupon)
                    {
                        OnPropertyChanged(nameof(IsCurrentCuopon));
                    }
                    OnPropertyChanged(nameof(Thickness));
                }
            }
        }

        public int Thickness
        {
            get
            {
                return _isSelected ? 2 : 0;
            }
        }

        private bool _isCoupon;
        public bool IsCoupon
        {
            get
            {
                return _isCoupon;
            }
            set
            {
                if (_isCoupon != value)
                {
                    _isCoupon = value;
                    OnPropertyChanged(nameof(IsCoupon));
                }
            }
        }

        public bool IsCurrentCuopon
        {
            get
            {
                return _isSelected && _isCoupon;
            }
        }
    }

    public static class ProductExt
    {
        public static ProductEx CloneX(this Product product, bool coupon = false)
        {
            ProductEx ret = new ProductEx()
            {
                type = product.type,
                argbColor = product.argbColor,
                color = product.color,
                id = product.id,
                local_name = product.local_name,
                locals = product.locals?.ToDictionary(x => x.Key, y => y.Value),
                price = product.price,
                quantity = product.quantity
            };
            ret.IsCoupon = coupon;
            return ret;
        }

        public static Color ColorTranslate(this ProductEx product)
        {
            if (string.IsNullOrEmpty(product.color))
            {
                byte a = (byte)((product.argbColor & 0xFF000000) >> 24);
                byte r = (byte)((product.argbColor & 0x00FF0000) >> 16);
                byte g = (byte)((product.argbColor & 0x0000FF00) >> 8);
                byte b = (byte)(product.argbColor & 0x000000FF);
                return Color.FromArgb(a, r, g, b);
            }
            else
            {
                return (Color)ColorConverter.ConvertFromString(product.color);
            }
        }
    }
}
