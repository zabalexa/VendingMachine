using System;

namespace VendingMachineViewModel
{
    /// <summary>
    /// Бизнес-логика покупки напитка
    /// </summary>
    public class PurchaseCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            Guid pid;
            return Guid.TryParse(parameter.ToString(), out pid) && GetDrinksQty(pid) > 0;
        }

        public override void Execute(object parameter)
        {
            Guid pid;
            if (Guid.TryParse(parameter.ToString(), out pid))
            {
                if (Shipping(GetDrinksPrice(pid)))
                {
                    Purchase(pid);
                    ThanksMessage();
                    if (GetDrinksQty(pid) == 0)
                    {
                        DrinksButtonHide(pid);
                    }
                }
                else
                {
                    InsufficientMessage();
                }
            }
        }
    }
}