using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace WindowsApp2.ViewModels.Commands
{
    public class RefreshSummoner : ICommand
    {
        public DetailPageViewModel ViewModel { get; set; }

        public RefreshSummoner(DetailPageViewModel viewModel)
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
            ViewModel.CheckSummonerFromDatabase(parameter as Page);
        }
    }
}