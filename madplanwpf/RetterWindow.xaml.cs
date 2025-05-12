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
using System.Windows.Shapes;
using madplanwpf.Models;
using madplanwpf.ViewModels;

namespace madplanwpf
{
    /// <summary>
    /// Interaction logic for RetterWindow.xaml
    /// </summary>
    public partial class RetterWindow : Window
    {

        public RetterWindow(List<Ret> retter)
        {
            InitializeComponent();
            RetterListView.ItemsSource = retter;
            DataContext = new TilføjRetViewModel();
        }
        private void TilføjRet_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Kommer!");
        }
    }
}
