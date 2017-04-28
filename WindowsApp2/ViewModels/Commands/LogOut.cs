using System;
using Windows.UI.Xaml.Controls;


namespace WindowsApp2.ViewModels.Commands
{
    class LogOut
    {
        public MainPageViewModel ViewModel { get; set; }

        public LogOut(MainPageViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            this.ViewModel.LogOut(parameter as Page);
        }
    }
}