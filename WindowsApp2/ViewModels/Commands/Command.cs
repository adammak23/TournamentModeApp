using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WindowsApp2.ViewModels.Commands
{
    public class Command : ICommand
    {
        private Action<string> omg;
        private Action _action;

        public Command(Action<string> omg)
        {
            this.omg = omg;
        }

        public Command(Action action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }
    }


}