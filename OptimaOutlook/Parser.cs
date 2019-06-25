using OptimaOutlook.Models;
using OptimaOutlook.Optima;
using System;
using System.Text.RegularExpressions;

namespace OptimaOutlook
{
    class Parser
    {
        public static string ParseSrs(string StringToParse, Srs Serwis)
        {
            string Builder = StringToParse;

            //Aliasy
            Builder = Builder.Replace("[IdZlecenia]", Serwis.SrZ_SrZId);
            Builder = Builder.Replace("[NumerPelnyZlecenia]", Serwis.SrZ_NumerPelny);
            Builder = Builder.Replace("[OpisZlecenia]", Serwis.SrZ_Opis);
            Builder = Builder.Replace("[Lokalizacja]", Serwis.SrZ_PodNazwa1);
            Builder = Builder.Replace("[DataRealizacji]", Convert.ToDateTime(Serwis.SrZ_DataRealizacji).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Builder = Builder.Replace("[DataZamknięcia]", Serwis.SrZ_DataZamkniecia);

            //Console.WriteLine("Parser: ID " + Serwis.SrZ_SrZId + "  / " + Serwis.SrZ_DataRealizacji);

            //WŁASNA KWERENDA SQL
            if (Builder.Contains("[SQL=>"))
            {
                Builder = ParseSQL(Builder);
            }

            return Builder;
        }

        public static string ParseKnt(string StringToParse, Knt Kontakt)
        {
            string Builder = StringToParse;

            //Aliasy
            Builder = Builder.Replace("[IdKontaktu]", Kontakt.CRK_CRKId);
            Builder = Builder.Replace("[NumerPelnyKontaktu]", Kontakt.CRK_NumerPelny);
            Builder = Builder.Replace("[OpisKontaktu]", Kontakt.CRK_Opis);

           // Console.WriteLine("Parser: ID " + Kontakt.CRK_CRKId + "  / " + Kontakt.CRK_DataDok);

            //WŁASNA KWERENDA SQL
            if (Builder.Contains("[SQL=>"))
            {
                Builder = ParseSQL(Builder);
            }

            return Builder;
        }

        static string ParseSQL(string Builder)
        {
            string stringToReplace = Regex.Match(Builder, @"\[SQL=>([^)]*)\]").Groups[0].Value;
            //MessageBox.Show(stringToReplace);
            string query = Regex.Match(Builder, @"\[SQL=>([^)]*)\]").Groups[1].Value;

            OptimaDB db = new OptimaDB();
            //MessageBox.Show(query);
            return Builder.Replace(stringToReplace, db.ExecuteUserQuery(query));
        }
    }
}
