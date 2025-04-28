using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace madplan.Utilities
{
    /// <summary>
    /// hjælpeklasse der oversætter ugedage til dansk og starter ugen mandag
    /// </summary>
    public static class DanskeUgedage
    {

        //dictionary med dansk ugedagsnavn som værdi for DayOfWeek
        public static readonly Dictionary<DayOfWeek, string> Navne = new Dictionary<DayOfWeek, string>
    {
        { DayOfWeek.Monday, "mandag" },
        { DayOfWeek.Tuesday, "tirsdag" },
        { DayOfWeek.Wednesday, "onsdag" },
        { DayOfWeek.Thursday, "torsdag" },
        { DayOfWeek.Friday, "fredag" },
        { DayOfWeek.Saturday, "lørdag" },
        { DayOfWeek.Sunday, "søndag" },
    };

        //array med dansk rækkefølge af ugedage
        public static readonly DayOfWeek[] ugeStartMandag =
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
            DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
    };

    }
}
