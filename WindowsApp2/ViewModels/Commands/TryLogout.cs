using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using WindowsApp2.ViewModels;

namespace WindowsApp2.ViewModels.Commands
{
    public class TryLogout : ICommand
    {
        public MainPageViewModel ViewModel { get; set; }

        public TryLogout(MainPageViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            ViewModel.LogOut(parameter as Page);
        }
    }
}