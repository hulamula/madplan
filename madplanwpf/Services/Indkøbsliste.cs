using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using madplanwpf.Models;
using System.IO;        // For File, StreamWriter, StreamReader, Path
using System.Text.Json; // For JSON handling, if needed


namespace madplanwpf.Services
{
    /// <summary>
    /// tilføj ingredienser fra ret til vareliste og exporter til .txt
    /// </summary>
    class Indkøbsliste
    {
        //declare en liste Varer med property
        public List<string> Varer { get; set; }

        //konstruktør af vareliste så Varer ikke er tom/null
        public Indkøbsliste()
        {
            Varer = new List<string>();
        }
        //tilføj ingredienser fra madplan til Varer-liste
        public void TilføjIngredienserFraMadplan(List<Ret> retter)
        {
            foreach (Ret ret in retter)
            {
                Varer.AddRange(ret.Ingredienser);
            }
            //fjern dubletter
            Varer = Varer.Distinct().ToList();
        }

        //udskriv indkøbsliste
        public void Udskriv()
        {
            Console.WriteLine("Indkøbsliste: ");
            foreach (string vare in Varer)
            {
                Console.WriteLine($"- {vare}");
            }
        }
        //gem indkøbsliste til .txt
        public void GemIndkøbsliste()
        {
            int ugeNummer = ISOWeek.GetWeekOfYear(DateTime.Now);
            string filNavn = $"Indkøbsliste uge {ugeNummer}.txt";

            using (StreamWriter writer = new StreamWriter(filNavn))
            {
                writer.WriteLine($"Indkøbsliste uge {ugeNummer}");

                foreach (string vare in Varer)
                {
                    writer.WriteLine($"- {vare}");
                }

            }
            Console.WriteLine($"Indkøbsliste gemt som {filNavn}");
        }
    }
}