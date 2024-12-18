namespace VendingMachineViewModel
{
    /// <summary>
    /// Бизнес-логика размена монет
    /// </summary>
    public class ChangeCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            return GetPayBalance() > 0;
        }

        public override void Execute(object parameter)
        {
            if (!PaymentBack())
            {
                InsufficientMessage();
            }
        }
    }
}
