using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using madplanwpf.Models;
using System.IO;        // For File, StreamWriter, StreamReader, Path
using System.Text.Json; // For JSON handling, if needed


namespace madplanwpf.Services
{
    public class InputService
    {
        /// <summary>
        /// metode til indhentning af ingrediensliste via indtastning
        /// </summary>
        static List<string> IndtastIngredienser()
        {
            List<string> ingredienser = new List<string>();
            Console.WriteLine("Indtast ingredienser. Afslut ved at indtaste 'færdig'");
            while (true)
            {
                string ingrediens = Console.ReadLine();
                if (ingrediens.Equals("færdig", StringComparison.OrdinalIgnoreCase))
                    break;
                if (!string.IsNullOrEmpty(ingrediens))
                    ingredienser.Add(ingrediens);
                Console.WriteLine("Ingrediens " + ingrediens + " tilføjet");
            }
            return ingredienser;
        }

        /// <summary>
        /// metode til indtastning af ret
        /// </summary>
        static Ret IndtastRet()
        {
            RetFiler.OpdaterRetAntal();
            Console.WriteLine("Indtast navn på ret");
            string navn = Console.ReadLine();
            List<string> ingredienser = new List<string>();
            ingredienser = IndtastIngredienser();
            return new Ret(navn, ingredienser);
        }

        /// <summary>
        /// metode til at tilføje retter til json via loop
        /// </summary>
        
        public static void TilføjRet()
        {
            Console.WriteLine("Vil du tilføje en ret? (ja/nej)");
            string svar = Console.ReadLine().ToLower();
            bool fortsæt = (svar == "ja");

            while (fortsæt)
            {
                Ret nyRet = IndtastRet();
                RetFiler.GemRet(nyRet);

                Console.WriteLine("Vil du tilføje endnu en ret? (ja/nej)");
                svar = Console.ReadLine().ToLower();
                fortsæt = (svar == "ja");
            }
            Console.WriteLine("Færdig med at indtaste retter");
        }
    }
}
