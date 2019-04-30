using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WinAdminClient.ViewModels;
using WinAdminClient.Windows;

namespace WinAdminClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void Application_Startup(object sender, StartupEventArgs e)
        {
            var window = new LoginWindow();
            window.DataContext = new LoginWindowViewModel(window);

            window.ShowDialog();
        }
    }
}
