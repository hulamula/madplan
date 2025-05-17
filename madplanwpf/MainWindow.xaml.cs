using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using madplanwpf.Models;
using madplanwpf.Services;
using madplanwpf.Utilities;



namespace madplanwpf
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        //declare liste ValgteRetter - udfyldes via combobox, overføres til nyt vindue
        private List<Ret> ValgteRetter;

        //declare string filSti - udfyldes via combobox, overføres til nyt vindue
        private string filSti;

        //declare list<String> indkøbsListe - udfyldes ifbm. generérplan, overføres til nyt vindue
        private List<string> indkøbsListe = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            VisIndkøbslisteKnap.IsEnabled = false;
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

        /// <summary>
        /// når indhold vælges i combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilvalgComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //hvis der er valgt et comboboxitem så cast til valgtFil
            if (FilvalgComboBox.SelectedItem is ComboBoxItem valgtFil)
            {
                //filnavn er Tag (som string)
                string filnavn = valgtFil.Tag.ToString();
                //filstil er combination af .exe-filens placering + filnavn
                filSti = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filnavn);


                //hvis der findes en fil på stien
                if (File.Exists(filSti))
                {
                    try
                    {
                        RetFiler.ValgtRetFilNavn = filSti;
                        ValgteRetter = RetFiler.HentRetter();
                    }
                    //hvis ikke muligt at læse indhold eller ikke muligt at deserialize giv fejlmeddelelse
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fejl i indlæsning: {ex.Message}");
                    }
                }
                //hvis ingen fil på sti giv fejlmeddelelse
                else
                {
                    MessageBox.Show("Fil ikke fundet");
                }

            }
        }


        private void RedigerRetterKnap_Click(object sender, RoutedEventArgs e)
        {
            //åbn RetterWindow
            RetterWindow vindue = new RetterWindow(ValgteRetter, filSti);
            vindue.Show();
        }
        private void GenererMadplanKnap_Click(object sender, RoutedEventArgs e)
        {
            if (ValgteRetter == null || ValgteRetter.Count == 0)
            {
                MessageBox.Show("Vælg en liste med retter");
                return;
            }
            //generer madplan via metode fra PlanGen.cs
            Dictionary<DayOfWeek, Ret> nyMadplan = PlanGen.LavPlan(ValgteRetter);

            //tilføj retter fra ugePlan til liste
            List<Ret> planlagteRetter = nyMadplan.Values.ToList();
            //opret indkøbsliste list
            Indkøbsliste genereretIndkøbsListe = new Indkøbsliste();
            //fyld indkøbsliste med ingredienser fra planlagteretter
            genereretIndkøbsListe.TilføjIngredienserFraMadplan(planlagteRetter);
            //udfyld private field IndkøbsListe med denne liste
            indkøbsListe = genereretIndkøbsListe.Varer;


            //udfyld listbox med madplan 
            nyMadplanListbox.Items.Clear();
            foreach (var dagRet in nyMadplan)
            {
                string danskUgedag = DanskeUgedage.Navne[dagRet.Key];
                string ret = dagRet.Value.Navn;
                nyMadplanListbox.Items.Add($"{danskUgedag}:     {ret}");
            }
            VisIndkøbslisteKnap.IsEnabled = true;

        }

        //når man klikker på "Vis indkøbsliste knappen"
        private void VisIndkøbslisteKnap_click (object sender, RoutedEventArgs e)

            //åbn vindue IndkøbslisteWindow
            {
            IndkøbsListeWindow vindue = new IndkøbsListeWindow(indkøbsListe);
            vindue.ShowDialog();
            }

    }
}