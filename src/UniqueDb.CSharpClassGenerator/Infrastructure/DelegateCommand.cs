namespace UniqueDb.CSharpClassGenerator.Infrastructure
{
    // public class DelegateCommand : DelegateCommand<object>
    // {
    //     public DelegateCommand(Action executeMethod)
    //         : base(o => executeMethod())
    //     {
    //     }
    //
    //     public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
    //         : base(o => executeMethod(), o => canExecuteMethod())
    //     {
    //     }
    // }
    //
    // public class DelegateCommand<T> : ICommand
    // {
    //     private readonly Func<T, bool> _canExecuteMethod;
    //     private readonly Action<T> _executeMethod;
    //     private bool _isExecuting;
    //
    //     public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod = null)
    //     {
    //         if ((executeMethod == null) && (canExecuteMethod == null))
    //         {
    //             throw new ArgumentNullException(nameof(executeMethod), @"Execute Method cannot be null");
    //         }
    //         _executeMethod = executeMethod;
    //         _canExecuteMethod = canExecuteMethod;
    //     }
    //
    //     public event EventHandler CanExecuteChanged
    //     {
    //         add
    //         {
    //             CommandManager.RequerySuggested += value;
    //         }
    //         remove
    //         {
    //             CommandManager.RequerySuggested -= value;
    //         }
    //     }
    //
    //     public void RaiseCanExecuteChanged()
    //     {
    //         CommandManager.InvalidateRequerySuggested();
    //     }
    //
    //     bool ICommand.CanExecute(object parameter)
    //     {
    //         return !_isExecuting && CanExecute((T)parameter);
    //     }
    //
    //     void ICommand.Execute(object parameter)
    //     {
    //         _isExecuting = true;
    //         try
    //         {
    //             RaiseCanExecuteChanged();
    //             Execute((T)parameter);
    //         }
    //         finally
    //         {
    //             _isExecuting = false;
    //             RaiseCanExecuteChanged();
    //         }
    //     }
    //
    //     public bool CanExecute(T parameter)
    //     {
    //         if (_canExecuteMethod == null)
    //             return true;
    //
    //         return _canExecuteMethod(parameter);
    //     }
    //
    //     public void Execute(T parameter)
    //     {
    //         _executeMethod(parameter);
    //     }
    // }
    //
    // public class SerialDelegateCommand : SerialDelegateCommand<object>
    // {
    //     public SerialDelegateCommand(Func<Task> executeMethod) : base(o => executeMethod())
    //     {
    //     }
    //
    //     public SerialDelegateCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod)
    //         : base((o) => executeMethod(), o => canExecuteMethod())
    //     {
    //     }
    // }
    //
    // public class SerialDelegateCommand<T> : ICommand
    // {
    //     private long _isCommandRunning = 0;
    //     private readonly Func<T, bool> _canExecuteMethod;
    //     private readonly Func<T, Task> _executeMethod;
    //     private bool _isExecuting;
    //
    //     public SerialDelegateCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod = null)
    //     {
    //         if ((executeMethod == null) && (canExecuteMethod == null))
    //         {
    //             throw new ArgumentNullException(nameof(executeMethod), @"Execute Method cannot be null");
    //         }
    //         _executeMethod = executeMethod;
    //         _canExecuteMethod = canExecuteMethod;
    //     }
    //
    //     public event EventHandler CanExecuteChanged
    //     {
    //         add
    //         {
    //             CommandManager.RequerySuggested += value;
    //         }
    //         remove
    //         {
    //             CommandManager.RequerySuggested -= value;
    //         }
    //     }
    //
    //     public void RaiseCanExecuteChanged()
    //     {
    //         CommandManager.InvalidateRequerySuggested();
    //     }
    //
    //     public bool CanExecute(object parameter)
    //     {
    //         return CanExecute((T)parameter);
    //     }
    //
    //     public void Execute(object parameter)
    //     {
    //         Execute((T)parameter);
    //     }
    //
    //     public bool CanExecute(T parameter)
    //     {
    //         if (IsExecuting()) return false;
    //
    //         if (_canExecuteMethod == null)
    //             return true;
    //
    //         return _canExecuteMethod(parameter);
    //     }
    //
    //     private bool IsExecuting()
    //     {
    //         return Interlocked.Read(ref _isCommandRunning) == 1;
    //     }
    //
    //     public async Task Execute(T parameter)
    //     {
    //         if (Interlocked.CompareExchange(ref _isCommandRunning, 1, 0) == 0) //If "Not Running", then set to "Running" and enter IF block
    //         {
    //             try
    //             {
    //                 RaiseCanExecuteChanged();
    //                 await _executeMethod(parameter);
    //             }
    //             catch (Exception e)
    //             {
    //                 throw;
    //             }
    //             finally
    //             {
    //                 Interlocked.Exchange(ref _isCommandRunning, 0); //Set back to "Not Running" status.
    //                 RaiseCanExecuteChanged();
    //             }
    //         }
    //     }
    // }
}