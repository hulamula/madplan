using System.Windows;
using madplanwpf.Models;

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
        }
        private void TilføjRet_Click(object sender, RoutedEventArgs e)
        {
            TilføjRetWindow vindue = new TilføjRetWindow();
            vindue.ShowDialog();
        }
    }
}
