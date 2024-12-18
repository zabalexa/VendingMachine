using System.Collections.Generic;
using VendingMachineDataInjectionInterfaces;

namespace VendingMachineViewModel
{
    /// <summary>
    /// Бизнес-логика пополнения монетоприемника вендинговой машины
    /// </summary>
    public class PaymentCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            return (parameter == null ? false : (GetCoinsQty((CoinType)parameter) > 0));
        }

        public override void Execute(object parameter)
        {
            Dictionary<CoinType, int> coins = new Dictionary<CoinType, int>();
            coins.Add((CoinType)parameter, 1);
            Payment(coins);
        }
    }
}
