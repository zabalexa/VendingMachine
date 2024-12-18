using VendingMachineDataInjectionInterfaces;

namespace VendingMachineStub.Repositories
{
    public class RepositoryFactory : IBaseRepositoryFactory
    {
        private VendingMachineStub _config;

        public RepositoryFactory()
        {
            _config = StubConfigurator.Current.CreateStub();
        }

        public RepositoryFactory(bool init)
        {
        }

        #region IBaseRepositoryFactory Support
        public bool IsStub
        {
            get { return true; }
        }

        public ICustomerPurseRepository CreateCustomerPurseRepository()
        {
            return new CustomerPurseRepository();
        }

        public IGoodsRepository CreateGoodsRepository()
        {
            return new GoodsRepository();
        }

        public IVendingMachineChangeRepository CreateVendingMachineChangeRepository()
        {
            return new VendingMachineChangeRepository();
        }

        public ICustomerPurse CustomerPurseProxy
        {
            get { return _config; }
        }

        public IGoods GoodsProxy
        {
            get { return _config; }
        }

        public IVendingMachineChange VendingMachineChangeProxy
        {
            get { return _config; }
        }

        #region ICloneable Support
        public object Clone()
        {
            return new RepositoryFactory() { _config = _config == null ? StubConfigurator.Current.CreateStub() : _config.Clone() as VendingMachineStub };
        }
        #endregion
        #endregion
    }
}
