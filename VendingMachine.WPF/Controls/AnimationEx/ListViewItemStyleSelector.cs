using System.Windows;
using System.Windows.Controls;
using VendingMachineViewModel;

namespace VendingMachine.WPF
{
    public class ListViewItemStyleSelector: StyleSelector
    {
        public Style CouponItemStyle { get; set; }
        public Style ItemStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            ListView list = ItemsControl.ItemsControlFromItemContainer(container) as ListView;
            int index = list?.ItemContainerGenerator.IndexFromContainer(container) ?? -1;
            ProductEx product = index < 0 ? null : list?.Items[index] as ProductEx;
            return (product?.IsCoupon ?? false) ? CouponItemStyle : ItemStyle;
        }
    }
}
