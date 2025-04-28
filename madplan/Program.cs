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


namespace madplan
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



        static void Main(string[] args)
        {

            //opstart: vælg fil til indlæsning
            FilVælger.VælgRetFil();

            //loop til manuel tilføjelse af retter
            InputService.TilføjRet();

            //generer madplan (obs: declare Dictionary til at opbevare resultat af madplan-metode) 
            List<Ret> retter = RetFiler.HentRetter();
            Dictionary<DayOfWeek, Ret> ugePlan = PlanGen.LavPlan(retter);

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