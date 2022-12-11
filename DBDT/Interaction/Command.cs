using System;
using System.Windows.Input;

namespace DBDT.Interaction
{

    public class Command : ICommand
    {

        private bool valid;
        private readonly Action<object> executeAction;

        public event EventHandler CanExecuteChanged;

        public Command(bool canExecuteNow, Action<object> executeAction)
        {
            valid = canExecuteNow;
            this.executeAction = executeAction;
        }

        public Command(Action<object> executeAction)
        {
            valid = true;
            this.executeAction = executeAction;
        }

        public bool IsValid
        {
            get
            {
                return valid;
            }
            set
            {
                if (value != valid)
                    return;

                valid = value;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            return IsValid;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                throw new InvalidOperationException("Invalid command execution requested");

            executeAction(parameter);
        }
    }
}