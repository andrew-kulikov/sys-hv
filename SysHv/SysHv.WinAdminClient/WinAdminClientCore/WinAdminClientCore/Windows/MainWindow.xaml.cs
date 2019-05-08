using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinAdminClientCore.ViewModels;
using WinAdminClientCore.Views;

namespace WinAdminClientCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SensorsStats _sensorsStats = new SensorsStats();
        private RegisterClient _registerService = new RegisterClient();
        private SensorsInfo _sensorsInfo = new SensorsInfo();

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainWindowViewModel viewModel)
        {
            DataContext = viewModel;

            _sensorsStats.ItemsControl.ItemsSource = viewModel.Computers;
            _sensorsInfo.ItemsControl.ItemsSource = viewModel.Computers;

            InitializeComponent();
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                case 0:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(_sensorsInfo);
                    break;
                case 1:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(_sensorsStats);
                    break;

                case 2:
                    break;

                case 3:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(_registerService);
                    break;
            }
        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
        }
    }
}
