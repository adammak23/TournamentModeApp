using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using WindowsApp2.Models;
using WindowsApp2.ViewModels.Commands;
using System.Net.Http;
using Template10.Utils;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.ComponentModel;//new
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Notifications;
using RiotSharp.ChampionEndpoint;
using RiotSharp;
using Windowsapp2.Services;
using System.Net.NetworkInformation;

namespace WindowsApp2.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private static readonly HttpClient client = new HttpClient();
        public ICommand TryLogin { get; set; }
        public ICommand TryLogout { get; set; }
        public ICommand TryRegister { get; set; }
        private string _BusyText = "You have no internet connection...";
        public string BusyText
        {
            get { return _BusyText; }
            set
            {
                Set(ref _BusyText, value);
                _ShowBusyCommand.RaiseCanExecuteChanged();
            }
        }

        DelegateCommand _ShowBusyCommand;
        public DelegateCommand ShowBusyCommand
            => _ShowBusyCommand ?? (_ShowBusyCommand = new DelegateCommand(async () =>
            {
                Views.Busy.SetBusy(true, _BusyText);
                await Task.Delay(5000);
                Views.Busy.SetBusy(false);
            }, () => !string.IsNullOrEmpty(BusyText)));


    private string errorText;
        public string ErrorText
        {
            get { return errorText; }
            set { errorText = value; RaisePropertyChanged("ErrorText"); }
        }
        private string errorTag;
        public string ErrorTag
        {
            get { return errorTag; }
            set { errorTag = value; RaisePropertyChanged("ErrorTag"); }
        }
        private TextBox loginTextBox;
        public TextBox LoginTextBox { get { return loginTextBox; } set { loginTextBox = value; RaisePropertyChanged("LoginTextBox"); } }

        private bool ProgressBarIsIndeterminate = false;
        public bool ProgressBar { get { return ProgressBarIsIndeterminate; } set { ProgressBarIsIndeterminate = value; RaisePropertyChanged("ProgressBar"); } }

        private Visibility loginbuttonsvisibility = Visibility.Visible;
        public Visibility LoginButtonsVisibility { get { return loginbuttonsvisibility; } set { loginbuttonsvisibility = value; RaisePropertyChanged("LoginButtonsVisibility"); } }

        private Visibility logoutbuttonsvisibility = Visibility.Collapsed;
        public Visibility LogoutButtonsVisibility { get { return logoutbuttonsvisibility; } set { logoutbuttonsvisibility = value; RaisePropertyChanged("LogoutButtonsVisibility"); } }

        private string password;
        public string Password { get { return password; } set { password = value; RaisePropertyChanged("Password"); } }
        private bool isinternetconnected;
        public bool isInternetConnected { get { return isinternetconnected; } set { isinternetconnected = value; RaisePropertyChanged("isInternetConnected"); } }
        RiotApi api = Api.GetApi();
        StaticRiotApi StaticApi = Api.GetStaticApi();



        public MainPageViewModel()
        {

            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            TileService.ShowToastNotification("Nowa rotacja bohaterów", "w najbli¿szy wtorek", 3);
            showRotation();
            Login = "";
            this.TryLogin = new Command(AccessTheWebAsync);
            this.TryLogout = new Command(LogOut);
            this.TryRegister = new Command(Register);
            this.LoginTextBox = new TextBox();

        }
        string _Login = "";
        public string Login { get { return _Login; } set { Set(ref _Login, value); } }

        // is htis even needed? xd
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.Any())
            {
                Login = suspensionState[nameof(Login)]?.ToString();
            }
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

        /// ////////////////////


        /// /////////////////////////
        public void GotoDetailsPage() =>
            NavigationService.NavigateAsync(typeof(Views.DetailPage), Login);

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);

        public async void LogOut()
        {

            ProgressBar = true;
            await Task.Delay(2000);
            ProgressBar = false;
            UserAccount.LogOut();
            ErrorText = "You have beed logged out.";
            ErrorTag = "LoggedOut";
            LoginButtonsVisibility = Visibility.Visible;
            LoginTextBox.Visibility = Visibility.Visible;

            LogoutButtonsVisibility = Visibility.Collapsed;

            Views.Shell.HamburgerMenu.IsFullScreen = true;
        }


        public async void AccessTheWebAsync()
        {

            isInternetConnected = NetworkInterface.GetIsNetworkAvailable();
            Debug.WriteLine(isInternetConnected);

            ErrorText = "Wait...";
            ProgressBar = true;
            var values = new Dictionary<string, string>
            {
               { "post_user", LoginTextBox.Text },
               { "post_pass", Password }
            };

            var content = new FormUrlEncodedContent(values);
            try
            {
                var response = await client.PostAsync("https://adammak2342.000webhostapp.com/login.php", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (responseString == "")
                {
                    ProgressBar = false;
                    ErrorText = "User doesn't exist.";
                }
                else if (responseString == "login-SUCCESS")
                {
                    UserAccount.LogIn(LoginTextBox.Text);
                    ErrorText = "Logged in as " + UserAccount.LoggedInUsername;
                    ErrorTag = "LoggedIn";
                    ProgressBar = false;
                    LoginTextBox.Visibility = Visibility.Collapsed;
                    LoginButtonsVisibility = Visibility.Collapsed;

                    LogoutButtonsVisibility = Visibility.Visible;


                    Views.Shell.HamburgerMenu.IsFullScreen = false;
                }
                else
                {
                    ProgressBar = false;
                    if (responseString.Contains("<br")) { AccessTheWebAsync(); }
                    else ErrorText = responseString;
                }
            }
            catch (HttpRequestException e) { errorText = "httpRequestException" /*e.StackTrace*/; }
        }
        public async void Register()
        {

            ErrorText = "Wait...";
            ProgressBar = true;

            var values = new Dictionary<string, string>
            {
               { "post_user", LoginTextBox.Text },
               { "post_pass", Password }
            };
            var content = new FormUrlEncodedContent(values);
            try
            {
                var response = await client.PostAsync("https://adammak2342.000webhostapp.com/register.php", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (responseString == "User has been created!")
                {
                    ErrorText = "Registered as " + LoginTextBox.Text;
                    UserAccount.LogIn(LoginTextBox.Text);
                    ErrorTag = "LoggedIn";
                    ProgressBar = false;
                    LoginTextBox.Visibility = Visibility.Collapsed;
                    LoginButtonsVisibility = Visibility.Collapsed;


                    LogoutButtonsVisibility = Visibility.Visible;

                    Views.Shell.HamburgerMenu.IsFullScreen = false;
                }

                else
                {
                    ProgressBar = false;
                    ErrorText = responseString; // "There is problem with connection";
                }
            }
            catch (HttpRequestException e) { ErrorText = "HttpRequestException" /*e.StackTrace*/; }
        }

        private void showRotation()
        {
            string rotacja = "";
            try
            {
                List<Champion> listachampow = api.GetChampions(Region.eune, true);
            
                for (int i = 0; i < listachampow.Count(); i++)
                {
                    rotacja += StaticApi.GetChampion(Region.eune, (int)listachampow[i].Id).Name + " ";
                }
                TileService.ShowToastNotification("Dzisiejsza rotacja", rotacja, 10);
                TileService.UpdateTile(rotacja);
            }
            catch (RiotSharpException e) { ErrorText = "Coœ posz³o nie tak"; }
        }
       
    }
}

