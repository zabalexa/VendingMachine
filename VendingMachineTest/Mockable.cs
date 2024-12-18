using System.Linq;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;
using VendingMachineModel;

namespace VendingMachineTest
{
    internal class MockCustomerPurseRepository
    {
        private readonly ICustomerPurse _proxy;

        internal MockCustomerPurseRepository(int quantity)
        {
            (_proxy = new ModelFactory("").CustomerPurseProxy).Repo.Reinit(Dummies.DummyInitCoins(quantity).ToDictionary(x => x.Key, y => new Coin() { type = y.Key, quantity = y.Value }));
        }

        public ICustomerPurse Proxy { get { return _proxy; }}
    }

    internal class MockVendingMachineChangeRepository
    {
        private readonly IVendingMachineChange _proxy;

        internal MockVendingMachineChangeRepository(int quantity)
        {
            (_proxy = new ModelFactory("").VendingMachineChangeProxy).Repo.Reinit(Dummies.DummyInitCoins(quantity).ToDictionary(x => x.Key, y => new Coin() { type = y.Key, quantity = y.Value }));
        }

        public IVendingMachineChange Proxy { get { return _proxy; } }
    }

    internal class MockGoodsRepository
    {
        private readonly IGoods _proxy;

        internal MockGoodsRepository(int quantity, int price)
        {
            (_proxy = new ModelFactory("").GoodsProxy).Repo.Reinit(Dummies.DummyInitGoods(quantity, price));
        }

        public IGoods Proxy { get { return _proxy; } }
    }

    internal interface IMockRepositoryFactory { }
    internal class MockRepositoryFactory : Mockable<IModelFactory, ModelFactory>, IMockRepositoryFactory { }
    
    internal class Mockable<I, T> where I : class where T : I, new()
    {
        private static I _mock;

        internal static void SetCurrent(I mock)
        {
            _mock = mock;
        }

        internal static I Current { get { return _mock ?? new T(); }}
    }
}
