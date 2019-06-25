using OptimaOutlook.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace OptimaOutlook.Optima
{
    public class OptimaDB
    {
        SqlConnection DBConnection;

        public OptimaDB()
        {
            string DataSource = Config.DataSource;
            string Database = Config.Database;
            string UserId = Config.userId;
            string Password = Config.Password;

            DBConnection = new SqlConnection(@"Data source=" + DataSource +
                                ";database=" + Database +
                                ";User id=" + UserId +
                                ";Password=" + Password + ";");
        }

        public string ExecuteUserQuery(string query)
        {
            try
            {
                string tmp = "";
                DBConnection.Open();
                SqlCommand komendaSQL = DBConnection.CreateCommand();
                komendaSQL.CommandText = query;
                SqlDataReader czytnik = komendaSQL.ExecuteReader();

                czytnik.Read();
                tmp = czytnik.GetValue(0).ToString();

                czytnik.Close();
                DBConnection.Close();

                return tmp;
            }
            catch (Exception e)
            {
                return "Wystąpił nieoczekiwany błąd!" + e.Message;
            }
        }

        public List<Srs> GetSrsFromOptima()
        {
            List<Srs> tmp = new List<Srs>();
            try
            {

                DBConnection.Open();
                SqlCommand komendaSQL = DBConnection.CreateCommand();
                komendaSQL.CommandText = "SELECT * FROM CDN.SrsZlecenia";
                SqlDataReader czytnik = komendaSQL.ExecuteReader();

                while (czytnik.Read())
                {
                    tmp.Add(new Srs
                    {
                        SrZ_SrZId = czytnik["SrZ_SrZId"].ToString(),
                        SrZ_DDfId = czytnik["SrZ_DDfId"].ToString(),
                        SrZ_KatID = czytnik["SrZ_KatID"].ToString(),
                        SrZ_NumerNr = czytnik["SrZ_NumerNr"].ToString(),
                        SrZ_NumerPelny = czytnik["SrZ_NumerPelny"].ToString(),
                        SrZ_Bufor = czytnik["SrZ_Bufor"].ToString(),
                        SrZ_Stan = czytnik["SrZ_Stan"].ToString(),
                        SrZ_DokStatus = czytnik["SrZ_DokStatus"].ToString(),
                        SrZ_PodmiotTyp = czytnik["SrZ_PodmiotTyp"].ToString(),
                        SrZ_PodmiotId = czytnik["SrZ_PodmiotId"].ToString(),
                        SrZ_PodNazwa1 = czytnik["SrZ_PodNazwa1"].ToString(),
                        SrZ_PodNazwa2 = czytnik["SrZ_PodNazwa2"].ToString(),
                        SrZ_PodNazwa3 = czytnik["SrZ_PodNazwa3"].ToString(),
                        SrZ_PodKraj = czytnik["SrZ_PodKraj"].ToString(),
                        SrZ_PodWojewodztwo = czytnik["SrZ_PodWojewodztwo"].ToString(),
                        SrZ_PodPowiat = czytnik["SrZ_PodPowiat"].ToString(),
                        SrZ_PodGmina = czytnik["SrZ_PodGmina"].ToString(),
                        SrZ_PodUlica = czytnik["SrZ_PodUlica"].ToString(),
                        SrZ_PodNrDomu = czytnik["SrZ_PodNrDomu"].ToString(),
                        SrZ_PodNrLokalu = czytnik["SrZ_PodNrLokalu"].ToString(),
                        SrZ_PodMiasto = czytnik["SrZ_PodMiasto"].ToString(),
                        SrZ_PodKodPocztowy = czytnik["SrZ_PodKodPocztowy"].ToString(),
                        SrZ_PodPoczta = czytnik["SrZ_PodPoczta"].ToString(),
                        SrZ_PodAdres2 = czytnik["SrZ_PodAdres2"].ToString(),
                        SrZ_Telefon = czytnik["SrZ_Telefon"].ToString(),
                        SrZ_Email = czytnik["SrZ_Email"].ToString(),
                        SrZ_OdbiorcaTyp = czytnik["SrZ_OdbiorcaTyp"].ToString(),
                        SrZ_OdbID = czytnik["SrZ_OdbID"].ToString(),
                        SrZ_OdbNazwa1 = czytnik["SrZ_OdbNazwa1"].ToString(),
                        SrZ_OdbNazwa2 = czytnik["SrZ_OdbNazwa2"].ToString(),
                        SrZ_OdbNazwa3 = czytnik["SrZ_OdbNazwa3"].ToString(),
                        SrZ_OdbKraj = czytnik["SrZ_OdbKraj"].ToString(),
                        SrZ_OdbWojewodztwo = czytnik["SrZ_OdbWojewodztwo"].ToString(),
                        SrZ_OdbPowiat = czytnik["SrZ_OdbPowiat"].ToString(),
                        SrZ_OdbGmina = czytnik["SrZ_OdbGmina"].ToString(),
                        SrZ_OdbUlica = czytnik["SrZ_OdbUlica"].ToString(),
                        SrZ_OdbNrDomu = czytnik["SrZ_OdbNrDomu"].ToString(),
                        SrZ_OdbNrLokalu = czytnik["SrZ_OdbNrLokalu"].ToString(),
                        SrZ_OdbMiasto = czytnik["SrZ_OdbMiasto"].ToString(),
                        SrZ_OdbKodPocztowy = czytnik["SrZ_OdbKodPocztowy"].ToString(),
                        SrZ_OdbPoczta = czytnik["SrZ_OdbPoczta"].ToString(),
                        SrZ_OdbAdres2 = czytnik["SrZ_OdbAdres2"].ToString(),
                        SrZ_OdbTelefon = czytnik["SrZ_OdbTelefon"].ToString(),
                        SrZ_OdbEmail = czytnik["SrZ_OdbEmail"].ToString(),
                        SrZ_ZlecajacyId = czytnik["SrZ_ZlecajacyId"].ToString(),
                        SrZ_ZlecajacyNazwisko = czytnik["SrZ_ZlecajacyNazwisko"].ToString(),
                        SrZ_ProwadzacyTyp = czytnik["SrZ_ProwadzacyTyp"].ToString(),
                        SrZ_ProwadzacyId = czytnik["SrZ_ProwadzacyId"].ToString(),
                        SrZ_Priorytet = czytnik["SrZ_Priorytet"].ToString(),
                        SrZ_SrUId = czytnik["SrZ_SrUId"].ToString(),
                        SrZ_DataDok = czytnik["SrZ_DataDok"].ToString(),
                        SrZ_DataPrzyjecia = czytnik["SrZ_DataPrzyjecia"].ToString(),
                        SrZ_DataRealizacji = czytnik["SrZ_DataRealizacji"].ToString(),
                        SrZ_CzasRealizacji = czytnik["SrZ_CzasRealizacji"].ToString(),
                        SrZ_DataZamkniecia = czytnik["SrZ_DataZamkniecia"].ToString(),
                        SrZ_MagId = czytnik["SrZ_MagId"].ToString(),
                        SrZ_EtapId = czytnik["SrZ_EtapId"].ToString(),
                        SrZ_EtapOpis = czytnik["SrZ_EtapOpis"].ToString(),
                        SrZ_EtapData = czytnik["SrZ_EtapData"].ToString(),
                        SrZ_Wykonano = czytnik["SrZ_Wykonano"].ToString(),
                        SrZ_Opis = czytnik["SrZ_Opis"].ToString(),
                        SrZ_TypNB = czytnik["SrZ_TypNB"].ToString(),
                        SrZ_WartoscNetto = czytnik["SrZ_WartoscNetto"].ToString(),
                        SrZ_WartoscBrutto = czytnik["SrZ_WartoscBrutto"].ToString(),
                        SrZ_WartoscNettoDoFA = czytnik["SrZ_WartoscNettoDoFA"].ToString(),
                        SrZ_WartoscBruttoDoFA = czytnik["SrZ_WartoscBruttoDoFA"].ToString(),
                        SrZ_CzasRealizacjiCzynnosci = czytnik["SrZ_CzasRealizacjiCzynnosci"].ToString(),
                        SrZ_ZbiorczeFaCzesci = czytnik["SrZ_ZbiorczeFaCzesci"].ToString(),
                        SrZ_ZbiorczeTwrIdCzesci = czytnik["SrZ_ZbiorczeTwrIdCzesci"].ToString(),
                        SrZ_ZbiorczeFaCzynnosci = czytnik["SrZ_ZbiorczeFaCzynnosci"].ToString(),
                        SrZ_ZbiorczeTwrIdCzynnosci = czytnik["SrZ_ZbiorczeTwrIdCzynnosci"].ToString(),
                        SrZ_Waluta = czytnik["SrZ_Waluta"].ToString(),
                        SrZ_KursNumer = czytnik["SrZ_KursNumer"].ToString(),
                        SrZ_KursL = czytnik["SrZ_KursL"].ToString(),
                        SrZ_KursM = czytnik["SrZ_KursM"].ToString(),
                        SrZ_DataKur = czytnik["SrZ_DataKur"].ToString(),
                        SrZ_WartoscNettoDoFAPLN = czytnik["SrZ_WartoscNettoDoFAPLN"].ToString(),
                        SrZ_WartoscBruttoDoFAPLN = czytnik["SrZ_WartoscBruttoDoFAPLN"].ToString(),
                        SrZ_WartoscNettoPLN = czytnik["SrZ_WartoscNettoPLN"].ToString(),
                        SrZ_WartoscBruttoPLN = czytnik["SrZ_WartoscBruttoPLN"].ToString(),
                        SrZ_OpeZalId = czytnik["SrZ_OpeZalId"].ToString(),
                        SrZ_StaZalId = czytnik["SrZ_StaZalId"].ToString(),
                        SrZ_TS_Zal = czytnik["SrZ_TS_Zal"].ToString(),
                        SrZ_OpeModId = czytnik["SrZ_OpeModId"].ToString(),
                        SrZ_StaModId = czytnik["SrZ_StaModId"].ToString(),
                        SrZ_TS_Mod = czytnik["SrZ_TS_Mod"].ToString(),
                        SrZ_PrzedstawicielId = czytnik["SrZ_PrzedstawicielId"].ToString(),
                        SrZ_PrzedstawicielNazwisko = czytnik["SrZ_PrzedstawicielNazwisko"].ToString(),
                        SrZ_PrzedstawicielTelefon = czytnik["SrZ_PrzedstawicielTelefon"].ToString(),
                        SrZ_ZlecajacyTelefon = czytnik["SrZ_ZlecajacyTelefon"].ToString(),
                        SrZ_FplTyp = czytnik["SrZ_FplTyp"].ToString(),
                        SrZ_FplId = czytnik["SrZ_FplId"].ToString(),
                        SrZ_TerminPlatTyp = czytnik["SrZ_TerminPlatTyp"].ToString(),
                        SrZ_TerminPlatData = czytnik["SrZ_TerminPlatData"].ToString(),
                        SrZ_OpeModKod = czytnik["SrZ_OpeModKod"].ToString(),
                        SrZ_OpeModNazwisko = czytnik["SrZ_OpeModNazwisko"].ToString(),
                        SrZ_OpeZalKod = czytnik["SrZ_OpeZalKod"].ToString(),
                        SrZ_OpeZalNazwisko = czytnik["SrZ_OpeZalNazwisko"].ToString(),
                    });
                }
                czytnik.Close();
                DBConnection.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine("Wystąpił nieoczekiwany błąd!");
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            return tmp;
        }

        public bool CheckIfExistInDatabase(SrsLink serwis)
        {
            try
            {
                bool tmp = false;
                DBConnection.Open();
                SqlCommand komendaSQL = DBConnection.CreateCommand();
                komendaSQL.CommandText = "SELECT * FROM CDN.SrsZlecenia WHERE SrZ_SrZId=" + serwis.IdSerwisu;
                SqlDataReader czytnik = komendaSQL.ExecuteReader();


                tmp = czytnik.HasRows;

                czytnik.Close();
                DBConnection.Close();

                return tmp;
            }
            catch (Exception e)
            {
                throw new SystemException("Wystąpił nieoczekiwany błąd!" + e.Message);
            }
        }

        public List<Knt> GetKntFromOptima()
        {
            throw new NotImplementedException();
        }
    }
}
