using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using madplanwpf.Models;
using madplanwpf.Services;

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

        private void OpdaterRetliste()
        {
            RedigerRetterListbox.ItemsSource = null;
            RedigerRetterListbox.ItemsSource = retter;
        }
     
        private void TilføjRet_Click(object sender, RoutedEventArgs e)
        {
            TilføjRetWindow vindue = new TilføjRetWindow(retter, filsti);
            vindue.ShowDialog();
            OpdaterRetliste();
        }

        private void RetterListSlet_Click(object sender, RoutedEventArgs e)
        {
            Button sletKnapRet = (Button)sender;
            Ret retSlettes = (Ret)sletKnapRet.Tag;
            retter.Remove(retSlettes);
            OpdaterRetliste();
        }

        private void RetterListRediger_Click(object sender, RoutedEventArgs e)
        {
            Button redigerRetKnap = (Button)sender;
            Ret retTilRedigering = (Ret)redigerRetKnap.Tag;
            Window vindue = new TilføjRetWindow(retter, filsti, retTilRedigering);
            vindue.ShowDialog();
            OpdaterRetliste();
        }

        private void RetterWindowGem_Click(object sender, RoutedEventArgs e)
        {
            RetFiler.GemRetter(retter, filsti);
            this.Close();
        }

    }


}

