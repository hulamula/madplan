﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.Text.Json;
using madplanwpf.Models;



namespace madplanwpf
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //declare liste ValgteRetter - udfyldes via combobox, overføres til nyt vindue
        private List<Ret> ValgteRetter;

        public MainWindow()
        {
            InitializeComponent();
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
                string filSti = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filnavn);


                //hvis der findes en fil på stien
                if (File.Exists(filSti) )
                {
                    try
                    {
                        //læs indhold af fil og gem til string
                        string jsonIndhold = File.ReadAllText(filSti);
                        //json-deserialize string til List<T> af typen Ret kaldet "ValgteRetter"
                        ValgteRetter = JsonSerializer.Deserialize<List<Ret>>(jsonIndhold);
                        MessageBox.Show($"Indlæst {ValgteRetter.Count} retter");
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
            RetterWindow vindue = new RetterWindow(ValgteRetter);
            vindue.Show();
        }
    }
}