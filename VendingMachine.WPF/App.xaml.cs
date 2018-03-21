using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace VendingMachine.WPF
{
    public delegate void VoidEventHandler();
    public delegate void VoidEventHandler<P>(P param);
    public delegate R RequsetEventHandler<R>();
    public delegate R RequsetEventHandler<R, P>(P param);

    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
    }
}
