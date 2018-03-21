using System;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachinePresenter.ViewInterfaces
{
    public interface IFillingCupProgressBarControlView
    {
        /// <summary>
        /// Событие готовности элемента управления
        /// </summary>
        event EventHandler Shown;
        /// <summary>
        /// Событие помещения в очередь выдачи выбранного клиентом напитка
        /// </summary>
        event VoidEventHandler<Guid> ProductGetOut;
        /// <summary>
        /// Событие выдачи клиенту и исключения из очереди напитка
        /// </summary>
        event VoidEventHandler Dequeue;
        /// <summary>
        /// Готов к работе
        /// </summary>
        /// <param name="e">Параметр, передаваемый от родительского элементапо стеку вызова</param>
        void Activate(EventArgs e);
        /// <summary>
        /// Поместить выбранный клиентом напиток в очередь выдачи
        /// </summary>
        /// <param name="id">Идентификатор напитка</param>
        void Purchase(Guid id);
        /// <summary>
        /// Обновить очередь выдачи напитков клиенту
        /// </summary>
        /// <param name="queue">Очередь напитков</param>
        void RefreshQueue(Queue<Product> queue);
    }
}
