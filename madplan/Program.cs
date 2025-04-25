using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;


namespace madplan_beta_1._1
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

    //Klasse til at gemme og indlæse og måske på sigt modificere filer
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
    internal class Program
    {
        //dictionary med dansk ugedagsnavn som værdi for DayOfWeek
        private static readonly Dictionary<DayOfWeek, string> DanskeUgedage = new Dictionary<DayOfWeek, string>
        {
            {DayOfWeek.Monday, "mandag" },
            {DayOfWeek.Tuesday, "tirsdag" },
            {DayOfWeek.Wednesday, "onsdag" },
            {DayOfWeek.Thursday, "torsdag" },
            {DayOfWeek.Friday, "fredag" },
            {DayOfWeek.Saturday, "lørdag" },
            {DayOfWeek.Sunday, "søndag" },
        };

        //array med dansk rækkefølge af ugedage
        private static readonly DayOfWeek[] ugeStartMandag =
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
            DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
        };

        static Program()

        //bruges til at vise "ø" korrekt i Console.WriteLine
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

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

        //metode til indhentning af ingrediensliste via indtastning
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

        //metode til indtastning af ret
        static Ret IndtastRet()
        {
            RetFiler.OpdaterRetAntal();
            Console.WriteLine("Indtast navn på ret");
            string navn = Console.ReadLine();
            List<string> ingredienser = new List<string>();
            ingredienser = IndtastIngredienser();
            return new Ret(navn, ingredienser);
        }

        //metode til at tilføje retter til json via loop
        static void TilføjRet()
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



        //metode til at generere ugeplan
        public static Dictionary<DayOfWeek, Ret> LavPlan(List<Ret> retter)
        {
            //sikkerhedstjek for tom liste
            if (retter.Count == 0)
            {
                Console.WriteLine("Der er ingen indtastede retter, programmet lukker");
                Console.ReadLine();
                return new Dictionary<DayOfWeek, Ret>();
            }

            //randomiser retter
            var random = new Random();
            List<Ret> randomretter = retter.OrderBy(x => random.Next()).ToList();

            //konstruer en tom dictionary med ugedag som nøgle og Ret som værdi
            Dictionary<DayOfWeek, Ret> ugePlan = new Dictionary<DayOfWeek, Ret>();

            //giv hver nøgle en værdi
            int index = 0;
            foreach (DayOfWeek dag in ugeStartMandag)
            {
                ugePlan[dag] = randomretter[index % randomretter.Count];
                index++;
            }
            return ugePlan;
        }

        static void Main(string[] args)
        {

            //opstart: vælg fil til indlæsning
            VælgRetFil();

            //loop til manuel tilføjelse af retter
            TilføjRet();

            //generer madplan (obs: declare Dictionary til at opbevare resultat af madplan-metode) 
            List<Ret> retter = RetFiler.HentRetter();
            Dictionary<DayOfWeek, Ret> ugePlan = LavPlan(retter);

            //udskriv madplan
            Console.WriteLine("Ugens madplan: ");
            foreach (var dagRet in ugePlan)
            {
                string danskUgedag = DanskeUgedage[dagRet.Key];
                Console.WriteLine($"{danskUgedag}: {dagRet.Value}");
            }
            Console.ReadLine();
            //konstruer tom indkøbsliste, konstruer liste med planlagte retter
            //tilføj ingredienser fra retter til indkøbslisten
            Indkøbsliste indkøbsliste = new Indkøbsliste();
            List<Ret> planlagteRetter = ugePlan.Values.ToList();
            indkøbsliste.TilføjIngredienserFraMadplan(planlagteRetter);
            indkøbsliste.Udskriv();
            Console.ReadKey();
            RetFiler.GemUgePlan(ugePlan);
            Console.ReadKey();
            indkøbsliste.GemIndkøbsliste();
            Console.ReadKey();

        }
    }
}