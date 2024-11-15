using Labb3_CsProg_ITHS.NET.Models;
using System.IO.Packaging;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;

namespace Labb3_CsProg_ITHS.NET.ViewModels
{
	public class ConfigurePackViewModel : ViewModelBase
	{
		private string _name;// = string.Empty;
		private Difficulty _difficulty;// = Difficulty.Easy;
		private uint _timeLimit;// = 10;
		//private bool _isNameValid;// = false;

		private Action? _onChanged;

		private void OnFirstChange()
		{
			IsChanged = true;
			CancelCommand?.RaiseCanExecuteChanged();
			_onChanged = null;
		}
		private void OnChange([CallerMemberName] string? name = null)
		{
			OnPropertyChanged(name);
			_onChanged?.Invoke();
			SaveCommand?.RaiseCanExecuteChanged();
		}

		private void SaveChanges()
		{
			Pack.Name = Name;
			Pack.Difficulty = Difficulty;
			Pack.TimeLimit = TimeLimit;
			IsChanged = false;
			CloseCommand?.Execute(null);
		}

		/// <summary>
		/// Dialog form
		/// </summary>
		public ConfigurePackViewModel(RelayCommand createCommand, RelayCommand cancelCommand)
		{
			IsChanged = true;
			_name = string.Empty;
			_difficulty = Difficulty.Easy;
			_timeLimit = 10;

			Pack = new NewQuestionPack(_name, _difficulty, _timeLimit);
			SaveCommand = new RelayCommand(
				_ => {
					SaveChanges();
					createCommand.Execute(Pack); }, 
				_ => IsNameValid && _timeLimit > 0);
			CancelCommand = cancelCommand;

			//Pack.PropertyChanged += Pack_PropertyChanged;
		}

		/// <summary>
		/// Edit form
		/// </summary>
		public ConfigurePackViewModel(QuestionPackVariant pack, RelayCommand closeCommand)
		{
			Pack = pack;
			_name = Name = pack.Name;
			Difficulty = pack.Difficulty;
			TimeLimit = pack.TimeLimit;

			_onChanged = OnFirstChange;
			CancelCommand = closeCommand;
			//CancelCommand.SetCanExecute(_ => IsChanged);
			SaveCommand = new RelayCommand(
				_ => SaveChanges(), 
				_ => IsNameValid && _timeLimit > 0);
			CloseCommand = closeCommand;
		}

		public QuestionPackVariant Pack { get; set; }

		public bool IsChanged { get; private set; } = false;
			//{
			//	return
			//		_name            != Pack.Name ||
			//		_difficulty      != Pack.Difficulty ||
			//		_timeLimit       != Pack.TimeLimit;
			//}
		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				OnChange();
			}
		}
		public Difficulty Difficulty
		{
			get => _difficulty;
			set
			{
				_difficulty = value;
				OnChange();
				OnPropertyChanged(nameof(DifficultyName));
			}
		}
		public string DifficultyName => _difficulty.ToString();

		public uint TimeLimit
		{
			get => _timeLimit;
			set
			{
				_timeLimit = value;
				OnChange();
			}
		}

		public bool IsNameValid => !string.IsNullOrWhiteSpace(_name);

		public RelayCommand? SaveCommand { get; }
		public RelayCommand CancelCommand { get; }
		public RelayCommand? CloseCommand { get; }

		public double Minimum => _diffSliderValues[0];
        public double Maximum => _diffSliderValues[^1];

		static ConfigurePackViewModel()
		{
			DoubleCollection diffSliderVals = new();
			foreach(var item in Enum.GetValues<Difficulty>())
			{
				diffSliderVals.Add((double)item);
			}
			_diffSliderValues = diffSliderVals;
		}
		private static readonly DoubleCollection _diffSliderValues;
		public DoubleCollection DiffSliderValues => _diffSliderValues;


		//private void Pack_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		//{
		//	switch(e.PropertyName)
		//	{
		//		case "Difficulty":
		//			OnPropertyChanged(nameof(DifficultyName));
		//			OnPropertyChanged(nameof(IsChanged));
		//			break;

		//		case "Name":
		//		case "TimeLimit":
		//			SaveCommand?.RaiseCanExecuteChanged();
		//			OnPropertyChanged(nameof(IsChanged));
		//			break;

		//		default:

		//			break;
		//	}
		//}

	}

	public class TimeValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			if (value is string str)
			{
				if (uint.TryParse(str, out uint result))
				{
					if (result > 0)
					{
						return ValidationResult.ValidResult;
					}
				}
			}
			return new ValidationResult(false, "Time limit must be a positive number");
		}
	}
}