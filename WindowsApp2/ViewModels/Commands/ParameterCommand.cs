using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace WindowsApp2.ViewModels.Commands
{
    public class ParameterCommand : ICommand
    {
        private ParameterDelegate _action;
        public string _stringvalue;

        public ParameterCommand(ParameterDelegate action, string stringvalue)
        {
            _action = action;
            _stringvalue = stringvalue;
            Debug.WriteLine(_stringvalue);

        }

        public event EventHandler CanExecuteChanged;


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action.Invoke(_stringvalue);
        }

    }

    public delegate void ParameterDelegate(string parameter);
}