using Lab_3.ViewModel;
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

namespace Lab_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        InvadersViewModel _invadersViewModel;
        public MainWindow()
        {
            InitializeComponent();
            _invadersViewModel = new InvadersViewModel();

        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            _invadersViewModel.KeyDown(e.Key);
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            _invadersViewModel.KeyUp(e.Key);
        }
    }
}
