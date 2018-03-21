using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using VendingMachineDataInjectionInterfaces;
using VendingMachineModel;

namespace VendingMachineTest
{
    [TestClass]
    public class VendingMachineTest
    {
        private IModelFactory _factory;

        public VendingMachineTest()
        {
            _factory = MockRepository.GenerateMock<IModelFactory>();
            MockRepositoryFactory.SetCurrent(_factory);
        }

        [TestMethod]
        public void TestPutCoins()//Тестирование опускания монет
        {
            _factory.Stub(x => x.CustomerPurseProxy).Return(new MockCustomerPurseRepository(2).Proxy);
            ICustomerPurseRepository customerBalance = _factory.CustomerPurseProxy.Repo;
            bool AllOperationSuccess = true;
            new List<CoinType>() { CoinType.coin1, CoinType.coin1, CoinType.coin2, CoinType.coin5, CoinType.coin10 }.ForEach(x => { if (!(AllOperationSuccess = customerBalance.SubCoins(x, 1))) return; });
            Assert.IsTrue(AllOperationSuccess);//При опускании монет, монеты были в наличии
            Assert.AreEqual(19, customerBalance.PayBalance);//Сумма внесенного баланса коректна
            Assert.IsFalse(customerBalance.SubCoins(CoinType.coin1, 1));//Невозможность опускания отсутствующих на балансе монет
        }

        [TestMethod]
        public void TestChange()//Тестирование размена
        {
            _factory.Stub(x => x.CustomerPurseProxy).Return(new MockCustomerPurseRepository(1).Proxy);
            ICustomerPurseRepository customerBalance = _factory.CustomerPurseProxy.Repo;
            customerBalance.ResetPayBalance(17);
            _factory.Stub(x => x.VendingMachineChangeProxy).Return(new MockVendingMachineChangeRepository(2).Proxy);
            IVendingMachineChangeRepository vendingMachineBalance = _factory.VendingMachineChangeProxy.Repo;
            int payBalance = customerBalance.PayBalance;
            Dictionary<CoinType, int> coins = vendingMachineBalance.GetChange(ref payBalance);
            Assert.AreEqual(0, payBalance);//Размен выполнен над всей внесенной суммой
            Assert.AreEqual(1, coins.ContainsKey(CoinType.coin10) ? coins[CoinType.coin10] : -1);//Проверка состава размена - есть 1 монета номиналом 10 рублей
            Assert.AreEqual(1, coins.ContainsKey(CoinType.coin5) ? coins[CoinType.coin5] : -1);//Проверка состава размена - есть 1 монета номиналом 5 рублей
            Assert.AreEqual(1, coins.ContainsKey(CoinType.coin2) ? coins[CoinType.coin2] : -1);//Проверка состава размена - есть 1 монета номиналом 2 рубля
            Assert.AreEqual(-1, coins.ContainsKey(CoinType.coin1) ? coins[CoinType.coin1] : -1);//Проверка состава размена - монета номиналом 1 рубль отсутствует
            Assert.IsTrue(customerBalance.PaymentBack(vendingMachineBalance, coins));//Перевод монет между балансами произошел без ошибок (в аппарате монет нужного номинала в достатке)
            coins = customerBalance.GetAll().ToDictionary(x => x.type, y => y.quantity);
            Assert.AreEqual(2, coins.ContainsKey(CoinType.coin10) ? coins[CoinType.coin10] : -1);//Проверка состава размена - есть 2 монеты номиналом 10 рублей
            Assert.AreEqual(2, coins.ContainsKey(CoinType.coin5) ? coins[CoinType.coin5] : -1);//Проверка состава размена - есть 2 монеты номиналом 5 рублей
            Assert.AreEqual(2, coins.ContainsKey(CoinType.coin2) ? coins[CoinType.coin2] : -1);//Проверка состава размена - есть 2 монеты номиналом 2 рубля
            Assert.AreEqual(1, coins.ContainsKey(CoinType.coin1) ? coins[CoinType.coin1] : -1);//Проверка состава размена - есть 1 монета номиналом 1 рубль
            customerBalance.ResetPayBalance(20);
            payBalance = customerBalance.PayBalance;
            coins = vendingMachineBalance.GetChange(ref payBalance);
            Assert.AreEqual(1, payBalance);//При размене на балансе обнаружился недостаток монет
            Assert.AreEqual(1, coins.ContainsKey(CoinType.coin10) ? coins[CoinType.coin10] : -1);//Проверка состава размена - есть 1 монета номиналом 10 рублей
            Assert.AreEqual(1, coins.ContainsKey(CoinType.coin5) ? coins[CoinType.coin5] : -1);//Проверка состава размена - есть 1 монета номиналом 5 рублей
            Assert.AreEqual(1, coins.ContainsKey(CoinType.coin2) ? coins[CoinType.coin2] : -1);//Проверка состава размена - есть 1 монета номиналом 2 рубля
            Assert.AreEqual(2, coins.ContainsKey(CoinType.coin1) ? coins[CoinType.coin1] : -1);//Проверка состава размена - есть 2 монеты номиналом 1 рубль
        }

