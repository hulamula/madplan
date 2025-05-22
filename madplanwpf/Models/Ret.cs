using System;
using System.Collections.Generic;

namespace madplanwpf.Models
{
        //en Ret repræsenterer en ret med et navn og en række ingredienser
        public class Ret
        {
        
            //properties
            public string Navn { get; set; }
            public List<string> Ingredienser { get; set; }

            //konstruktør
            public Ret(string navn, List<string> ingredienser)
            {
                Navn = navn;
                Ingredienser = ingredienser;
            }

        
        }
    
}
