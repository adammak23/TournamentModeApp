using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WindowsApp2.Models;
using WindowsApp2.ViewModels.Commands;
using System.Diagnostics;

using RiotSharp;
using RiotSharp.StatsEndpoint;
using RiotSharp.SummonerEndpoint;
using RiotSharp.LeagueEndpoint;
using System.Net.Http;
using Windows.UI.Xaml;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using System.Windows.Input;
using RiotSharp.GameEndpoint;
using Windows.UI.Notifications;
using RiotSharp.ChampionEndpoint;

namespace WindowsApp2.ViewModels
{
    [Newtonsoft.Json.JsonArrayAttribute]//not needed?
    [Windows.UI.Xaml.Data.Bindable]
    public class DetailPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ICommand AddSummoner { get; set; }
        public ICommand RefreshSummoner { get; set; }
        //public ICommand checktournaments { get; set; }

        private string summonername;
        public string Summoner { get { return summonername; } set { summonername = value; RaisePropertyChanged("Summoner"); } }

        private string summonername2 = UserAccount.GetSummoner();
        public string SummonerName { get { return summonername2; } set { summonername2 = value; RaisePropertyChanged("SummonerName"); } }

        private string soloq = "Solo Queue: Unranked";
        public string SoloQ { get { return soloq; } set { soloq = value; RaisePropertyChanged("SoloQ"); } }

        private string flexq = "Flex Queue: Unranked";
        public string FlexQ { get { return flexq; } set { flexq = value; RaisePropertyChanged("FlexQ"); } }

        private string tt = "3v3: Unranked";
        public string TT { get { return tt; } set { tt = value; RaisePropertyChanged("TT"); } }

        private string summonerborder = "none";
        public string SummonerBorder { get { return summonerborder; } set { summonerborder = value; RaisePropertyChanged("SummonerBorder"); } }

        private string summonericon = "none";
        public string SummonerIcon { get { return summonericon; } set { summonericon = value; RaisePropertyChanged("SummonerIcon"); } }

        private string enrolled = "You are not enrolled.";
        public string Enrolled { get { return enrolled; } set { enrolled = value; RaisePropertyChanged("Enrolled"); } }

        private string tournamentcode = "";
        public string TournamentCode { get { return tournamentcode; } set { tournamentcode = value; RaisePropertyChanged("TournamentCode"); } }

        private string soloqmedal = "http://static.lolskill.net/img/tiers/192/unranked.png";
        public string SoloQMedal { get { return soloqmedal; } set { soloqmedal = value; RaisePropertyChanged("SoloQMedal"); } }

        private string flexqmedal = "http://static.lolskill.net/img/tiers/192/unranked.png";
        public string FlexQMedal { get { return flexqmedal; } set { flexqmedal = value; RaisePropertyChanged("FlexQMedal"); } }

        private string ttmedal = "http://static.lolskill.net/img/tiers/192/unranked.png";
        public string TTMedal { get { return ttmedal; } set { ttmedal = value; RaisePropertyChanged("TTMedal"); } }

        private string errorText;
        public string ErrorText
        {
            get { return errorText; }
            set { errorText = value; RaisePropertyChanged("ErrorText"); }
        }

        private string username = "Empty Username";
        public string Username { get { return username; } set { username = value; RaisePropertyChanged("Username"); } }

        public string mostplayedchampion="";
        public string MostPlayedChampion { get { return mostplayedchampion; } set { mostplayedchampion = value; RaisePropertyChanged("MostPlayedChampion"); } }
        public string mostplayedchampionimage="";
        public string MostPlayedChampionImage { get { return mostplayedchampionimage; } set { mostplayedchampionimage = value; RaisePropertyChanged("MostPlayedChampionImage"); } }

        public List<Game> gry;
        public List<int> listaId;



        public DetailPageViewModel()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            //Refresh
            Username = UserAccount.LoggedInUsername;
            Enrolled = "You are not enrolled.";

