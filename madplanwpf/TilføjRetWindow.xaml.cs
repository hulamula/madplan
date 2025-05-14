using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace madplanwpf
{
    /// <summary>
    /// Interaction logic for Tilføj_Ret.xaml
    /// </summary>
    public partial class TilføjRetWindow : Window
    {
        public TilføjRetWindow()
        {
            InitializeComponent();
            RetIngrediensBox.ItemsSource = ingredienser;
        }

        private ObservableCollection<string> ingredienser = new ObservableCollection<string>();

        private void TilføjIngrediens_Click(object sender, RoutedEventArgs e)
        {
            string input = TilføjIngrediensBox.Text.Trim();

            if (!string.IsNullOrEmpty(input))
            {
                ingredienser.Add(input);
                TilføjIngrediensBox.Clear();
            }
        }
    
    private void TilføjIngrediensBox_TrykTast(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string input = TilføjIngrediensBox.Text.Trim();

                if (!string.IsNullOrEmpty(input))
                {
                    ingredienser.Add(input);
                    TilføjIngrediensBox.Clear();
                }
            }
        }

        private void GemRet_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Kommer!");
        }
    }
}


