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




        public TournamentViewModel()
        {

            this.GetTournaments = new ParameterCommand(Enroll);

        }

        public async void Enroll(string Tournament)
        {
            UserAccount.Enroll(Tournament);
            ErrorText = "Wait...";

            var values = new Dictionary<string, string>
               {
                  { "post_user", UserAccount.LoggedInUsername },
                  { "post_tour", Tournament }
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
    