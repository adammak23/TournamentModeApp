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


namespace WindowsApp2.ViewModels
{
    class TournamentViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ICommand GetTournaments { get; set; }
        private string errortext = "none";
        public string ErrorText { get { return errortext; } set { errortext = value; RaisePropertyChanged("ErrorText"); } }
        private string tournament;
        public string Tournament { get { return tournament; } set { tournament = value; RaisePropertyChanged("Tournament"); } }
        public static List<TournamentModel> turnieje = new List<TournamentModel>();



        public TournamentViewModel()
        {

            this.GetTournaments = new ParameterCommand(Enroll,Tournament);

            turnieje.Add(new TournamentModel("1v1", "EUNE", 100, 1, "blindpick", "Elimination", "Howling Abyss") { });
            turnieje.Add(new TournamentModel("3v3 Blindpick BO5", "EUNE", 100, 3, "blindpick", "Best of 5", "Twisted Treeline") { });
            turnieje.Add(new TournamentModel("5v5 random elimination", "EUNE", 100, 5, "random", "Elimination", "Summoner's Rift") { });

        }

        public async void Enroll(string Tournament)
        {
            UserAccount.Enroll(this.Tournament);
            ErrorText = "Wait...";

            var values = new Dictionary<string, string>
               {
                  { "post_user", UserAccount.LoggedInUsername },
                  { "post_tour", turnieje[0].Name }
               };

            var content = new FormUrlEncodedContent(values);

            Debug.WriteLine(Tournament);

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
    