        [TestMethod]
        public void TestPurchase()//Тестирование покупки
        {
            _factory.Stub(x => x.CustomerPurseProxy).Return(new MockCustomerPurseRepository(1).Proxy);
            ICustomerPurseRepository customerBalance = _factory.CustomerPurseProxy.Repo;
            _factory.Stub(x => x.VendingMachineChangeProxy).Return(new MockVendingMachineChangeRepository(2).Proxy);
            IVendingMachineChangeRepository vendingMachineBalance = _factory.VendingMachineChangeProxy.Repo;
            _factory.Stub(x => x.GoodsProxy).Return(new MockGoodsRepository(1, 7).Proxy);
            IGoodsRepository goods = _factory.GoodsProxy.Repo;
            Assert.IsTrue(customerBalance.Payment(vendingMachineBalance, Dummies.DummyInitCoins(1)));//Перевод монет между балансами произошел без ошибок (в аппарате монет нужного номинала в достатке)
            Assert.IsTrue(customerBalance.Shipping(goods.Drinks.First(x => x.Value.type == DrinkType.Tea).Value.price));//Успешное списание средств с внесенной суммы
            goods.Purchase(goods.Drinks.First(x => x.Value.type == DrinkType.Tea).Key);//Покупка напитка
            Assert.AreEqual(-1, goods.Drinks.Any(x => x.Value.type == DrinkType.Tea) ? goods.Drinks.First(x => x.Value.type == DrinkType.Tea).Value.quantity : -1);//Ассортимен напитков не содержит нужный
            Assert.IsTrue(customerBalance.Shipping(goods.Drinks.First(x => x.Value.type == DrinkType.Coffee).Value.price));//Успешное списание средств с внесенной суммы
            goods.Purchase(goods.Drinks.First(x => x.Value.type == DrinkType.Coffee).Key);//Покупка напитка
            Assert.AreEqual(-1, goods.Drinks.Any(x => x.Value.type == DrinkType.Coffee) ? goods.Drinks.First(x => x.Value.type == DrinkType.Coffee).Value.quantity : -1);//Ассортимен напитков не содержит нужный
            Assert.IsFalse(customerBalance.Shipping(goods.Drinks.First(x => x.Value.type == DrinkType.Juice).Value.price));//Списание средств не удалось из-за недостатка внесенной суммы
            Assert.IsFalse(customerBalance.Payment(vendingMachineBalance, Dummies.DummyInitCoins(1)));//Перевод монет между балансами имеет ошибку (в аппарате нет монет нужного номинала)
            Assert.AreEqual(1, goods.Drinks.Any(x => x.Value.type == DrinkType.Juice) ? goods.Drinks.First(x => x.Value.type == DrinkType.Juice).Value.quantity : -1);//Ассортимен напитков содержит нужный
        }
    }
}
