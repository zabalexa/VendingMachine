using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace VendingMachineDataInjectionInterfaces.DomainMappedObjects
{
    public class Product: MVVM, ICloneable
    {
        public Guid id { get; set; }

        private string _local_name;
        public string local_name
        {
            get
            {
                return _local_name;
            }
            set
            {
                if (_local_name != value)
                {
                    _local_name = value;
                    //OnPropertyChanged(nameof(local_name));
                }
            }
        }

        private string _color;
        public string color
        {
            get
            {
                return _color;
            }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    //OnPropertyChanged(nameof(color));
                }
            }
        }

        private int _price;
        public int price
        {
            get
            {
                return _price;
            }
            set
            {
                if (_price != value)
                {
                    _price = value;
                    //OnPropertyChanged(nameof(price));
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

        private DrinkType _type;
        public DrinkType type
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
                    //OnPropertyChanged(nameof(type));
                }
            }
        }

        public Color parsedColor
        {
            get { return ColorTranslator.FromHtml(color); }
            set { color = value.ToHtml(); }
        }

        //private ReadOnlyObservableCollection<KeyValuePair<string, string>> _locals;
        public Dictionary<string, string> locals { get; set; }
        //{
        //    get
        //    {
        //        return _locals.ToDictionary(x => x.Key, y => y.Value);
        //    }
        //    set
        //    {
        //        if (_locals.Count() != value.Count())
        //        {
        //            _locals = new ReadOnlyObservableCollection<KeyValuePair<string, string>>(new ObservableCollection<KeyValuePair<string, string>>(value));
        //            //OnPropertyChanged(nameof(locals));
        //        }
        //    }
        //}

        #region ICloneable Support
        public object Clone()
        {
            return new Product() { id = id, local_name = local_name, color = color, price = price, quantity = quantity, type = type, locals = locals?.ToDictionary(x => x.Key, y => y.Value) };
        }
        #endregion
    }

    public static class ProductExt
    {
        public static string ToHtml(this Color color)
        {
            return ColorTranslator.ToHtml(color);
        }
    }
}
