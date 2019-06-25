using System;
using System.Collections.Generic;
using System.Data.SQLite;
using OptimaOutlook.Models;

namespace OptimaOutlook
{
    public class LinksDatabase
    {
        
        static readonly string connectionString;
        static LinksDatabase()
        {
            string homePath = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            Console.WriteLine(homePath);
            connectionString = @"Data Source=" + homePath + "\\Baza.db; Version=3; FailIfMissing=True; Foreign Keys=True;";
        }
        //pobierz srs
        public static SrsLink PobierzPowiazanieDo(Srs serwis)
        {
            SrsLink la = new SrsLink();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM PowiazaniaSrs WHERE IdSerwisu = " + serwis.SrZ_SrZId;

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                la.Id = Int32.Parse(reader["Id"].ToString());
                                la.IdSerwisu = Int32.Parse(reader["IdSerwisu"].ToString());
                                la.EventId = reader["EventId"].ToString();
                                la.DataModyfikacji = Convert.ToDateTime(reader["DataModyfikacjiSerwisu"].ToString());
                            }
                            else return null;

                        }
                    }
                    conn.Close();
                }
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message + " PobierzPowiazanieDo(Srs serwis)");
            }
            return la;
        }

        //pobierz knt
        public static SrsLink PobierzPowiazanieDo(Knt kontakt)
        {
            throw new NotImplementedException();
        }

        //update srs
        public static void AktualizujPowiazanie(SrsLink powiazanie, Srs Serwis)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE PowiazaniaSrs "
                        + "SET DataModyfikacjiSerwisu = @Data "
                        + "WHERE Id = @Id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@Data", Serwis.SrZ_TS_Mod);
                    cmd.Parameters.AddWithValue("@Id", powiazanie.Id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException e)
                    {
                        Console.WriteLine(e.Message + " AktualizujPowiazanie(SrsLink powiazanie, Srs Serwis)");
                    }
                }
                conn.Close();
            }
        }
        //update knt
        public static void AktualizujPowiazanie(SrsLink powiazanie, Knt kontakt)
        {
            throw new NotImplementedException();
        }



        //dodaj srs
        public static void DodajPowiazanie(Srs Serwis, string EventId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO PowiazaniaSrs(IdSerwisu, EventId, DataModyfikacjiSerwisu) VALUES (@IdSerwisu, @EventId, @DataModyfikacjiSerwisu)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@IdSerwisu", Serwis.SrZ_SrZId);
                    cmd.Parameters.AddWithValue("@EventId", EventId);
                    cmd.Parameters.AddWithValue("@DataModyfikacjiSerwisu", Serwis.SrZ_TS_Mod);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException e)
                    {
                        Console.WriteLine(e.Message + " DodajPowiazanie(Srs Serwis, string EventId)");
                    }
                }
                conn.Close();
            }
        }

        //dodaj knt
        public static void DodajPowiazanie(Knt kontakt, string EventId)
        {
            throw new NotImplementedException();
        }



        //delete Srs
        public static void UsunPowiazanieSrs(SrsLink powiazanie)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "DELETE FROM PowiazaniaSrs WHERE Id = @IdPowiazania";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@IdPowiazania", powiazanie.Id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException e)
                    {
                        Console.WriteLine(e.Message + " UsunPowiazanieSrs(SrsLink powiazanie)");
                    }
                }
                conn.Close();
            }
        }


        public static ToConfig GetConfig()
        {
            //TO-DO do zrobienia !
            ToConfig tmp;
            tmp.DataSource = "";

            tmp.Database = "";

            tmp.userId = "";

            tmp.Password = "";

            tmp.SrsTopicTemplate = "";

            tmp.SrsDescriptionTemplate = "";

            tmp.KntTopicTemplate = "";

            tmp.KntDescriptionTemplate = "";

            tmp.SrsCalendarId = "";

            tmp.ClientId = "";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM Config;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tmp.DataSource = reader["DataSource"].ToString();

                                tmp.Database = reader["Database"].ToString();

                                tmp.userId = reader["UserId"].ToString();

                                tmp.Password = reader["Password"].ToString();

                                tmp.SrsTopicTemplate = reader["SrsTopicTemplate"].ToString();

                                tmp.SrsDescriptionTemplate = reader["SrsDescTemplate"].ToString();

                                tmp.KntTopicTemplate = reader["KntTopicTemplate"].ToString();

                                tmp.KntDescriptionTemplate = reader["KntDescTemplate"].ToString();

                                tmp.SrsCalendarId = reader["SrsCalendarId"].ToString();

                                tmp.ClientId = reader["ClientId"].ToString();
                            }
                            

                        }
                    }
                    conn.Close();
                }
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
            }
            return tmp;
        }

        public static List<SrsLink> GetAllSrsLinks()
        {
            List<SrsLink> la = new List<SrsLink>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM PowiazaniaSrs;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var tmp = new SrsLink();
                                tmp.Id = Int32.Parse(reader["Id"].ToString());
                                tmp.IdSerwisu = Int32.Parse(reader["IdSerwisu"].ToString());
                                tmp.EventId = reader["EventId"].ToString();
                                tmp.DataModyfikacji = Convert.ToDateTime(reader["DataModyfikacjiSerwisu"].ToString());
                                la.Add(tmp);
                            }

                        }
                    }
                    conn.Close();
                }
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message + " get allsrs links)");
            }
            return la;
        }


        public struct ToConfig
        {
            public string DataSource;

            public string Database;

            public string userId;

            public string Password;

            public string SrsTopicTemplate;

            public string SrsDescriptionTemplate;

            public string KntTopicTemplate;

            public string KntDescriptionTemplate;

            public string SrsCalendarId;

            public string ClientId;
        }
    }
}
