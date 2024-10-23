using Labb3_CsProg_ITHS.NET.Models;
using System.Windows.Controls;
using System.Windows.Media;

namespace Labb3_CsProg_ITHS.NET.ViewModels
{
	public class CreatePackViewModel : ViewModelBase
    {
		private string _name = string.Empty;
		private Difficulty _difficulty = Difficulty.Easy;
		private uint _timeLimit = 10;
		private bool _isNameValid = false;

		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				OnPropertyChanged();
				IsNameValid = !string.IsNullOrWhiteSpace(_name);
			}
		}
        internal Difficulty Difficulty { get; }
        public double DifficultyDouble
		{
			get => (double)_difficulty;
			set
			{
				_difficulty = (Difficulty)value;
				OnPropertyChanged();
				OnPropertyChanged("DifficultyName");
			}
		}
		public string DifficultyName => _difficulty.ToString();

		public uint TimeLimit
		{
			get => _timeLimit;
			set
			{
				_timeLimit = value;
				OnPropertyChanged();
			}
		}

		public bool IsNameValid
		{
			get => _isNameValid;
			set
			{
				_isNameValid = value;
				OnPropertyChanged();
			}
		}

		public SolidColorBrush NameColor => IsNameValid ? Brushes.Black : Brushes.Red;

		public RelayCommand CreateCommand { get; }

		public CreatePackViewModel(RelayCommand createCommand)
		{
			CreateCommand = createCommand;

		}

		public DoubleCollection DiffSliderValues
		{
			get
			{
				DoubleCollection result = new();
				foreach (var item in Enum.GetValues<Difficulty>())
				{
					result.Add((double)item);
				}
				Minimum = result[0];
				Maximum = result[^1];
				return result;
			}
		}

        public double Minimum { get; private set; }
        public double Maximum { get; private set; }


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