            this.listaId = new List<int>();
            if (UserAccount.EnrolledTournament!="") Enrolled = "Participant of: "+UserAccount.EnrolledTournament;
            //this.checktournaments = new Command(CheckTournaments);
            this.AddSummoner = new Command(AddingSummoner);
            this.RefreshSummoner = new Command(CheckSummonerFromDatabase);
            if (Enrolled != "You are not enrolled.") TournamentCode = "EUNE04383-4e59af6f-c21d-482a-8ad1-05e27b3b0f41";
        }

        public string server = "eune";
        RiotApi api = Api.GetApi();
        StaticRiotApi StaticApi = Api.GetStaticApi();

        public List<League> liga;
        Summoner summoner;

        public int RomanToIntConverter(string ro)
        {
            if (ro == "V") return 5;
            if (ro == "IV") return 4;
            if (ro == "III") return 3;
            if (ro == "II") return 2;
            if (ro == "I") return 1;
            else return 0;
        }
        public void ImageStrings()
        {
            SummonerIcon = "http://avatar.leagueoflegends.com/" + server + "/" + SummonerName + ".png";
            //https://opgg-static.akamaized.net/images/lol/champion/Morgana.png
        }

        /*
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            var value = parameter as string;
            // TODO: use the parameter somehow
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
                suspensionState[nameof(Login)] = Login;
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }
        */

        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }

        public async void AddingSummoner()
        {

            ErrorText = "Wait...";

            var values = new Dictionary<string, string>
               {
                  { "post_user", Username },
                  { "post_summ", Summoner }
               };

            var content = new FormUrlEncodedContent(values);

            try
            {
                HttpClient client = new HttpClient();
                var response = await client.PostAsync("https://adammak2342.000webhostapp.com/AddSummoner.php", content);
                var responseString = await response.Content.ReadAsStringAsync();
                UserAccount.SetSummoner(Summoner);
                SummonerName = Summoner;
                CheckSummonerFromDatabase();
                ErrorText = responseString;
            }
            catch (HttpRequestException e) { ErrorText = e.StackTrace; }
        }

        public static async Task Refresh()
        {
            var values = new Dictionary<string, string>
               {
                  { "post_user", UserAccount.LoggedInUsername },
               };

            var content = new FormUrlEncodedContent(values);
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.PostAsync("https://adammak2342.000webhostapp.com/account.php", content);
                var responseString = await response.Content.ReadAsStringAsync();
                UserAccount.SetSummoner(responseString);
            }
            catch (HttpRequestException e) { }
        }

        public async Task GetSumm()
        {
            try
            {
                await Refresh();
                notification();
                SummonerName = UserAccount.GetSummoner();
                summoner = api.GetSummoner(Region.eune, UserAccount.GetSummoner());
                liga = summoner.GetLeagues();

                gry = summoner.GetRecentGames();
                foreach (Game game in gry)
                {
                    listaId.Add(game.ChampionId);
                }
                
                int id = listaId.GroupBy(i => i).OrderByDescending(g => g.Count()).Take(1).Select(g => g.Key).First();
                MostPlayedChampion = StaticApi.GetChampion(Region.eune, id).Name +" "+ StaticApi.GetChampion(Region.eune, id).Title;
                MostPlayedChampionImage = "https://opgg-static.akamaized.net/images/lol/champion/" + StaticApi.GetChampion(Region.eune, id).Key + ".png";

            }
            catch (RiotSharpException ex)
            {
                //Summoner doesnt exist or server error
            }
            catch (Newtonsoft.Json.JsonSerializationException exx) { /*when failed, cannot serialize/deserialize status.failed to string/int64/?*/}
        }

        public async Task GetLeagues()
        {
            await GetSumm();

            if (liga != null)
            {
                if (liga.Count >= 1)
                {
                    SummonerBorder = "http://opgg-static.akamaized.net/images/borders2/" + liga[0].Tier.ToString().ToLower() + ".png";
                    SoloQ = "Solo Queue: " + liga[0].Tier + " " + liga[0].Entries[0].Division;
                    SoloQMedal = "https://opgg-static.akamaized.net/images/medals/" + liga[0].Tier.ToString().ToLower() + "_" + RomanToIntConverter(liga[0].Entries[0].Division) + ".png";

                }
                if (liga.Count >= 2)
                {
                    FlexQ = "Flex Queue: " + liga[1].Tier + " " + liga[1].Entries[0].Division;
                    FlexQMedal = "https://opgg-static.akamaized.net/images/medals/" + liga[1].Tier.ToString().ToLower() + "_" + RomanToIntConverter(liga[1].Entries[0].Division) + ".png";

                }

                if (liga.Count == 3)
                {
                    TT = "3v3: " + liga[2].Tier + " " + liga[2].Entries[0].Division;
                    TTMedal = "https://opgg-static.akamaized.net/images/medals/" + liga[2].Tier.ToString().ToLower() + "_" + RomanToIntConverter(liga[2].Entries[0].Division) + ".png";

                }
            }
            //return liga;
        }
        public async void CheckSummonerFromDatabase()
        {

            ErrorText = "Wait...";

            await CheckTournaments();

            var values = new Dictionary<string, string>
               {
                  { "post_user", UserAccount.LoggedInUsername },
               };

            var content = new FormUrlEncodedContent(values);

            try
            {
                HttpClient client = new HttpClient();
                await GetLeagues();
                ImageStrings();

                SoloQ = this.SoloQ;
                FlexQ = this.FlexQ;
                TT = this.TT;

                var response = await client.PostAsync("https://adammak2342.000webhostapp.com/account.php", content);
                var responseString = await response.Content.ReadAsStringAsync();
                UserAccount.SetSummoner(responseString);
                SummonerName = responseString;
                Summoner = responseString;
                ErrorText = "";

            }
            catch (HttpRequestException e) { ErrorText = e.StackTrace; }
        }
        public async Task CheckTournaments()
        {
            ErrorText = "Sprawdzanie zapisanych turniejów...";
            var values = new Dictionary<string, string>
               {
                  { "post_user", UserAccount.LoggedInUsername },
               };

            var content = new FormUrlEncodedContent(values);

            try
            {
                HttpClient client = new HttpClient();

                var response = await client.PostAsync("https://adammak2342.000webhostapp.com/CheckTournaments.php", content);
                var responseString = await response.Content.ReadAsStringAsync();
                UserAccount.Enroll(responseString);

                if (UserAccount.EnrolledTournament == "") Enrolled = "You are not enrolled.";
                if (UserAccount.EnrolledTournament != "") Enrolled = "Participant of: " + UserAccount.EnrolledTournament;
                if (Enrolled == "You are not enrolled.") TournamentCode = "";
                if (Enrolled != "You are not enrolled.") TournamentCode = "EUNE04383-4e59af6f-c21d-482a-8ad1-05e27b3b0f41";

                ErrorText = "";

            }
            catch (HttpRequestException e) { ErrorText = e.StackTrace; }
        }
        private void notification()
        {
            ShowToastNotification("Nowa rotacja bohaterów", "ka¿dy wtorek", 2);
        }
        private void showRotation()
        {
            List<Champion> listachampow = api.GetChampions(Region.eune,true);
            for (int i = 0; i <= listachampow.Count(); i++)
             Debug.WriteLine(StaticApi.GetChampion(Region.eune, (int)listachampow[i].Id).Name);
            //ShowToastNotification("Stan powietrza", selectedStation.HumanIndex, 10);
            UpdateTile("Dzisiejsza rotacja: " );
        }
        private void ShowToastNotification(string title, string stringContent, int time)
        {
            var ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText04);
            var toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(title));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(stringContent));
            var toastNode = toastXml.SelectSingleNode("/toast");
            var audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            var toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(time);
            ToastNotifier.Show(toast);
        }
        public void UpdateTile(string infoString)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            var tileXml =
                TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text01);

            var tileAttributes = tileXml.GetElementsByTagName("text");
            tileAttributes[0].AppendChild(tileXml.CreateTextNode(infoString));
            var tileNotification = new TileNotification(tileXml);

            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }
    }
}

