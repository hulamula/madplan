using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using madplanwpf;
using madplanwpf.Services;

namespace madplanwpf.ViewModels
{
   public class RelayCommand : ICommand
    {
        private readonly Action _execute;

        public RelayCommand(Action execute)
        {
            _execute = execute;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }

        
    }


    public class TilføjRetViewModel
    {
        public ICommand TilføjRetCommand { get; }

        public ICommand TilføjIngrediensCommand { get; }

        public ObservableCollection<String> TilføjIngredienser { get; set; }

        public TilføjRetViewModel()
        {
            TilføjRetCommand = new RelayCommand(TilføjRet);

            TilføjIngrediensCommand = new RelayCommand(TilføjIngrediens);

            TilføjIngredienser = new ObservableCollection<String>();

        }

        private void TilføjRet()
        {
                //opret Tilføj Ret-vindue
                var TilføjRetVindue = new madplanwpf.Tilføj_Ret();

            //åbn vindue som dialog
           TilføjRetVindue.ShowDialog();
        }

        public string IngrediensInput { get; set; }

        private void TilføjIngrediens()
        {
            //hvis der står noget i feltet ved klik  
            if (!string.IsNullOrEmpty(IngrediensInput))
            {
                TilføjIngredienser.Add(IngrediensInput); //tilføj til observable collection
                IngrediensInput = string.Empty; //tøm felt efter input
            }
            
        }


    }
}

