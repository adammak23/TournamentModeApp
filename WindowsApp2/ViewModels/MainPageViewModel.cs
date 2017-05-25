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

namespace WindowsApp2.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private static readonly HttpClient client = new HttpClient();
        public ICommand TryLogin { get; set; }
        public ICommand TryLogout { get; set; }
        public ICommand TryRegister { get; set; }
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





        public MainPageViewModel()
        {

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
                    ErrorText = responseString;// "There is problem with connection, please try again";
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

    }
}

