using System;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachineTest
{
    internal class Dummies
    {
        internal static Dictionary<CoinType, int> DummyInitCoins(int quantity)
        {
            Dictionary<CoinType, int> coins = new Dictionary<CoinType, int>();
            new List<CoinType>() { CoinType.coin1, CoinType.coin2, CoinType.coin5, CoinType.coin10 }.ForEach(x => coins.Add(x, quantity));
            return coins;
        }

        internal static Dictionary<Guid, Product> DummyInitGoods(int quantity, int price)
        {
            Dictionary<Guid, Product> drinks = new Dictionary<Guid, Product>();
            new List<DrinkType>() { DrinkType.Tea, DrinkType.Coffee, DrinkType.Juice }.ForEach(x => drinks.Add(Guid.NewGuid(), new Product() { type = x, quantity = quantity, price = price }));
            return drinks;
        }
    }
}
