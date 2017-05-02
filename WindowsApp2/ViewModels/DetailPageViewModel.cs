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

using RiotSharp;
using RiotSharp.StatsEndpoint;
using RiotSharp.SummonerEndpoint;
using RiotSharp.LeagueEndpoint;
using System.Net.Http;
using Windows.UI.Xaml;
using System.ComponentModel;

namespace WindowsApp2.ViewModels
{
    [Newtonsoft.Json.JsonArrayAttribute]//not needed?
    public class DetailPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public AddSummoner AddSummoner { get; set; }
        public RefreshSummoner RefreshSummoner { get; set; }
        public DetailPageViewModel()
        {
           
            Refresh();
            Username = UserAccount.LoggedInUsername;
            AddSummoner = new AddSummoner(this);
            RefreshSummoner = new RefreshSummoner(this);
            GetLeagues();
            ImageStrings();
            
        }
        public string Username = "Empty Username";
        public string server = "eune";
        public string SummonerName = UserAccount.GetSummoner();
        public string SoloQ = "Solo Queue: Unranked";
        public string SoloQMedal = "http://static.lolskill.net/img/tiers/192/unranked.png";
        public string FlexQ = "Flex Queue: Unranked";
        public string FlexQMedal = "http://static.lolskill.net/img/tiers/192/unranked.png";
        public string TT = "3v3: Unranked";
        public string TTMedal = "http://static.lolskill.net/img/tiers/192/unranked.png";
        RiotApi api = Api.GetApi();
        public string SummonerIcon;
        public string SummonerBorder = "none";
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
            //http://static.lolskill.net/img/champions/64/karma.png mini 64x64 https://opgg-static.akamaized.net/images/lol/champion/Morgana.png
            //http://static.lolskill.net/img/tiers/192/goldIII.png lub https://opgg-static.akamaized.net/images/medals/gold_3.png
            //http://opgg-static.akamaized.net/images/borders2/platinum.png
        }
        /*

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            Login = (suspensionState.ContainsKey(nameof(Login))) ? suspensionState[nameof(Login)]?.ToString() : parameter?.ToString();
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

        public async void AddingSummoner(Page grid)
        {
            TextBlock block = grid.FindName("Error2") as TextBlock;
            TextBox Summoner = grid.FindName("Summoner") as TextBox;

            block.Text = "Wait...";

               var values = new Dictionary<string, string>
               {
                  { "post_user", UserAccount.LoggedInUsername },
                  { "post_summ", Summoner.Text }
               };

               var content = new FormUrlEncodedContent(values);

            try
            {
                HttpClient client = new HttpClient();
                var response = await client.PostAsync("https://adammak2342.000webhostapp.com/AddSummoner.php", content);
                var responseString = await response.Content.ReadAsStringAsync();
                UserAccount.SetSummoner(Summoner.Text);
                SummonerName = Summoner.Text;
                block.Text = responseString; 
            }
            catch (HttpRequestException e) { block.Text = e.StackTrace; }
        }
        public async void CheckSummonerFromDatabase(Page grid)
        {
            TextBlock block = grid.FindName("Error2") as TextBlock;
            TextBlock Summoner = grid.FindName("SummonerName") as TextBlock;

            block.Text = "Wait...";

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
                SummonerName = responseString;
                Summoner.Text = responseString;
                block.Text = responseString;
            }
            catch (HttpRequestException e) { block.Text = e.StackTrace; }
        }

        public static async void Refresh()
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
                //SummonerName = responseString;
            }
            catch (HttpRequestException e) { }
        }

                public List<League> GetSumm()
        {
            try
            {
                Refresh();
                SummonerName = UserAccount.GetSummoner();
                System.Diagnostics.Debug.WriteLine();
                
                summoner = api.GetSummoner(Region.eune, SummonerName);
                liga = summoner.GetLeagues();
                
            }
            catch (RiotSharpException ex)
            {
                 //Summoner doesnt exist or server error
            }
            catch (Newtonsoft.Json.JsonSerializationException exx) { /*when failed, cannot serialize/deserialize status.failed to string/int64/?*/}
            return liga;
        }

        public List<League> GetLeagues()
        {
            GetSumm();

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
            return liga;
        }
    }
}

