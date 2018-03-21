using VendingMachineDataInjectionInterfaces;

namespace VendingMachineModel
{
    public interface ISourceSelector
    {
        IGoods GoodsProxy { get; }
        ICustomerPurse CustomerPurseProxy { get; }
        IVendingMachineChange VendingMachineChangeProxy { get; }
    }

    public class StrategyHolder
    {
        private ISourceSelector _source = null;

        public StrategyHolder(IRepositoryFactory storageRepo, IBaseRepositoryFactory initRepo, bool webAPI)
        {
            _source = (storageRepo?.AuthUsersProxy.IsAuthComplete() ?? false) && (webAPI || (!storageRepo?.AuthUsersProxy.IsAdmin() ?? false)) ? new DatabaseSource(storageRepo) : new ConfigSource(initRepo.Clone() as IBaseRepositoryFactory) as ISourceSelector;
        }

        public ISourceSelector CurrentSource
        {
            get { return _source; }
        }
    }

    public class ConfigSource : ISourceSelector
    {
        private readonly IBaseRepositoryFactory _configRepo;

        public ConfigSource(IBaseRepositoryFactory configRepo)
        {
            _configRepo = configRepo;
        }

        #region ISourceSelector Support
        public IGoods GoodsProxy
        {
            get { return _configRepo?.GoodsProxy; }
        }

        public ICustomerPurse CustomerPurseProxy
        {
            get { return _configRepo?.CustomerPurseProxy; }
        }

        public IVendingMachineChange VendingMachineChangeProxy
        {
            get { return _configRepo?.VendingMachineChangeProxy; }
        }
        #endregion
    }

    public class DatabaseSource : ISourceSelector
    {
        private readonly IRepositoryFactory _storageRepo;

        public DatabaseSource(IRepositoryFactory storageRepo)
        {
            _storageRepo = storageRepo;
        }

        #region ISourceSelector Support
        public IGoods GoodsProxy
        {
            get { return _storageRepo?.GoodsProxy; }
        }

        public ICustomerPurse CustomerPurseProxy
        {
            get { return _storageRepo?.CustomerPurseProxy; }
        }

        public IVendingMachineChange VendingMachineChangeProxy
        {
            get { return _storageRepo?.VendingMachineChangeProxy; }
        }
        #endregion
    }
}
