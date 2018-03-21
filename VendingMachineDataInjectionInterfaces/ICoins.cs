using System;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachineDataInjectionInterfaces
{
    /// <summary>
    /// Тип монет
    /// </summary>
    public enum CoinType
    {
        /// <summary>
        /// Монета неизвестного номинала
        /// </summary>
        unknown = 0,
        /// <summary>
        /// Номинал моенты - 1 рубль
        /// </summary>
        coin1 = 1,
        /// <summary>
        /// Номинал моенты - 2 рубля
        /// </summary>
        coin2 = 2,
        /// <summary>
        /// Номинал моенты - 5 рублей
        /// </summary>
        coin5 = 5,
        /// <summary>
        /// Номинал моенты - 10 рублей
        /// </summary>
        coin10 = 10
    }

    public interface ICoins<T> where T : class
    {
        IBaseCoinsRepository Current { get; }
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

    public interface ICustomerPurse : ICoins<ICustomerPurseRepository>, IRepo<ICustomerPurseRepository>
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

    public interface IVendingMachineChange : ICoins<IVendingMachineChangeRepository>, IRepo<IVendingMachineChangeRepository>
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
}
