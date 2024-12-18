namespace VendingMachineDataInjectionInterfaces
{
    public delegate void VoidEventHandler();
    public delegate void VoidEventHandler<P>(P param);
    public delegate void VoidEventHandler<P1, P2>(P1 param1, P2 param2);
    public delegate R RequestEventHandler<R>();
    public delegate R RequestEventHandler<R, P>(P param);

    public interface IAuthUsers
    {
        bool IsAuthComplete();
        bool IsAdmin();
    }
}
