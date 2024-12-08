using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfClientApp.ViewModels
{
    //в конструктор AsyncRelayCommand будем передавать лямбда-выражением действия команды,
    //второй параметр необязательный - делаем по умолчанию кнопку активной
    public class AsyncRelayCommand: ICommand
    {        
        private readonly Func<object, Task> execute;
        private readonly Func<object, bool> canExecute;

        public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || CanExecute(parameter);
        }

        public async void Execute(object? parameter)
        {
            await execute(parameter);
        }
    }
}

