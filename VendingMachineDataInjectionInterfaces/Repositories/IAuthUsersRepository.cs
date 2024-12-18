namespace VendingMachineDataInjectionInterfaces
{
    public interface IAuthUsersRepository
    {
        bool IsAuthComplete();
        bool IsAdmin();
    }
}
