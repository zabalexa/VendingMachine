using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;
using VendingMachineStub.Repositories;

namespace VendingMachineStub
{
    public class VendingMachineStub : ICustomerPurse, IVendingMachineChange, IGoods, ICloneable
    {
        private ICustomerPurseRepository _customerPurseRepository;
        private IVendingMachineChangeRepository _vendingMachineChangeRepository;
        private IGoodsRepository _goodsRepository;

        public VendingMachineStub()
        {
        }

        public VendingMachineStub(IBaseRepositoryFactory repo)
        {
            _customerPurseRepository = repo.CreateCustomerPurseRepository();
            _vendingMachineChangeRepository = repo.CreateVendingMachineChangeRepository();
            _goodsRepository = repo.CreateGoodsRepository();
        }

        #region ICustomerPurse Support
        public bool Reset(int id)
        {
            return _customerPurseRepository?.Reset(id) ?? false;
        }

        public int PayBalance
        {
            get { return _customerPurseRepository?.PayBalance ?? 0; }
        }

        public bool Shipping(int price)
        {
            return _customerPurseRepository?.Shipping(price) ?? false;
        }

        public void ResetPayBalance(int value = 0)
        {
            _customerPurseRepository?.ResetPayBalance(value);
        }

        #region ICoins Support
        IBaseCoinsRepository ICoins<ICustomerPurseRepository>.Current
        {
            get { return _customerPurseRepository; }
        }

        void ICoins<ICustomerPurseRepository>.Reset()
        {
            _customerPurseRepository.Reset();
        }

        Dictionary<CoinType, Coin> ICoins<ICustomerPurseRepository>.Coins
        {
            get { return _customerPurseRepository.Coins; }
        }

        bool ICoins<ICustomerPurseRepository>.AddCoins(CoinType coinType, int quantity)
        {
            return _customerPurseRepository.AddCoins(coinType, quantity);
        }

        bool ICoins<ICustomerPurseRepository>.SubCoins(CoinType coinType, int quantity)
        {
            return _customerPurseRepository.SubCoins(coinType, quantity);
        }

        bool ICoins<ICustomerPurseRepository>.Payment(IBaseCoinsRepository otherPurse, Dictionary<CoinType, int> coins)
        {
            return _customerPurseRepository.Payment(otherPurse, coins);
        }

        bool ICoins<ICustomerPurseRepository>.PaymentBack(IBaseCoinsRepository otherPurse, Dictionary<CoinType, int> coins)
        {
            return _customerPurseRepository.PaymentBack(otherPurse, coins);
        }
        #endregion
        #region IRepo Support
        ICustomerPurseRepository IRepo<ICustomerPurseRepository>.Repo
        {
            get { return _customerPurseRepository; }
        }
        #endregion
        #endregion

        #region IVendingMachineChange Support
        bool IVendingMachineChange.Reset(Guid vmID)
        {
            return _vendingMachineChangeRepository?.Reset(vmID) ?? false;
        }

        public Dictionary<CoinType, int> GetChange(ref int payBalance)
        {
            return _vendingMachineChangeRepository?.GetChange(ref payBalance);
        }

        #region ICoins Support
        IBaseCoinsRepository ICoins<IVendingMachineChangeRepository>.Current
        {
            get { return _vendingMachineChangeRepository; }
        }

        void ICoins<IVendingMachineChangeRepository>.Reset()
        {
            _vendingMachineChangeRepository.Reset();
        }

        Dictionary<CoinType, Coin> ICoins<IVendingMachineChangeRepository>.Coins
        {
            get { return _vendingMachineChangeRepository.Coins; }
        }

        bool ICoins<IVendingMachineChangeRepository>.AddCoins(CoinType coinType, int quantity)
        {
            return _vendingMachineChangeRepository.AddCoins(coinType, quantity);
        }

        bool ICoins<IVendingMachineChangeRepository>.SubCoins(CoinType coinType, int quantity)
        {
            return _vendingMachineChangeRepository.SubCoins(coinType, quantity);
        }

        bool ICoins<IVendingMachineChangeRepository>.Payment(IBaseCoinsRepository otherPurse, Dictionary<CoinType, int> coins)
        {
            return _vendingMachineChangeRepository.Payment(otherPurse, coins);
        }

        bool ICoins<IVendingMachineChangeRepository>.PaymentBack(IBaseCoinsRepository otherPurse, Dictionary<CoinType, int> coins)
        {
            return _vendingMachineChangeRepository.PaymentBack(otherPurse, coins);
        }
        #endregion
        #region IRepo Support
        IVendingMachineChangeRepository IRepo<IVendingMachineChangeRepository>.Repo
        {
            get { return _vendingMachineChangeRepository; }
        }
        #endregion
        #endregion

        #region IGoods Support
        bool IGoods.Reset(Guid vmID)
        {
            return _goodsRepository?.Reset(vmID) ?? false;
        }

        public void Reset()
        {
            _goodsRepository?.Reset();
        }

        public Dictionary<Guid, Product> Drinks
        {
            get { return _goodsRepository?.GetAll().ToDictionary(x => x.id, y => y.Clone() as Product); }
        }

        public Product GetProduct(Guid id)
        {
            return _goodsRepository?.Get(id);
        }

        public void Purchase(Guid id)
        {
            _goodsRepository?.Purchase(id);
        }

        #region IRepo Support
        IGoodsRepository IRepo<IGoodsRepository>.Repo
        {
            get { return _goodsRepository; }
        }
        #endregion
        #endregion

        #region ICloneable Support
        public object Clone()
        {
            return new VendingMachineStub() { _customerPurseRepository = _customerPurseRepository.Clone() as ICustomerPurseRepository, _vendingMachineChangeRepository = _vendingMachineChangeRepository.Clone() as IVendingMachineChangeRepository, _goodsRepository = _goodsRepository.Clone() as IGoodsRepository };
        }
        #endregion
    }

    public interface IStubConfigurator
    {
        VendingMachineStub CreateStub();
    }

    public class StubConfigurator : IStubConfigurator
    {
        public StubConfigurator()
        {
        }

        #region IStubConfigurator Support
        private static IStubConfigurator _mock;

        public static IStubConfigurator Current
        {
            get
            {
                return _mock ?? (_mock = new StubConfigurator());
            }
            set
            {
                _mock = value;
            }
        }

        public VendingMachineStub CreateStub()
        {
            return new VendingMachineStub(new RepositoryFactory(true));
        }
        #endregion
    }
}
