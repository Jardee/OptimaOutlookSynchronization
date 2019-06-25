using System;
using System.Collections.Generic;
using Microsoft.Identity.Client;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using OptimaOutlook.Models;

namespace OptimaOutlook.MGraph
{
    public class MGraphHelper
    {
        public static GraphServiceClient GetGraphServiceClient(string clientId, string authority, IEnumerable<string> scopes)
        {
            var authenticationProvider = CreateAuthorizationProvider(clientId, authority, scopes);
            return new GraphServiceClient(authenticationProvider);
        }

        private static IAuthenticationProvider CreateAuthorizationProvider(string clientId, string authority, IEnumerable<string> scopes)
        {
            IPublicClientApplication clientApplication;

            clientApplication = PublicClientApplicationBuilder.Create(clientId)
                                    .WithAuthority(authority).Build();

            //= new PublicClientApplication(clientId, authority);
            return new MsalAuthenticationProvider(clientApplication, scopes.ToArray());
        }

        //dla srs
        public static async Task UpdateEvent(string ID, Srs serwis, GraphServiceClient x, string srsTopicTemplate, string srsDescTemplate)
        {
            await x.Me.Calendar.Events[ID].Request().UpdateAsync(new Event()
            {
                Subject = Parser.ParseSrs(srsTopicTemplate, serwis),
                Body = new ItemBody() { ContentType = BodyType.Html, Content = Parser.ParseSrs(srsDescTemplate, serwis) },
                Start = new DateTimeTimeZone() { DateTime = Convert.ToDateTime(serwis.SrZ_DataPrzyjecia).ToString("yyyy-MM-ddTHH:mm:ss.fffffff"), TimeZone = "Europe/Warsaw" },
                End = new DateTimeTimeZone() { DateTime = Convert.ToDateTime(serwis.SrZ_DataPrzyjecia).ToString("yyyy-MM-ddTHH:mm:ss.fffffff"), TimeZone = "Europe/Warsaw" },
                Location = new Location() { DisplayName = serwis.SrZ_PodMiasto }

            });

        }

        
        public static async Task DeleteEvent(string EventID, GraphServiceClient x)
        {
            await x.Me.Calendar.Events[EventID].Request().DeleteAsync();

        }

        public static async Task<string> StworzNowyEvent(Srs serwis, GraphServiceClient x, string srsTopicTemplate, string srsDescTemplate, string srsCalendarId)
        {
            Microsoft.Graph.Location loc = new Location();
            loc.DisplayName = serwis.SrZ_PodMiasto;
            var newEvent = new Event();
            newEvent.Subject = Parser.ParseSrs(srsTopicTemplate, serwis);
            newEvent.Body = new ItemBody() { ContentType = BodyType.Html, Content = Parser.ParseSrs(srsDescTemplate, serwis) };
            newEvent.Location = loc;
            //Console.WriteLine("MGRAPH: ID " + serwis.SrZ_SrZId + " / " + serwis.SrZ_DataPrzyjecia + " / " + serwis.SrZ_DataPrzyjecia);
            newEvent.Start = new DateTimeTimeZone() { DateTime = Convert.ToDateTime(serwis.SrZ_DataPrzyjecia).ToString("yyyy-MM-ddTHH:mm:ss.fffffff"), TimeZone = "Europe/Warsaw" };
            newEvent.End = new DateTimeTimeZone() { DateTime = Convert.ToDateTime(serwis.SrZ_DataPrzyjecia).ToString("yyyy-MM-ddTHH:mm:ss.fffffff"), TimeZone = "Europe/Warsaw" };

            Event xx = await x.Me.Calendars[srsCalendarId].Events.Request().AddAsync(newEvent);

            return xx.Id;
        }

        //dla knt
        public static Task<string> StworzNowyEvent(Knt kontakt, GraphServiceClient graphServiceClient, string srsTopicTemplate, string srsDescriptionTemplate, string srsCalendarId)
        {
            throw new NotImplementedException();
        }

        public static async Task<string> UpdateEvent(string idKalendarza, Knt kontakt, GraphServiceClient graphServiceClient, string srsTopicTemplate, string srsDescriptionTemplate)
        {
            throw new NotImplementedException();
        }
    }
}
