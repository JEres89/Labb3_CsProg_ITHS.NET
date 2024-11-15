using System.ComponentModel;
using System.Windows.Input;

namespace Labb3_CsProg_ITHS.NET.ViewModels
{
	public class RelayCommand : ICommand
	{
		private Action<object?> _execute;
		private Func<object?, bool>? _canExecute;
		private HashSet<string> _properties = new();

		public RelayCommand(Action<object?> execute, Func<object?,bool>? canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public event EventHandler? CanExecuteChanged;

		public void SetCanExecute(Func<object?, bool>? canExecute) => _canExecute = canExecute;

		public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

		public void Execute(object? parameter) => _execute(parameter);

		internal void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

		//internal void ListenToCommand(RelayCommand command)  
		//{
		//	command.CanExecuteChanged += RaiseCanExecuteChanged;
		//}
		internal void ListenToSource(INotifyPropertyChanged source, params string[] propertyName)  
		{
			source.PropertyChanged += PropertyChanged;
			ListenToPropertyChanged(propertyName);
		}

		internal void ListenToPropertyChanged(params string[] properties)
		{
			foreach(var property in properties)
			{
				ListenToPropertyChanged(property);
			}
		}
		internal void ListenToPropertyChanged(string propertyName) => _properties.Add(propertyName);
		internal void PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if(e.PropertyName == null || _properties.Contains(e.PropertyName))
				RaiseCanExecuteChanged();
		}
	}
}
