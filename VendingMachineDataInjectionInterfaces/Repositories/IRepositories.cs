using System;
using System.Collections.Generic;

namespace VendingMachineDataInjectionInterfaces
{
    public interface IRepo<T> where T : class
    {
        T Repo { get; }
    }

    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        T Get(Guid id);
        T Get(string id);
        bool Create(T item);
        bool Update(T item);
        bool Delete(int id, bool all = false);
        bool Delete(Guid id, bool all = false);
        bool Delete(string id, bool all = false);
    }

    public interface IBaseRepositoryFactory : ICloneable
    {
        bool IsStub { get; }
        ICustomerPurseRepository CreateCustomerPurseRepository();
        ICustomerPurse CustomerPurseProxy { get; }
        IVendingMachineChangeRepository CreateVendingMachineChangeRepository();
        IVendingMachineChange VendingMachineChangeProxy { get; }
        IGoodsRepository CreateGoodsRepository();
        IGoods GoodsProxy { get; }
    }

    public interface IRepositoryFactory : IBaseRepositoryFactory
    {
        IAuthUsersRepository CreateAuthUsersRepository();
        IAuthUsers AuthUsersProxy { get; }
    }
}
