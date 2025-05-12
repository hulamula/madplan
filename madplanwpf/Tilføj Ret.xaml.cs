using System;
using System.Collections.Generic;
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
using madplanwpf.ViewModels;

namespace madplanwpf
{
    /// <summary>
    /// Interaction logic for Tilføj_Ret.xaml
    /// </summary>
    public partial class Tilføj_Ret : Window
    {
        public Tilføj_Ret()
        {
            InitializeComponent();
        }
    


    private void TilføjIngrediensBox_TrykTast(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((TilføjRetViewModel)DataContext).TilføjIngrediensCommand.Execute(null);
            }
        }
    }

}


