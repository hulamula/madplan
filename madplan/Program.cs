using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using madplan.Models;
using madplan.Services;
using madplan.Utilities;


namespace madplan_beta_1._1
{
    }
    internal class Program
    {
     
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
            foreach (DayOfWeek dag in DanskeUgedage.ugeStartMandag)
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
                string danskUgedag = DanskeUgedage.Navne[dagRet.Key];
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