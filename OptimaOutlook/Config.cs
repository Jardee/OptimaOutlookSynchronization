using System;
using System.Collections.Generic;
using System.Globalization;

namespace OptimaOutlook
{
    static public class Config
    {
        static public readonly string Instance = "https://login.microsoftonline.com/{0}";

        static public readonly string TenantId = "common";


        static public readonly string ClientId;

        static public readonly string DataSource;

        static public readonly string Database;

        static public readonly string userId;

        static public readonly string Password;

        static public readonly string SrsTopicTemplate;

        static public readonly string SrsDescriptionTemplate;

        static public readonly string KntTopicTemplate;

        static public readonly string KntDescriptionTemplate;

        static public readonly string SrsCalendarId;

        static public readonly string SrSSMTPClientHost;

        static public readonly int SrSSMTPClientPort;

        static public readonly string SrSMailUsername;

        static public readonly string SrSMailPassword;

        static public readonly IEnumerable<string> Scopes = new string[]{"User.Read",
                                                                        "Calendars.Read",
                                                                        "Calendars.Read.Shared",
                                                                        "Calendars.ReadWrite",
                                                                        "Calendars.ReadWrite.Shared" };

        static public readonly string Authority = string.Format(CultureInfo.InvariantCulture, Instance, TenantId);

        //konstruktor
        static Config()
        {
            LinksDatabase.ToConfig tmp = LinksDatabase.GetConfig();

            DataSource = tmp.DataSource;

            Database = tmp.Database;

            userId = tmp.userId;

            Password = tmp.Password;

            SrsTopicTemplate = tmp.SrsTopicTemplate;

            SrsDescriptionTemplate = tmp.SrsDescriptionTemplate;

            KntTopicTemplate = tmp.KntTopicTemplate;

            KntDescriptionTemplate = tmp.KntDescriptionTemplate;

            SrsCalendarId = tmp.SrsCalendarId;

            ClientId = tmp.ClientId;

            SrSSMTPClientHost = tmp.SrSSMTPClientHost;

            SrSSMTPClientPort = tmp.SrSSMTPClientPort;
             
            SrSMailUsername = tmp.SrSMailUsername;

            SrSMailPassword = tmp.SrSMailPassword;

            Console.WriteLine(Database + " " + DataSource + " " + userId);
        }
    }
}
