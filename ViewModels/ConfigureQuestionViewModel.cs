//using Labb3_CsProg_ITHS.NET.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Media;

//namespace Labb3_CsProg_ITHS.NET.ViewModels
//{
//    public class ConfigureQuestionViewModel : ViewModelBase
//	{
//		private string _name;// = string.Empty;
//		private Difficulty _difficulty;// = Difficulty.Easy;
//		private uint _timeLimit;// = 10;

//		private Action? _onChanged;

//		private void OnFirstChange()
//		{
//			IsChanged = true;
//			CancelCommand?.RaiseCanExecuteChanged();
//			_onChanged = null;
//		}
//		private void OnChange([CallerMemberName] string? name = null)
//		{
//			OnPropertyChanged(name);
//			_onChanged?.Invoke();
//			SaveCommand?.RaiseCanExecuteChanged();
//		}

//		private void SaveChanges()
//		{
//			Pack.Name = Name;
//			Pack.Difficulty = Difficulty;
//			Pack.TimeLimit = TimeLimit;
//			IsChanged = false;
//			CancelCommand?.SetCanExecute(null);
//			CloseCommand?.Execute(null);
//		}

//		/// <summary>
//		/// Dialog form
//		/// </summary>
//		public ConfigureQuestionViewModel(RelayCommand createCommand, RelayCommand cancelCommand)
//		{
//			IsChanged = true;
//			_name = string.Empty;
//			_difficulty = Difficulty.Easy;
//			_timeLimit = 10;

//			Pack = new NewQuestionPack(_name, _difficulty, _timeLimit);
//			SaveCommand = new RelayCommand(
//				_ => {
//					SaveChanges();
//					createCommand.Execute(Pack);
//				},
//				_ => IsNameValid && _timeLimit > 0);
//			CancelCommand = cancelCommand;

//		}

//		/// <summary>
//		/// Edit form
//		/// </summary>
//		public ConfigureQuestionViewModel(QuestionPackVariant pack, RelayCommand cancelCommand)
//		{
//			Pack = pack;
//			Name = pack.Name;
//			Difficulty = pack.Difficulty;
//			TimeLimit = pack.TimeLimit;

//			_onChanged = OnFirstChange;
//			CancelCommand = cancelCommand;
//			CancelCommand.SetCanExecute(_ => IsChanged);
//			SaveCommand = new RelayCommand(
//				_ => SaveChanges(),
//				_ => IsNameValid && _timeLimit > 0);
//		}

//		public QuestionPackVariant Pack { get; set; }

//		public bool IsChanged { get; private set; } = false;
//		public string Name
//		{
//			get => _name;
//			set
//			{
//				_name = value;
//				OnChange();
//			}
//		}
//		public Difficulty Difficulty
//		{
//			get => _difficulty;
//			set
//			{
//				_difficulty = value;
//				OnChange();
//				OnPropertyChanged(nameof(DifficultyName));
//			}
//		}
//		public string DifficultyName => _difficulty.ToString();

//		public uint TimeLimit
//		{
//			get => _timeLimit;
//			set
//			{
//				_timeLimit = value;
//				OnChange();
//			}
//		}

//		public bool IsNameValid => !string.IsNullOrWhiteSpace(_name);

//		public RelayCommand? SaveCommand { get; }
//		public RelayCommand CancelCommand { get; }
//		public RelayCommand? CloseCommand { get; set; }

//		public double Minimum => _diffSliderValues[0];
//		public double Maximum => _diffSliderValues[^1];

//		static ConfigureQuestionViewModel()
//		{
//			DoubleCollection diffSliderVals = new();
//			foreach(var item in Enum.GetValues<Difficulty>())
//			{
//				diffSliderVals.Add((double)item);
//			}
//			_diffSliderValues = diffSliderVals;
//		}
//		private static readonly DoubleCollection _diffSliderValues;
//		public DoubleCollection DiffSliderValues => _diffSliderValues;
//	}
//}