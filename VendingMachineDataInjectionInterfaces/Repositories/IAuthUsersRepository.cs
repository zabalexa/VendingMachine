using System;
using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces.DomainMappedObjects;

namespace VendingMachineDataInjectionInterfaces
{
    public interface IAuthUsersRepository
    {
        bool IsAuthComplete();
        bool IsAdmin();
    }
}
