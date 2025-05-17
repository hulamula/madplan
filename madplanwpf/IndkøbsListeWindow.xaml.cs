using System;
using System.Collections.Generic;
using System.Globalization;
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
using madplanwpf.Services;
using Microsoft.Win32;

namespace madplanwpf
{
    /// <summary>
    /// Interaction logic for IndkøbsListeWindow.xaml
    /// </summary>
    public partial class IndkøbsListeWindow : Window
    {
        private List<string> varer;
        public IndkøbsListeWindow(List<string> varer)
        {
            InitializeComponent();
            this.varer = varer;
            IndkøbslisteListbox.ItemsSource= varer;
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
                Indkøbsliste.GemIndkøbsliste(varer, filSti);
                MessageBox.Show($"Gemt til {filSti}");
            }

        }

    }
}
