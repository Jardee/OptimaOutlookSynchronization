using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using OptimaOutlook.Models;
using OptimaOutlook.Optima;
using OptimaOutlook.MGraph;
using OptimaOutlook;
using Microsoft.Graph;


namespace OptimaOutlookCMDUI
{
    class Program
    {
        private Timer _timer = new Timer();
        GraphServiceClient graphServiceClient;
        OptimaDB db;

        static void Main(string[] args)
        {
            new Program().start();
        }

        public void start()
        {
            graphServiceClient = MGraphHelper.GetGraphServiceClient(Config.ClientId, Config.Authority, Config.Scopes);

            db = new OptimaDB();

            _timer = new Timer(60000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
            Console.ReadKey();
        }

        private async Task CheckSrs()
        {
            List<Srs> Serwisy = db.GetSrsFromOptima();

            foreach (Srs Serwis in Serwisy)
            {
                SrsLink powiazanie = LinksDatabase.PobierzPowiazanieDo(Serwis);
                if (powiazanie != null)
                {

                    if (powiazanie.DataModyfikacji != Convert.ToDateTime(Serwis.SrZ_TS_Mod))
                    {
                        Console.WriteLine("UPDATE: ID " + Serwis.SrZ_SrZId + " Opis: " + Serwis.SrZ_Opis + " Data Wykonania: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                        await MGraphHelper.UpdateEvent(powiazanie.EventId, Serwis, graphServiceClient, Config.SrsTopicTemplate, Config.SrsDescriptionTemplate);
                        LinksDatabase.AktualizujPowiazanie(powiazanie, Serwis);
                    }
                }
                else
                {
                    Console.WriteLine("INSERT: ID " + Serwis.SrZ_SrZId + " Opis: " + Serwis.SrZ_Opis + " Data Wykonania: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                    string idkalendarza = await MGraphHelper.StworzNowyEvent(Serwis, graphServiceClient, Config.SrsTopicTemplate, Config.SrsDescriptionTemplate, Config.SrsCalendarId);
                    LinksDatabase.DodajPowiazanie(Serwis, idkalendarza);
                }
            }

            foreach (var item in LinksDatabase.GetAllSrsLinks())
            {
                //Console.WriteLine(item.IdSerwisu + " " + db.CheckIfExistInDatabase(item));
                if (!db.CheckIfExistInDatabase(item))
                {
                    //TO-DO wrzucanie logów do bazy, wysyłanie wiadomosci do wybranych uzytkowników gdy coś się zadzieje doda nowe / aktualizuje / usunie
                    Console.WriteLine("DELETE: ID Serwisu " + item.IdSerwisu + " Data Wykonania: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                    await MGraphHelper.DeleteEvent(item.EventId, graphServiceClient);
                    LinksDatabase.UsunPowiazanieSrs(item);
                }
            }
        }

        private async Task CheckKnt()
        {
            List<Knt> Kontakty = db.GetKntFromOptima();

            foreach (Knt Kontakt in Kontakty)
            {
                SrsLink powiazanie = LinksDatabase.PobierzPowiazanieDo(Kontakt);
                if (powiazanie != null)
                {

                    if (powiazanie.DataModyfikacji != Convert.ToDateTime(Kontakt.CRK_TS_Mod))
                    {
                        Console.WriteLine("UPDATE: ID " + Kontakt.CRK_CRKId + " Opis: " + Kontakt.CRK_Opis + " Data Wykonania: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                        await MGraphHelper.UpdateEvent(powiazanie.EventId, Kontakt, graphServiceClient, Config.KntTopicTemplate, Config.KntDescriptionTemplate);
                        LinksDatabase.AktualizujPowiazanie(powiazanie, Kontakt);
                    }
                }
                else
                {
                    //Tutaj to trochę inaczej trzeba zrobić w pliku z konfiguracją będzie trzeba uwzględnić Do jakiego np. opiekuna jaki jest przypisany kalendarz (id)
                    Console.WriteLine("INSERT: ID " + Kontakt.CRK_CRKId + " Opis: " + Kontakt.CRK_Opis + " Data Wykonania: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                    string idkalendarza = await MGraphHelper.StworzNowyEvent(Kontakt, graphServiceClient, Config.KntTopicTemplate, Config.KntDescriptionTemplate, Config.SrsCalendarId);
                    LinksDatabase.DodajPowiazanie(Kontakt, idkalendarza);
                }
            }
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            await CheckSrs();

            //await CheckKnt();

            Console.ReadKey();
        }

    }
}
