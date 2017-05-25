using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Template10.Mvvm;
using WindowsApp2.ViewModels.Commands;
using WindowsApp2.Models;
using Windows.UI.Xaml.Controls;

namespace WindowsApp2.ViewModels
{
    class TournamentViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ICommand GetTournaments { get; set; }

        private string errortext = "";
        public string ErrorText { get { return errortext; } set { errortext = value; RaisePropertyChanged("ErrorText"); } }
        public static string Tournament;
        public static List<TournamentModel> turnieje = new List<TournamentModel>() { new TournamentModel("1v1", "EUNE", 100, 1, "blindpick", "Elimination", "Howling Abyss"), new TournamentModel("3v3 Blindpick BO5", "EUNE", 100, 3, "blindpick", "Best of 5", "Twisted Treeline"), new TournamentModel("5v5 random elimination", "EUNE", 100, 5, "random", "Elimination", "Summoner's Rift")};
        private static int SelectedIndex;


        public TournamentViewModel()
        {
            this.GetTournaments = new Command(Enroll);
        }

        public static int ItemClick(object sender, ItemClickEventArgs e)
        {
            TournamentModel item = (TournamentModel)e.ClickedItem;
            SelectedIndex = turnieje.FindIndex(a => a.Name == item.Name);
            Tournament = turnieje[SelectedIndex].Name;
            return SelectedIndex;
        }

        public async void Enroll()
        {
            UserAccount.Enroll(Tournament);
            ErrorText = "Wait...";

            var values = new Dictionary<string, string>
               {
                  { "post_user", UserAccount.LoggedInUsername },
                  { "post_tour", turnieje[SelectedIndex].Name }
               };

            var content = new FormUrlEncodedContent(values);

            try
            {
                HttpClient client = new HttpClient();
                var response = await client.PostAsync("https://adammak2342.000webhostapp.com/Enroll.php",content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (responseString == "")
                {
                    ErrorText = "Doesn't exist.";
                }

                else
                {
                    ErrorText = responseString;
                }
            }
            catch (HttpRequestException e) { ErrorText = "httpRequestException" /*e.StackTrace*/; }






        }
    }
}
    