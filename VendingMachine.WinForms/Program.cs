using System;
using System.Windows.Forms;
using VendingMachineModel;
using VendingMachinePresenter;

namespace VendingMachine.WinForms
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            VendingMachineForm form = new VendingMachineForm();
            IModelFactory model = new ModelFactory(null);
            VendingMachineViewPresenter presenter = new VendingMachineViewPresenter(form, model);
            Application.Run(form);
        }
    }
}
