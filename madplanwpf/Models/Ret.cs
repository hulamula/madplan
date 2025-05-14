using System;
using System.Collections.Generic;

namespace madplanwpf.Models
{
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
