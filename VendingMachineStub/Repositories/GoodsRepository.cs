using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachineDataInjectionInterfaces;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachineStub.Repositories
{
    public class GoodsRepository : BaseGoodsRepository, IGoodsRepository
    {
        #region IBaseGoodsRepository Support
        public override void Purchase(Guid id)
        {
            if (--_drinks[id].quantity <= 0)
            {
                _drinks.Remove(id);
            }
        }
        #endregion

        #region IGoodsRepository Support
        #region IRepository Support
        public bool Create(Product item)
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

        public Product Get(Guid id)
        {
            return (_drinks?.ContainsKey(id) ?? false) ? _drinks[id] : null;
        }

        public Product Get(string id)
        {
            Guid g = Guid.Empty;
            return Guid.TryParseExact(id, "N", out g) && (_drinks?.ContainsKey(g) ?? false) ? _drinks[g] : null;
        }

        public Product Get(int id)
        {
            return id > (_drinks?.Count ?? 0) ? null : _drinks?.Values.Skip(id - 1).First();
        }

        public IEnumerable<Product> GetAll()
        {
            if (_drinks.Any())
            {
                foreach (KeyValuePair<Guid, Product> d in _drinks)
                {
                    yield return d.Value;
                }
            }
        }

        public bool Update(Product item)
        {
            return false;
        }
        #endregion
        #region ICloneable Support
        public object Clone()
        {
            return new GoodsRepository() { _drinks = _drinks == null ? new Dictionary<Guid, Product>() : _drinks.ToDictionary(x => x.Key, y => y.Value.Clone() as Product) };
        }
        #endregion
        #endregion
    }
}
