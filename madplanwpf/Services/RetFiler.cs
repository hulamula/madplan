using System.Globalization;
using System.IO;        // For File, StreamWriter, StreamReader, Path
using System.Text.Json;
using System.Windows; // For JSON handling, if needed
using madplanwpf.Models;


namespace madplanwpf.Services
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
        public const string BrugerRetter = "Retter.json";
        public const string StandardRetter = "DefaultRetter.json";

        //metode til at ensrette store og små begyndelsesbogstaver
        public static void CaseFixer(List<Ret> retter)
        {
            foreach (Ret ret in retter)
            {
                if (!string.IsNullOrEmpty(ret.Navn))
                {
                    ret.Navn = char.ToUpper(ret.Navn[0]) + ret.Navn.Substring(1).ToLower();
                }
                if (ret.Ingredienser != null)
                {
                    ret.Ingredienser = ret.Ingredienser.Select(i => i.ToLower()).ToList();
                }

            }
        }

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
                    List<Ret> retter = JsonSerializer.Deserialize<List<Ret>>(jsonString) ?? new List<Ret>();
                    CaseFixer(retter);
                    return retter;
                }

            }
            //hvis json ikke findes returneres tom liste via List<T> konstruktør
            //så kode der bruger listen ikke fejler
            return new List<Ret>();

        }


        //metode til at gemme en ny ret
        public static void GemNyRet(Ret ret)
        {
            if (ret == null)
            {
                return;
            }

            //hent eksisterende retfil
            List<Ret> retter = HentRetter();

            //tilføj ny ret til liste
            retter.Add(ret);

            //fix case
            CaseFixer(retter);

            //skriv liste med eksisterende + ny ret til
            string jsonString = JsonSerializer.Serialize(retter, indstillinger);
            try
            {
                File.WriteAllText(ValgtRetFilNavn, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fejl ved gem: " + ex.Message);
            }
            
        }

        //public metode til at gemme liste med retter 
        public static void GemRetter(List<Ret> retter, string sti)
        {
            //fix case
            CaseFixer(retter);

            //skriv liste med retter 
            string jsonString = JsonSerializer.Serialize(retter, indstillinger);
            try
            {
                File.WriteAllText(sti, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fejl ved gem: " + ex.Message);
            }
        }
    }
}
