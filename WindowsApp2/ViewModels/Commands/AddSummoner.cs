using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace WindowsApp2.ViewModels.Commands
{
    public class AddSummoner : ICommand
    {
        public DetailPageViewModel ViewModel { get; set; }

        public AddSummoner(DetailPageViewModel viewModel)
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
            ViewModel.AddingSummoner(parameter as Page);
        }
    }
}