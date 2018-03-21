using Ninject;
using Ninject.Extensions.Conventions;
using VendingMachineDataInjectionInterfaces;
using System.Linq;
using Ninject.Parameters;
using System;

namespace VendingMachineModel
{
    public interface IModelFactory
    {
        IAuthUsers AuthUsersProxy { get; }
        IGoods GoodsProxy { get; }
        ICustomerPurse CustomerPurseProxy { get; }
        IVendingMachineChange VendingMachineChangeProxy { get; }
    }

    public class ModelFactory : IModelFactory
    {
        private static bool _loaded = false;
        private static IRepositoryFactory _storageRepo;
        private static IBaseRepositoryFactory _initRepo;
        private StrategyHolder _strategyHolder;
        private readonly bool webAPI;

        public ModelFactory()
        {
            webAPI = true;
        }

        public ModelFactory(string path = null)
        {
            webAPI = false;
            if (!_loaded)
            {
                lock("Ninject")
                {
                    if (!_loaded)
                    {
                        _loaded = true;
                        string p = path?.Substring(path.IndexOf("^") + 1) ?? AppDomain.CurrentDomain.BaseDirectory;
                        using (var kernel = new StandardKernel())
                        {
                            kernel.Bind(x => x.FromAssembliesInPath(p, y => y.FullName.StartsWith("VendingMachine")).SelectAllClasses().InheritedFrom<IBaseRepositoryFactory>().BindAllInterfaces().Configure(y => y.When(z => z.Target?.Type != typeof(IRepositoryFactory)).BindingConfiguration.Parameters.Add(new ConstructorArgument("path", p))));
                            _storageRepo = kernel.GetAll<IRepositoryFactory>().FirstOrDefault();
                            _initRepo = kernel.GetAll<IBaseRepositoryFactory>().FirstOrDefault(x => (x as IRepositoryFactory) == null);
                        }
                    }
                }
            }
        }

        private void Initialize()
        {
            if (_strategyHolder == null)
            {
                lock ("Initialize")
                {
                    if (_strategyHolder == null)
                    {
                        _strategyHolder = new StrategyHolder(_storageRepo, _initRepo, webAPI);
                    }
                }
            }
        }

        #region IModelFactory Support
        public IGoods GoodsProxy
        {
            get
            {
                Initialize();
                return _strategyHolder.CurrentSource.GoodsProxy;
            }
        }

        public ICustomerPurse CustomerPurseProxy
        {
            get
            {
                Initialize();
                return _strategyHolder.CurrentSource.CustomerPurseProxy;
            }
        }

        public IVendingMachineChange VendingMachineChangeProxy
        {
            get
            {
                Initialize();
                return _strategyHolder.CurrentSource.VendingMachineChangeProxy;
            }
        }

        public IAuthUsers AuthUsersProxy
        {
            get { return _storageRepo?.AuthUsersProxy; }
        }
        #endregion
    }
}
