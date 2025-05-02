using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using madplan.Models;

namespace madplan.Services
{
    /// <summary>
    /// Klasse til at gemme og indlæse og modificere filer
    /// </summary>
    public class RetFiler
    {
        //indstil JsonSerializerOptions
        //WriteIndented giver menneskeligt læsbar json
        //Encoder gør at json ikke "undgår" æ ø å'er
        private static readonly JsonSerializerOptions indstillinger = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
        };

        //navngiv fil til brug for JSON (initialiser som ikke-null tom "")
        //navngiv filer med hhv. standard- og brugerinputtede retter
        public static string ValgtRetFilNavn = string.Empty;
        public static string BrugerRetter = "Retter.json";
        public static string StandardRetter = "DefaultRetter.json";

        //metode til at hente liste med retter fra fil
        public static List<Ret> HentRetter()
        {

            //hvis json fil findes og hvis den ikke er tom/null
            //og hvis den kan indlæses korrekt så oprettes liste

            if (File.Exists(ValgtRetFilNavn))
            {
                string jsonString = File.ReadAllText(ValgtRetFilNavn);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    return JsonSerializer.Deserialize<List<Ret>>(jsonString) ?? new List<Ret>();
                }

            }
            //hvis json ikke findes returneres tom liste via List<T> konstruktør
            //så kode der bruger listen ikke fejler
            return new List<Ret>();

        }

        //public metode til at finde antal retter via json-fil og opdatere det i Ret-klasse
        public static void OpdaterRetAntal()
        {
            List<Ret> retter = HentRetter();
            if (retter.Count > 0)
            {
                int højesteNummer = retter[^1].Nummer;
                Ret.SætAntal(højesteNummer);
            }
        }

        //public metode til at gemme en ret, kan måske gøres privat senere
        public static void GemRet(Ret ret)
        {
            //hent eksisterende retfil
            List<Ret> retter = HentRetter();

            //sæt ret-nummer 

            //tilføj ny ret til liste
            retter.Add(ret);

            //skriv liste med eksisterende + ny ret til
            string jsonString = JsonSerializer.Serialize(retter, indstillinger);
            File.WriteAllText(ValgtRetFilNavn, jsonString);
            Console.WriteLine($"Ret nummer {ret.Nummer} {ret.Navn} gemt til {Path.GetFullPath(ValgtRetFilNavn)}");
        }

        //public metode til at gemme en UgePlan som json
        public static void GemUgePlan(Dictionary<DayOfWeek, Ret> ugePlan)
        {
            int ugeNummer = ISOWeek.GetWeekOfYear(DateTime.Now);

            string filnavn = $"Madplan uge {ugeNummer}.json";

            string jsonString = JsonSerializer.Serialize(ugePlan, indstillinger);

            File.WriteAllText(filnavn, jsonString);

            Console.WriteLine($"Madplan gemt som {filnavn}");
        }
    }
}
