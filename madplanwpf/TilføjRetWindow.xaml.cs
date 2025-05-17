using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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
        private Ret? retTilRedigering = null;
        private List<Ret> retter;
        private string filsti;
        private ObservableCollection<string> ingredienser = new ObservableCollection<string>();

        public TilføjRetWindow(List<Ret> retter, string filsti)
        {
            InitializeComponent();
            this.retter = retter;
            this.filsti = filsti;
            RetIngrediensBox.ItemsSource = ingredienser;
        }

        public TilføjRetWindow(List<Ret> retter, string filsti, Ret retTilRedigering)
        {
            InitializeComponent();
            this.retter = retter;
            this.filsti = filsti;
            this.retTilRedigering = retTilRedigering;
            RetNavnBox.Text = retTilRedigering.Navn;
            ingredienser = new ObservableCollection<string>(retTilRedigering.Ingredienser);
            RetIngrediensBox.ItemsSource= ingredienser;
        }


        //tillad luk vindue via escape
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            base.OnPreviewKeyDown(e);
        }

        private void TilføjIngrediens_Click(object sender, RoutedEventArgs e)
        {
            string input = TilføjIngrediensBox.Text.Trim();

            if (!string.IsNullOrEmpty(input))
            {
                ingredienser.Add(input);
                TilføjIngrediensBox.Clear();
            }
        }

        private void SletIngrediens_Click(object sender, RoutedEventArgs e)
        {
            Button sletIngrediensKnap = (Button)sender;
            string ingrediensSlettes = (string)sletIngrediensKnap.Tag;
            ingredienser.Remove(ingrediensSlettes);
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

        private void Ingrediens_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            string nyIngrediens = box.Text;
            string gammelIngrediens = (string)box.Tag;
            if (!string.IsNullOrWhiteSpace(nyIngrediens) && nyIngrediens != gammelIngrediens)
            {
                int index = ingredienser.IndexOf(gammelIngrediens);
                if (index != -1)
                {
                    ingredienser[index] = nyIngrediens;
                }
            }
        }

        private void GemRet_Click(object sender, RoutedEventArgs e)
        {
            string navn = RetNavnBox.Text.Trim();
            if (string.IsNullOrEmpty(navn))
            {
                MessageBox.Show("Indtast navn på ret");
                return;
            }

            List<string> ingrediensListe = ingredienser.ToList();
            if (!ingrediensListe.Any())
            {
                MessageBox.Show("Indtast mindst én ingrediens");
                return;
            }       


            if (retTilRedigering == null)
            {
                if (retter.Any(r => r.Navn.Equals(navn, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Der findes allerede en ret med det navn");
                    return;
                }

                    Ret nyRet = new Ret(navn, ingrediensListe);
                    retter.Add(nyRet);
                    RetFiler.ValgtRetFilNavn = filsti;
                    RetFiler.GemNyRet(nyRet);
                    this.Close();
                
            }
            else

            {
                if (retter.Any(r => r != retTilRedigering && r.Navn.Equals(navn, StringComparison.OrdinalIgnoreCase)))

                    { 
                        MessageBox.Show("Der findes en anden ret med det navn");
                        return;
                    }
                    
                    retTilRedigering.Navn = RetNavnBox.Text.Trim();
                    retTilRedigering.Ingredienser = ingrediensListe;
                    RetFiler.GemRetter(retter, filsti);
                    this.Close();
                    
                
            }
            MessageBox.Show("Ret tilføjet!");
            
            
        }
    }
}


