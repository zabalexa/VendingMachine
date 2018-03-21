using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces;
using VendingMachinePresenter.ViewInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachine.WinForms
{
    public partial class GoodsButtonControl : UserControl, IGoodsButtonControlView
    {
        private Dictionary<Guid, Button> _buttons;
        private Dictionary<Guid, EventHandler> _goodsButtonHandlers = new Dictionary<Guid, EventHandler>();
        public event EventHandler Shown;
        public event VoidEventHandler<Guid> Purchase;
        public event RequestEventHandler<bool, int> ShippingRequest;

        public GoodsButtonControl()
        {
            InitializeComponent();
            groupBox1.Text = ResourceLoadHelper.GetLocalString("DrinksGroup");
        }

        public void Activate(EventArgs e)
        {
            if (Shown != null)
            {
                Shown(this, e);
            }
        }

        public void RefreshGoods(Dictionary<Guid, Product> drinks)
        {
            descriptionToolTips.RemoveAll();
            if (drinks.Any() && (_buttons == null))
            {
                _buttons = drinks.Values.ToDictionary(x => x.id, y => new Button() { Size = new Size(94, 94), Font = new Font(Font, FontStyle.Bold), TextAlign = ContentAlignment.TopRight, ForeColor = y.parsedColor.R + y.parsedColor.G + y.parsedColor.B > 381 ? Color.Black : Color.White, BackColor =y.parsedColor, BackgroundImageLayout = ImageLayout.Stretch, BackgroundImage = y.type == DrinkType.None ? null : ResourceLoadHelper.GetImage(ResourceLoadHelper.GetImageName(y.type)) });
                foreach (KeyValuePair<Guid, Button> btn in _buttons)
                {
                    _goodsButtonHandlers.Add(btn.Key, (s, e) => { if (Purchase != null) Purchase(btn.Key); });
                    btn.Value.Click += _goodsButtonHandlers[btn.Key];
                }
                flowLayoutPanel1.Controls.AddRange(_buttons.Values.ToArray());
            }
            else
            {
                if (_buttons == null)
                {
                    return;
                }
                List<Guid> delete = _buttons.Keys.Except(drinks.Keys).ToList();
                foreach(Guid guid in delete)
                {
                    _buttons[guid].Click -= _goodsButtonHandlers[guid];
                    flowLayoutPanel1.Controls.Remove(_buttons[guid]);
                    _buttons.Remove(guid);
                }
            }
            if (_buttons.Any())
            {
                foreach (Product drink in drinks.Values)
                {
                    _buttons[drink.id].Text = drink.quantity.ToString();
                    descriptionToolTips.SetToolTip(_buttons[drink.id], string.Format(ResourceLoadHelper.GetLocalString("GoodsDescription"), ResourceLoadHelper.GetLocalName(drink.locals), drink.price, drink.quantity));
                }
            }
        }

        public bool Shipping(int price)
        {
            if ((ShippingRequest != null) && (price > 0))
            {
                return ShippingRequest(price);
            }
            return false;
        }

        public void Unsubscribe()
        {
            if (_buttons != null)
            {
                foreach (KeyValuePair<Guid, Button> btn in _buttons)
                {
                    btn.Value.Click -= _goodsButtonHandlers[btn.Key];
                }
            }
        }
    }
}
