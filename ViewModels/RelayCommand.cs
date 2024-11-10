using System.Windows.Input;

namespace Labb3_CsProg_ITHS.NET.ViewModels
{
	public class RelayCommand : ICommand
	{
		private Action<object?> _execute;
		private Func<object?, bool>? _canExecute;
		public RelayCommand(Action<object?> execute, Func<object?,bool>? canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public void SetCanExecute(Func<object?, bool>? canExecute)
		{
			_canExecute = canExecute;
		}

		public event EventHandler? CanExecuteChanged;

		public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

		public void Execute(object? parameter) => _execute(parameter);

		internal void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
