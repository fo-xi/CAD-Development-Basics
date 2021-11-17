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
using GlassesFrameView.Service;
using GlassesFrameViewModel;

namespace GlassesFrameView
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowVM _mainWindow = new MainWindowVM(new MessageBoxService());

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _mainWindow;
        }
    }
}
