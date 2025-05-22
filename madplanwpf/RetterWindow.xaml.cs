using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using madplanwpf.Models;
using madplanwpf.Services;

namespace madplanwpf
{
    /// <summary>
    /// Interaction logic for RetterWindow.xaml
    /// Vindue til visning og redigering af indlæste retter
    /// </summary>
    public partial class RetterWindow : Window
    {
        //lokale variable til brug i denne fil
        private List<Ret> retter;
        private string filsti;

        //konstruktør åbnes med retliste og filsti fra mainwindow, overføres til lokale variable
        public RetterWindow(List<Ret> retter, string filsti)
        {
            InitializeComponent();
            RedigerRetterListbox.ItemsSource = retter;
            this.retter = retter;
            this.filsti = filsti;
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

        //metode til nulstil og genindlæs liste med retter
        private void OpdaterRetliste()
        {
            RedigerRetterListbox.ItemsSource = null;
            RedigerRetterListbox.ItemsSource = retter;
        }
     
        //håndtering af klik på "Tilføj ret" knap, overfører liste med retter og filsti
        private void TilføjRet_Click(object sender, RoutedEventArgs e)
        {
            TilføjRetWindow vindue = new TilføjRetWindow(retter, filsti);
            vindue.ShowDialog();
            OpdaterRetliste();
        }

        //håndtering af klik på "Slet"-knap til sletning af ret
        private void RetterListSlet_Click(object sender, RoutedEventArgs e)
        {
            Button sletKnapRet = (Button)sender;
            Ret retSlettes = (Ret)sletKnapRet.Tag;
            retter.Remove(retSlettes);
            OpdaterRetliste();
        }

        //håndtering af klik på "Redigér ret"-knap
        private void RetterListRediger_Click(object sender, RoutedEventArgs e)
        {
            Button redigerRetKnap = (Button)sender;
            Ret retTilRedigering = (Ret)redigerRetKnap.Tag;
            Window vindue = new TilføjRetWindow(retter, filsti, retTilRedigering);
            vindue.ShowDialog();
            OpdaterRetliste();
        }

        //håndtering af klik på "Gem" knap opdaterer retfil og lukker vindue
        private void RetterWindowGem_Click(object sender, RoutedEventArgs e)
        {
            RetFiler.GemRetter(retter, filsti);
            this.Close();
        }

    }


}

