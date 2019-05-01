using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WinAdminClientCore.ViewModels;
using WinAdminClientCore.Windows;

namespace WinAdminClientCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void Application_Startup(object sender, StartupEventArgs e)
        {

        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var window = new LoginWindow();
            //window.DataContext = new LoginWindowViewModel(window);
            window.DataContext = new LoginWindowViewModel(window);


            //ConfigurationManager.AppSettings.
            window.ShowDialog();
        }

        public void ConfigureServices()
        {
            MessageBox.Show(ConfigurationManager.AppSettings["asd"]);
        }
    }
}
