using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachineStub.Repositories
{
    public class CoinsRepository : BaseCoinsRepository, ICoinsRepository
    {
        #region ICoinsRepository Support
        #region IRepository Support
        public bool Create(Coin item)
        {
            return false;
        }

        public bool Delete(string id, bool all)
        {
            return false;
        }

        public bool Delete(Guid id, bool all)
        {
            return false;
        }

        public bool Delete(int id, bool all)
        {
            return false;
        }

        public Coin Get(Guid id)
        {
            return null;
        }

        public Coin Get(string id)
        {
            return null;
        }

        public Coin Get(int id)
        {
            return (_coins?.ContainsKey((CoinType)id) ?? false) ? _coins[(CoinType)id] : null;
        }

        public IEnumerable<Coin> GetAll()
        {
            return _coins?.Values;
        }

        public bool Update(Coin item)
        {
            return false;
        }
        #endregion
        #region ICloneable Support
        public virtual object Clone()
        {
            return new CoinsRepository() { _coins = _coins == null ? new Dictionary<CoinType, Coin>() : _coins.ToDictionary(x => x.Key, y => y.Value.Clone() as Coin) };
        }
        #endregion
        #endregion
    }

    public class CustomerPurseRepository : CoinsRepository, ICustomerPurseRepository
    {
        private int _payBalance = 0;

        #region ICustomerPurseRepository Support
        public bool Reset(int id)
        {
            return false;
        }

        public int PayBalance
        {
            get { return _payBalance; }
        }

        public void ResetPayBalance(int value)
        {
            _payBalance = value;
        }

        public bool Shipping(int price)
        {
            if (_payBalance < price || price < 0)
            {
                return false;
            }
            _payBalance -= price;
            return true;
        }

        #region IBaseCoinsRepository
        public override bool SubCoins(CoinType coinType, int quantity)
        {
            if ((coinType == CoinType.unknown) || (_coins[coinType].quantity < quantity))
            {
                return false;
            }
            _payBalance += quantity * (int)coinType;
            _coins[coinType].quantity -= quantity;
            return true;
        }
        #endregion
        #endregion

        #region ICloneable Support
        public new object Clone()
        {
            return new CustomerPurseRepository() { _coins = _coins == null ? new Dictionary<CoinType, Coin>() : _coins.ToDictionary(x => x.Key, y => y.Value.Clone() as Coin) };
        }
        #endregion
    }

    public class VendingMachineChangeRepository : CoinsRepository, IVendingMachineChangeRepository
    {
        #region IVendingMachineChangeRepository
        public bool Reset(Guid vmID)
        {
            return false;
        }

        public Dictionary<CoinType, int> GetChange(ref int payBalance)
        {
            Dictionary<CoinType, int> coins = new Dictionary<CoinType, int>();
            foreach (CoinType i in new List<CoinType> { CoinType.coin10, CoinType.coin5, CoinType.coin2, CoinType.coin1 })
            {
                if (_coins[i].quantity > 0)
                {
                    int quantity = payBalance / (int)i;
                    if (quantity > 0)
                    {
                        if (_coins[i].quantity - quantity > 0)
                        {
                            coins.Add(i, quantity);
                            payBalance %= (int)i;
                        }
                        else
                        {
                            coins.Add(i, _coins[i].quantity);
                            payBalance -= coins[i] * (int)i;
                        }
                    }
                }
            }
            return coins;
        }
        #endregion

        #region ICloneable Support
        public new object Clone()
        {
            return new VendingMachineChangeRepository() { _coins = _coins == null ? new Dictionary<CoinType, Coin>() : _coins.ToDictionary(x => x.Key, y => y.Value.Clone() as Coin) };
        }
        #endregion
    }
}
