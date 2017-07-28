using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ScottCognitiveApp.Models;

namespace ScottCognitiveApp.Common
{
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;

        private Func<bool> _canExecute;

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
            _execute = execute;
        }
        


        public RelayCommand(Action<object> execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this._execute = execute;
            this._canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute();
        }

        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                this._execute(parameter);
            }
            else
            {
                this._execute("hello");
            }
            
        }

        public void OnCanExecuteChanged()
        {
            var handler = this.CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

    }
}
