using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace madplan.Services
{
    /// <summary>
    /// Beder bruger om at vælge default- eller brugerdefinerede retter, indlæser fil
    /// </summary>
    public static class FilVælger
    {
        //metode til at vælge json fil ved start af app
        public static void VælgRetFil()
        {
            Console.WriteLine("Vil du bruge standardretter? (ja/nej)");
            string valg = Console.ReadLine()?.Trim().ToLower();

            if (valg == "ja")
            {
                Console.WriteLine($"Indlæser standardretter fra {RetFiler.StandardRetter}");
                RetFiler.ValgtRetFilNavn = RetFiler.StandardRetter;
            }
            else
            {
                Console.WriteLine($"Indlæser brugerdefinerede retter fra {RetFiler.BrugerRetter}");
                RetFiler.ValgtRetFilNavn = RetFiler.BrugerRetter;
            }
        }
    }
}
