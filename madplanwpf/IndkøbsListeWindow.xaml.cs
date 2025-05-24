using System.Globalization;
using System.Windows;
using System.Windows.Input;
using madplanwpf.Services;
using Microsoft.Win32;

namespace madplanwpf
{
    /// <summary>
    /// Interaction logic for IndkøbsListeWindow.xaml
    /// </summary>
    public partial class IndkøbsListeWindow : Window
    {
        private Indkøbsliste _indkøbsliste;
        public IndkøbsListeWindow(Indkøbsliste indkøbsliste)
        {
            InitializeComponent();
            _indkøbsliste = indkøbsliste;
            IndkøbslisteListbox.ItemsSource= indkøbsliste.Varer;
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

        private void GemIndkøbslisteKnap_click (object sender, RoutedEventArgs e)
        {
            int ugeNummer = ISOWeek.GetWeekOfYear(DateTime.Now);
            string filNavn = $"Indkøbsliste uge {ugeNummer}.txt";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = filNavn;
            if (saveFileDialog.ShowDialog() == true)
            {
                string filSti = saveFileDialog.FileName;
                _indkøbsliste.GemIndkøbsliste(filSti);
                MessageBox.Show($"Gemt til {filSti}");
            }

        }

    }
}
