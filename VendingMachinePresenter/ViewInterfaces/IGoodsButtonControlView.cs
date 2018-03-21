using System;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachinePresenter.ViewInterfaces
{
    public interface IGoodsButtonControlView
    {
        /// <summary>
        /// Событие готовности элемента управления
        /// </summary>
        event EventHandler Shown;
        /// <summary>
        /// Событие помещения в очередь выдачи выбранного клиентом напитка
        /// </summary>
        event VoidEventHandler<Guid> Purchase;
        /// <summary>
        /// Событие покупки со списанием средств внесенной суммы
        /// </summary>
        event RequestEventHandler<bool, int> ShippingRequest;
        /// <summary>
        /// Готов к работе
        /// </summary>
        /// <param name="e">Параметр, передаваемый от родительского элементапо стеку вызова</param>
        void Activate(EventArgs e);
        /// <summary>
        /// Обновление ассортимента напитков
        /// </summary>
        /// <param name="drinks">Ассортимент напитков</param>
        void RefreshGoods(Dictionary<Guid, Product> drinks);
        /// <summary>
        /// Списание средств за напиток
        /// </summary>
        /// <param name="price">Стоимость напитка</param>
        /// <returns></returns>
        bool Shipping(int price);
    }
}
