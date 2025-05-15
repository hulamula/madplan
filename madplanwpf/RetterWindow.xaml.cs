using System.Windows;
using madplanwpf.Models;

namespace madplanwpf
{
    /// <summary>
    /// Interaction logic for RetterWindow.xaml
    /// </summary>
    public partial class RetterWindow : Window
    {

        private List<Ret> retter;
        private string filsti;
        public RetterWindow(List<Ret> retter, string filsti)
        {
            InitializeComponent();
            RetterListView.ItemsSource = retter;
            this.retter = retter;
            this.filsti = filsti;
        }
        private void TilføjRet_Click(object sender, RoutedEventArgs e)
        {
            TilføjRetWindow vindue = new TilføjRetWindow(retter, filsti);
            vindue.ShowDialog();
            RetterListView.ItemsSource = null;
            RetterListView.ItemsSource = retter;
        }
    }
}
