using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using OptimaOutlook.Models;
using OptimaOutlook.Optima;
using OptimaOutlook.MGraph;
using Microsoft.Graph;
using System.Net.Mail;
using System.Net;

namespace OptimaOutlook
{
    public class MainClass
    {
        private Timer _timer = new Timer();
        GraphServiceClient graphServiceClient;
        OptimaDB db;


        public void start()
        {
            graphServiceClient = MGraphHelper.GetGraphServiceClient(Config.ClientId, Config.Authority, Config.Scopes);

            db = new OptimaDB();

            _timer = new Timer(20000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
            Console.ReadKey();
        }

        private async Task CheckSrs()
        {
            List<Srs> Serwisy = db.GetSrsFromOptima();
            string Powiadomienie = "";

            foreach (Srs Serwis in Serwisy)
            {
                SrsLink powiazanie = LinksDatabase.PobierzPowiazanieDo(Serwis);
                if (powiazanie != null)
                {

                    if (powiazanie.DataModyfikacji != Convert.ToDateTime(Serwis.SrZ_TS_Mod))
                    {
                        string msg = "Zaktualizowano Zlecenie Serwisowe: ID " + Serwis.SrZ_SrZId + " Opis: " + Serwis.SrZ_Opis + " o godzinie: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                        Console.WriteLine(msg);
                        Powiadomienie += (msg + Environment.NewLine);
                        await MGraphHelper.UpdateEvent(powiazanie.EventId, Serwis, graphServiceClient, Config.SrsTopicTemplate, Config.SrsDescriptionTemplate);
                        LinksDatabase.AktualizujPowiazanie(powiazanie, Serwis);

                    }
                }
                else
                {
                    string msg = "Dodano Zlecenie Serwisowe: ID " + Serwis.SrZ_SrZId + " Opis: " + Serwis.SrZ_Opis + " o godzinie: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                    Console.WriteLine(msg);
                    Powiadomienie += (msg + Environment.NewLine);
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

                    string msg = "Usunięto Zlecenie Serwisowe: ID Serwisu " + item.IdSerwisu + " o godzinie: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                    Console.WriteLine(msg);
                    Powiadomienie += (msg + Environment.NewLine);
                    await MGraphHelper.DeleteEvent(item.EventId, graphServiceClient);
                    LinksDatabase.UsunPowiazanieSrs(item);
                }
            }

            if (!(Powiadomienie == ""))
            {
                //Send mail
                SmtpClient x = new SmtpClient(Config.SrSSMTPClientHost, Config.SrSSMTPClientPort);
                x.EnableSsl = true;
                x.Timeout = 10000;
                x.DeliveryMethod = SmtpDeliveryMethod.Network;
                x.UseDefaultCredentials = false;
                x.Credentials = new NetworkCredential(Config.SrSMailUsername, Config.SrSMailPassword);

                MailMessage msgg = new MailMessage();
                msgg.To.Add(Config.SrSMailUsername);
                msgg.From = new MailAddress(Config.SrSMailUsername);
                msgg.Subject = "Moduł Zlecenia Serwisowe";
                msgg.Body = Powiadomienie;

                x.SendAsync(msgg, null);
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
            _timer.Stop();

            //await CheckKnt();
            await CheckSrs().ContinueWith((z) => _timer.Start());
        }

    }
}
