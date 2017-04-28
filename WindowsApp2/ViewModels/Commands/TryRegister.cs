using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace WindowsApp2.ViewModels.Commands
{
    public class TryRegister : ICommand
    {
        public MainPageViewModel ViewModel { get; set; }

        public TryRegister(MainPageViewModel viewModel)
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
            this.ViewModel.AccessTheWebAsync(parameter as Page);
        }
    }
}