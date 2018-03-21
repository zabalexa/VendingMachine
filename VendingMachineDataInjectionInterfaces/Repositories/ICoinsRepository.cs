using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachineDataInjectionInterfaces
{
    public enum AccountType
    {
        Unknown,
        Customer,
        VendingMachine
    }

    public interface IBaseCoinsRepository
    {
        IBaseCoinsRepository Current { get; }
        void Reinit(Dictionary<CoinType, Coin> coins);
        /// <summary>
        /// Сброс репозитория
        /// </summary>
        void Reset();
        /// <summary>
        /// Текущий баланс в монетах
        /// </summary>
        Dictionary<CoinType, Coin> Coins { get; }
        /// <summary>
        /// Пополнить баланс в монетах
        /// </summary>
        /// <param name="coinType">Тип монеты</param>
        /// <param name="quantity">Количество монет указанного номинала</param>
        /// <returns>Признак успешной операции</returns>
        bool AddCoins(CoinType coinType, int quantity);
        /// <summary>
        /// Потратить баланс в монетах
        /// </summary>
        /// <param name="coinType">Тип монеты</param>
        /// <param name="quantity">Количество монет указанного номинала</param>
        /// <returns>Признак успешной операции</returns>
        bool SubCoins(CoinType coinType, int quantity);
        /// <summary>
        /// Перевод монет между кошельками
        /// </summary>
        /// <param name="otherPurse">Другой баланс</param>
        /// <param name="coins">Монеты разного номинала</param>
        /// <returns>Признак успешной операции</returns>
        bool Payment(IBaseCoinsRepository otherPurse, Dictionary<CoinType, int> coins);
        /// <summary>
        /// Обратный перевод монет между кошельками
        /// </summary>
        /// <param name="otherPurse">Другой баланс</param>
        /// <param name="coins">Монеты разного номинала</param>
        /// <returns>Признак успешной операции</returns>
        bool PaymentBack(IBaseCoinsRepository otherPurse, Dictionary<CoinType, int> coins);
    }

    public interface ICoinsRepository: IRepository<Coin>, IBaseCoinsRepository, ICloneable
    {
    }

    public interface ICustomerPurseRepository : ICoinsRepository
    {
        /// <summary>
        /// Загрузить значения по-умолчанию из шаблона
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns></returns>
        bool Reset(int id);
        /// <summary>
        /// Зарезервированная на покупки сумма
        /// </summary>
        int PayBalance { get; }
        /// <summary>
        /// Очистить значение зарезервированной суммы
        /// </summary>
        /// <param name="value">Сумма остатка</param>
        void ResetPayBalance(int value = 0);
        /// <summary>
        /// Уменьшить зарезервированную сумму на стоимость покупки
        /// </summary>
        /// <param name="price">Стоимость покупки</param>
        /// <returns>Признак успешной операции</returns>
        bool Shipping(int price);
    }

    public interface IVendingMachineChangeRepository : ICoinsRepository
    {
        /// <summary>
        /// Загрузить значения по-умолчанию из шаблона
        /// </summary>
        /// <param name="vmID">ID вендингового аппарата</param>
        /// <returns></returns>
        bool Reset(Guid vmID);
        /// <summary>
        /// Получить размен монетами
        /// </summary>
        /// <param name="payBalance">Запрашиваемая к размену сумма (уменьшается в процессе до 0, иначе не хватает на балансе монет на оставшуюся сумму)</param>
        /// <returns>Монеты разного номинала</returns>
        Dictionary<CoinType, int> GetChange(ref int payBalance);
    }

    public abstract class BaseCoinsRepository : IBaseCoinsRepository
    {
        #region IBaseCoinsRepository Support
        public IBaseCoinsRepository Current
        {
            get { return this; }
        }

        protected Dictionary<CoinType, Coin> _coins;

        public void Reinit(Dictionary<CoinType, Coin> coins)
        {
            _coins = coins;
        }
        
        /// <summary>
        /// Сброс репозитория
        /// </summary>
        public virtual void Reset()
        {
            _coins?.Clear();
            _coins = null;
        }

        protected virtual void Initialize(bool selectable = false)
        {
        }

        /// <summary>
        /// Текущий баланс в монетах
        /// </summary>
        public virtual Dictionary<CoinType, Coin> Coins
        {
            get
            {
                Initialize();
                return _coins;
            }
        }

        /// <summary>
        /// Пополнить баланс в монетах
        /// </summary>
        /// <param name="coinType">Тип монеты</param>
        /// <param name="quantity">Количество монет указанного номинала</param>
        /// <returns>Признак успешной операции</returns>
        public virtual bool AddCoins(CoinType coinType, int quantity)
        {
            if (coinType == CoinType.unknown)
            {
                return false;
            }
            Coins[coinType].quantity += quantity;
            return true;
        }

        /// <summary>
        /// Потратить баланс в монетах
        /// </summary>
        /// <param name="coinType">Тип монеты</param>
        /// <param name="quantity">Количество монет указанного номинала</param>
        /// <returns>Признак успешной операции</returns>
        public virtual bool SubCoins(CoinType coinType, int quantity)
        {
            if ((coinType == CoinType.unknown) || (Coins[coinType].quantity < quantity))
            {
                return false;
            }
            Coins[coinType].quantity -= quantity;
            return true;
        }

        /// <summary>
        /// Перевести монеты на другой баланс
        /// </summary>
        /// <param name="toPurse">Пополняемый кошелек</param>
        /// <param name="coinType">Тип монеты</param>
        /// <param name="quantity">Количество монет указанного номинала</param>
        /// <returns>Признак успешной операции</returns>
        protected bool AddCoins(IBaseCoinsRepository toPurse, CoinType coinType, int quantity)
        {
            if (SubCoins(coinType, quantity))
            {
                return toPurse.AddCoins(coinType, quantity);
            }
            return false;
        }

        /// <summary>
        /// Получить монеты из другого баланса
        /// </summary>
        /// <param name="fromPurse">Опустошаемый кошелек</param>
        /// <param name="coinType">Тип монеты</param>
        /// <param name="quantity">Количество монет указанного номинала</param>
        /// <returns>Признак успешной операции</returns>
        protected bool SubCoins(IBaseCoinsRepository fromPurse, CoinType coinType, int quantity)
        {
            if (fromPurse.SubCoins(coinType, quantity))
            {
                return AddCoins(coinType, quantity);
            }
            return false;
        }

        /// <summary>
        /// Перевод монет между кошельками
        /// </summary>
        /// <param name="otherPurse">Другой баланс</param>
        /// <param name="coins">Монеты разного номинала</param>
        /// <returns>Признак успешной операции</returns>
        public bool Payment(IBaseCoinsRepository otherPurse, Dictionary<CoinType, int> coins)
        {
            bool overDraftIsNotNeed = true;
            List<CoinType> keys = coins.Keys.ToList();
            foreach (CoinType key in keys)
            {
                if (key == CoinType.unknown)
                {
                    continue;
                }
                if (Coins[key].quantity < coins[key])
                {
                    coins[key] -= Coins[key].quantity;
                    AddCoins(otherPurse, key, Coins[key].quantity);
                    overDraftIsNotNeed = false;
                }
                else
                {
                    AddCoins(otherPurse, key, coins[key]);
                    coins[key] = 0;
                }
            }
            return overDraftIsNotNeed;
        }

        /// <summary>
        /// Обратный перевод монет между кошельками
        /// </summary>
        /// <param name="otherPurse">Другой баланс</param>
        /// <param name="coins">Монеты разного номинала</param>
        /// <returns>Признак успешной операции</returns>
        public bool PaymentBack(IBaseCoinsRepository otherPurse, Dictionary<CoinType, int> coins)
        {
            bool overDraftIsNotNeed = true;
            List<CoinType> keys = coins.Keys.ToList();
            foreach (CoinType key in keys)
            {
                if (key == CoinType.unknown)
                {
                    continue;
                }
                int farCoinQuantity = otherPurse.Coins[key].quantity;
                if (farCoinQuantity < coins[key])
                {
                    coins[key] -= farCoinQuantity;
                    SubCoins(otherPurse, key, farCoinQuantity);
                    overDraftIsNotNeed = false;
                }
                else
                {
                    SubCoins(otherPurse, key, coins[key]);
                    coins[key] = 0;
                }
            }
            return overDraftIsNotNeed;
        }
        #endregion
    }
}
