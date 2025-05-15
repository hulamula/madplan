using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using madplanwpf.Models;
using madplanwpf.Services;

namespace madplanwpf
{
    /// <summary>
    /// Interaction logic for TilføjRetWindow.xaml
    /// </summary>
    public partial class TilføjRetWindow : Window
    {
        private List<Ret> retter;
        private string filsti;

        public TilføjRetWindow(List<Ret> retter, string filsti)
        {
            InitializeComponent();
            this.retter = retter;
            this.filsti = filsti;
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
            string navn = RetNavnBox.Text.Trim();
            List<string> ingrediensListe = ingredienser.ToList();
            Ret nyRet = new Ret(navn, ingrediensListe);
            retter.Add(nyRet);
            RetFiler.ValgtRetFilNavn = filsti;
            RetFiler.GemRet(nyRet);
            this.Close();
        }
    }
}


