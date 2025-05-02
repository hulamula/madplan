using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace madplan.Models
{
        public class Ret
        {
            //static field til at holde styr på antal retter
            private static int antal = 0;

            //public metode til at sætte antal retter
            public static void SætAntal(int Antal)
            {
                antal = Antal;
            }

            //properties
            public string Navn { get; set; }
            public List<string> Ingredienser { get; set; }
            public int Nummer { get; set; }

            //konstruktør
            public Ret(string navn, List<string> ingredienser)
            {
                Navn = navn;
                Ingredienser = ingredienser;
                Nummer = ++antal;
            }

            //override af ToString() så rettens navn printes ved ToString()
            public override string ToString()
            {
                return Navn;
            }

        }
    
}
