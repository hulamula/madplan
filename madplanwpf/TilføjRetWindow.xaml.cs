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
    /// Vindue til oprettelse eller redigering af enkelt ret
    /// </summary>
    public partial class TilføjRetWindow : Window
    {
        //lokale variabler til brug i filen
        private Ret? retTilRedigering = null;
        private List<Ret> retter;
        private string filsti;
        private ObservableCollection<string> ingredienser = new ObservableCollection<string>();

        //konstruktør af vindue, overfører liste med retter og string med filplacering
        //denne konstruktør bruges ved tilføjelse af ny ret
        public TilføjRetWindow(List<Ret> retter, string filsti)
        {
            InitializeComponent();
            this.retter = retter;
            this.filsti = filsti;
            RetIngrediensBox.ItemsSource = ingredienser;
        }

        //konstruktør af vindue, overfører liste med retter og string med filplacering
        //desuden info om ret-objekt til redigering
        //denne konstruktør bruges ved redigering af eksisterende ret
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

        //håndter klik på knap "Tilføj ingrediens"
        private void TilføjIngrediens_Click(object sender, RoutedEventArgs e)
        {
            string input = TilføjIngrediensBox.Text.Trim();

            if (!string.IsNullOrEmpty(input))
            {
                ingredienser.Add(input);
                TilføjIngrediensBox.Clear();
            }
        }

        //håndter klik på knap "Slet" sletter ingrediens
        private void SletIngrediens_Click(object sender, RoutedEventArgs e)
        {
            Button sletIngrediensKnap = (Button)sender;
            string ingrediensSlettes = (string)sletIngrediensKnap.Tag;
            ingredienser.Remove(ingrediensSlettes);
        }

        //muligt at tilføje indtastet ingrediens ved brug af enter-tast
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


        //håndtering af inline redigering af ingrediens på liste
        //LostFocus betyder at bruger har forladt indtastningsfelt
        private void Ingrediens_LostFocus(object sender, RoutedEventArgs e)
        {
            //declare Textbox og brug indhold fra feltet (sender, skal castes til Textbox)
            TextBox box = (TextBox)sender;
            //gem indtastet tekst
            string nyIngrediens = box.Text;
            //hent tidligere tekst via Tag 
            string gammelIngrediens = (string)box.Tag;
            //tjek at felt ikke er tomt og at ny ingrediens ikke er identisk med gammel
            if (!string.IsNullOrWhiteSpace(nyIngrediens) && nyIngrediens != gammelIngrediens)
            {
                //find gammel ingrediens index
                int index = ingredienser.IndexOf(gammelIngrediens);
                //hvis fundet overskriv gammel ingrediens med ny
                if (index != -1)
                {
                    ingredienser[index] = nyIngrediens;
                }
            }
        }

        //håndtering af klik på "Gem" knap gemmer retter og tilføjer til indlæst retliste og json fil på filsti
        private void GemRet_Click(object sender, RoutedEventArgs e)
        {
            string navn = RetNavnBox.Text.Trim();
            //tjek om navn er indtastet
            if (string.IsNullOrEmpty(navn))
            {
                MessageBox.Show("Indtast navn på ret");
                return;
            }
            //tjek om ingredienser tilføjet
            List<string> ingrediensListe = ingredienser.ToList();
            if (!ingrediensListe.Any())
            {
                MessageBox.Show("Indtast mindst én ingrediens");
                return;
            }       

            //hvis ny ret tilføjes
            if (retTilRedigering == null)
            {
                //tjek at ret ikke allerede findes
                if (retter.Any(r => r.Navn.Equals(navn, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Der findes allerede en ret med det navn");
                    return;
                }
                    
                    //tilføj ret til liste og opdater json-fil
                    Ret nyRet = new Ret(navn, ingrediensListe);
                    retter.Add(nyRet);
                    RetFiler.ValgtRetFilNavn = filsti;
                    RetFiler.GemNyRet(nyRet);
                    this.Close();
                
            }
            else

            //hvis eksisterende ret redigeres
            {
                //tjek om navn er ændret til et navn identisk med anden ret i indlæst liste
                if (retter.Any(r => r != retTilRedigering && r.Navn.Equals(navn, StringComparison.OrdinalIgnoreCase)))

                    { 
                        MessageBox.Show("Der findes en anden ret med det navn");
                        return;
                    }
                    
                    //gem ret til liste og opdater json-fil
                    retTilRedigering.Navn = RetNavnBox.Text.Trim();
                    retTilRedigering.Ingredienser = ingrediensListe;
                    RetFiler.GemRetter(retter, filsti);
                    this.Close();
                    
                
            }
            
            
        }
    }
}


