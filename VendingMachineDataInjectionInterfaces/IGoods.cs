using System;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachineDataInjectionInterfaces
{
    public enum DrinkType
    {
        None,
        Tea,
        Coffee,
        Juice
    }

    public interface IGoods : IRepo<IGoodsRepository>
    {
        bool Reset(Guid vmID);
        /// <summary>
        /// Сброс репозитория
        /// </summary>
        void Reset();
        /// <summary>
        /// Ассотритмент напитков
        /// </summary>
        Dictionary<Guid, Product> Drinks { get; }
        /// <summary>
        /// Выбрать напиток для покупки
        /// </summary>
        /// <param name="id">Идентификатор напитка</param>
        void Purchase(Guid id);
        /// <summary>
        /// Получить информацию о напитке
        /// </summary>
        /// <param name="id">Идентификатор напитка</param>
        /// <returns></returns>
        Product GetProduct(Guid id);
    }
}
