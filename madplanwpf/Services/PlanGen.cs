using madplanwpf.Models;
using madplanwpf.Utilities;


namespace madplanwpf.Services
{
    public class PlanGen
    {
        /// <summary>
        /// metode til at generere ugeplan
        /// </summary>
        public static Dictionary<DayOfWeek, Ret> LavPlan(List<Ret> retter)
        {
            //returner tom dictionary hvis listen med retter er tom
            if (retter.Count == 0)
            {
                return new Dictionary<DayOfWeek, Ret>();
            }

            //randomiser retter
            var random = new Random();
            List<Ret> randomretter = retter.OrderBy(x => random.Next()).ToList();

            //konstruer en tom dictionary med ugedag som nøgle og Ret som værdi
            Dictionary<DayOfWeek, Ret> ugePlan = new Dictionary<DayOfWeek, Ret>();

            //index integer til brug i foreach loop
            int index = 0;

            //udfyld dictionary med vilkårlige retter
            //brug modulo "%" til at genbruge retter hvis listen er kortere end 7
            foreach (DayOfWeek dag in DanskeUgedage.ugeStartMandag)
            {
                ugePlan[dag] = randomretter[index % randomretter.Count];
                index++;
            }
            return ugePlan;
        }
    }
}
