using System;

namespace VendingMachineDataInjectionInterfaces.DomainMappedObjects
{
    public class Order
    {
        public Guid id;
        public string name;
        public DrinkType type;
        public string color;
        public int quantity;
        public int percent;
    }
}
