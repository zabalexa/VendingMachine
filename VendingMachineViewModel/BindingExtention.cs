using System.Windows.Input;

namespace VendingMachineViewModel
{
    public static class BindingExtention
    {
        public static ICommand BindCommandToModel(this BaseCommand command, DrinksViewModel model)
        {
            return command.Subscribe(model);
        }

        public static ICommand BindCommandToModel(this BaseCommand command, CoinsViewModel model)
        {
            return command.Subscribe(model);
        }

        public static DrinksViewModel BindToMainModel(this DrinksViewModel model)
        {
            return VendingMachineViewModel.Current.Subscribe(model);
        }

        public static CoinsViewModel BindToMainModel(this CoinsViewModel model)
        {
            return VendingMachineViewModel.Current.Subscribe(model);
        }

        public static QueueViewModel BindToMainModel(this QueueViewModel model)
        {
            return VendingMachineViewModel.Current.Subscribe(model);
        }
    }
}
