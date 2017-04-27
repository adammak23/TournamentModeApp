using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using WindowsApp2.ViewModels;

namespace WindowsApp2.ViewModels.LoginCommands
{
    public class TryLogin : ICommand
    {
        //
        private readonly Action<object> _Execute;
        private readonly Predicate<object> _canExecute;
        public TryLogin(Action<object> execute, Predicate<object> canExecute)
        {
            _canExecute = canExecute;
            _Execute = execute;
        }
        public TryLogin(Action<object> execute) : this (execute, null) { }
        //
        public MainPageViewModel ViewModel { get; set; }

        public TryLogin(MainPageViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            ViewModel.AccessTheWebAsync(parameter as Page);
           
        }
    }
}
