using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using madplan.Models;
using madplan.Utilities;

namespace madplan.Services
{
    public class PlanGen
    {
        /// <summary>
        /// metode til at generere ugeplan
        /// </summary>
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
    }
}
