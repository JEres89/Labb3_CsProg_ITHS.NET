using Labb3_CsProg_ITHS.NET.Models;
using System.IO.Packaging;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Labb3_CsProg_ITHS.NET.ViewModels
{
	internal class ConfigurePackViewModel : ViewModelBase
    {
		//private string _name = string.Empty;
		//private Difficulty _difficulty = Difficulty.Easy;
		//private uint _timeLimit = 10;
		private bool _isNameValid = false;

		//private QuestionPack? _original;
        //public NewQuestionPack? NewPack { get; set; }
        public QuestionPackVariant Pack { 
			get; 
			set; }
		public bool IsChanged
		{
			get
			{
				return
					Pack.Name            != Pack.DomainPack.Name ||
					Pack.Difficulty      != Pack.DomainPack.Difficulty ||
					Pack.TimeLimit       != Pack.DomainPack.TimeLimit ||
					Pack.Questions.Count != Pack.DomainPack.Questions.Count;
			}
		}
		//public string Name
		//{
		//	get => Pack.Name;
		//	set
		//	{
		//		Pack.Name = value;
		//		OnPropertyChanged();
		//		//IsNameValid = !string.IsNullOrWhiteSpace(value);
		//		SaveCommand.RaiseCanExecuteChanged();
		//	}
		//}
        //internal Difficulty Difficulty { get; }
  //      public double DifficultyDouble
		//{
		//	get => (double)Pack.Difficulty;
		//	set
		//	{
		//		Pack.Difficulty = (Difficulty)value;
		//		OnPropertyChanged();
		//		OnPropertyChanged("DifficultyName");
		//	}
		//}
		public string DifficultyName => Pack.Difficulty.ToString();

		//public string? TimeLimit
		//{
		//	get => Pack.TimeLimit.ToString();
		//	set
		//	{
		//		if(!uint.TryParse(value, out var timeLimit))
		//		{
		//			//timeLimit = Pack.TimeLimit;
		//			Pack.OnPropertyChanged();
		//			return;
		//		}
		//		Pack.TimeLimit = timeLimit;
		//		//_timeLimit = value!=null? uint.Parse(value) : 0;
		//		//SaveCommand?.RaiseCanExecuteChanged();
		//		//OnPropertyChanged();
		//	}
		//}
		
		public bool IsNameValid => !string.IsNullOrWhiteSpace(Pack.Name);

		public RelayCommand? SaveCommand { get; }
		public RelayCommand? CancelCommand { get; }

		public ConfigurePackViewModel(RelayCommand createCommand, RelayCommand cancelCommand)
		{
			Pack = new NewQuestionPack(string.Empty, Difficulty.Easy, 10);
			SaveCommand = createCommand;
			SaveCommand.SetCanExecute(_ => IsNameValid && Pack.TimeLimit > 0);
			Pack.PropertyChanged += Pack_PropertyChanged;
			CancelCommand = cancelCommand;
		}

        public ConfigurePackViewModel(QuestionPackVariant pack, RelayCommand cancelCommand)
		{
			Pack = pack;
			CancelCommand = cancelCommand;
			//CancelCommand.SetCanExecute(_ => IsChanged);
		}

		private void Pack_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch(e.PropertyName)
			{
				case "Difficulty":
					OnPropertyChanged(nameof(DifficultyName));
					OnPropertyChanged(nameof(IsChanged));
					break;

				case "Name":
				case "TimeLimit":
					SaveCommand?.RaiseCanExecuteChanged();
					OnPropertyChanged(nameof(IsChanged));
					break;

				default:

					break;
			}
		}

		//private void ResetChanges()
		//{
			//Name = _original!.Name;
			//DifficultyDouble = (double)_original.Difficulty;
			//TimeLimit = _original.TimeLimit.ToString();
		//}

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

	[ValueConversion(typeof(uint), typeof(string))]
	public class TimeStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value is uint time)
			{
				return time.ToString();
			}
			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value is string str)
			{
				if(uint.TryParse(str, out uint result))
				{
					return result;
				}
			}
			return 0;
		}
	}

	[ValueConversion(typeof(Difficulty), typeof(double))]

	public class DifficultyConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value is Difficulty diff)
			{
				return (double)diff;
			}
			return 0.0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value is double dbl)
			{
				return (Difficulty)dbl;
			}
			return Difficulty.Easy;
		}
	}
}