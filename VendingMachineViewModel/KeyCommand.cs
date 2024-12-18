using System;
using System.Windows;
using System.Windows.Input;

namespace VendingMachineViewModel
{
    public class FocusCommand: ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return parameter != null;
        }

        public void Execute(object parameter)
        {
            (parameter as UIElement)?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}
