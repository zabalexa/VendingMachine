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
                }
            }
        }

        public Color parsedColor
        {
            get { return ColorTranslator.FromHtml(color); }
            set { color = value.ToHtml(); }
        }

        public int argbColor
        {
            get { return parsedColor.ToArgb(); }
            set { parsedColor = Color.FromArgb(value); }
        }

        public Dictionary<string, string> locals { get; set; }

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
