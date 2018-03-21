using System;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachineDataInjectionInterfaces
{
    public interface IBaseGoodsRepository
    {
        void Reinit(Dictionary<Guid, Product> drinks);
        /// <summary>
        /// Загрузить значения по-умолчанию из шаблона
        /// </summary>
        /// <param name="vmID">ID вендингового аппарата</param>
        /// <returns></returns>
        bool Reset(Guid vmID);
        /// <summary>
        /// Сброс репозитория
        /// </summary>
        void Reset();
        /// <summary>
        /// Остаток напитков
        /// </summary>
        Dictionary<Guid, Product> Drinks { get; }
        /// <summary>
        /// Выбрать напиток для покупки
        /// </summary>
        /// <param name="id">Идентификатор напитка</param>
        void Purchase(Guid id);
    }

    public interface IGoodsRepository : IRepository<Product>, IBaseGoodsRepository, ICloneable
    {
    }

    public abstract class BaseGoodsRepository : IBaseGoodsRepository
    {
        #region IBaseGoodsRepository Support
        protected Dictionary<Guid, Product> _drinks;

        public void Reinit(Dictionary<Guid, Product> drinks)
        {
            _drinks = drinks;
        }

        /// <summary>
        /// Сброс репозитория
        /// </summary>
        public virtual void Reset()
        {
            _drinks?.Clear();
            _drinks = null;
        }

        /// <summary>
        /// Загрузить значения по-умолчанию из шаблона
        /// </summary>
        /// <param name="vmID">ID вендингового аппарата</param>
        /// <returns></returns>
        public virtual bool Reset(Guid vmID)
        {
            return false;
        }

        protected virtual void Initialize(bool selectable = false)
        {
        }

        /// <summary>
        /// Остаток напитков
        /// </summary>
        public virtual Dictionary<Guid, Product> Drinks
        {
            get
            {
                Initialize();
                return _drinks;
            }
        }

        /// <summary>
        /// Выбрать напиток для покупки
        /// </summary>
        /// <param name="id">Идентификатор напитка</param>
        public abstract void Purchase(Guid id);
        #endregion
    }
}
