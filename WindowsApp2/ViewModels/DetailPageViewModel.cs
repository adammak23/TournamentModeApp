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


namespace WindowsApp2.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {
        public DetailPageViewModel()
        {
            
        }

        public string server = "eune";
        public string summoner = "carammbaa";
        public string SummonerIconImageString
        {
            get { return "http://avatar.leagueoflegends.com/" + server + "/" + summoner + ".png"; }
            //http://static.lolskill.net/img/champions/64/karma.png mini 64x64 https://opgg-static.akamaized.net/images/lol/champion/Morgana.png
            //http://static.lolskill.net/img/tiers/192/goldIII.png lub https://opgg-static.akamaized.net/images/medals/gold_3.png
            //http://opgg-static.akamaized.net/images/borders2/platinum.png
        }

        private string _Login = "Default";
        public string Login { get { return _Login; } set { Set(ref _Login, value); } }

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
        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }

    }
}

