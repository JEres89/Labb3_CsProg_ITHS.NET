

using Labb3_CsProg_ITHS.NET.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Labb3_CsProg_ITHS.NET.ViewModels;

public class MainWindowsViewModel : ViewModelBase
{
	private ConfigurationViewModel? _configurationViewModel;
	private ConfigurationViewModel? _cachedConfigurationViewModel;
	private PlayerViewModel? _playerViewModel;
	private PlayerViewModel? _cachedPlayerViewModel;
	private int selectedPackIndex = -1;

	private QuestionPack? _selectedPackModel;
	public QuestionPack? SelectedPackModel
	{
		get
		{
			if(_selectedPackModel != null)
				return _selectedPackModel;

			if(selectedPackIndex > -1)
			{
				DomainModel.QuestionPacks.TryGetValue(selectedPackIndex, out var pack);
				return pack;
			}
			return null;
		}
		set
		{
			selectedPackIndex = value?.ID??-1;
			_selectedPackModel = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<QuestionPack> QuestionPacks { get; private set; } = new();

	private void DomainModel_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if(sender is Dictionary<int, QuestionPack> packs && 
			e.Action == NotifyCollectionChangedAction.Reset)
		{
			_selectedPackModel = null;
			selectedPackIndex = -1;
			QuestionPacks = new(packs.Values);
			OnPropertyChanged(nameof(QuestionPacks));
			OnPropertyChanged(nameof(HasQuizes));
			OnPropertyChanged(nameof(HasNoQuizes));
			OnPropertyChanged(nameof(SelectedPackModel));
		}
		else if(sender is QuestionPack pack)
		{
			switch(e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					QuestionPacks.Add(pack);
					break;
				case NotifyCollectionChangedAction.Remove:
					QuestionPacks.Remove(pack);
					break;
				case NotifyCollectionChangedAction.Replace:
					QuestionPacks.Move(QuestionPacks.IndexOf(pack), QuestionPacks.IndexOf(pack));
					break;
				case NotifyCollectionChangedAction.Reset:
					break;
				default:
					break;
			}
			OnPropertyChanged(nameof(HasQuizes));
			OnPropertyChanged(nameof(HasNoQuizes));
		}
	}
	private void ConfSelectedQuestionChanged(object? _, PropertyChangedEventArgs e)
	{
		if(e.PropertyName == nameof(ConfigurationViewModel.SelectedPack))
		{
			_selectedPackModel = null;
			selectedPackIndex = _configurationViewModel!.SelectedPack?.ID??-1;
			OnPropertyChanged(nameof(SelectedPackModel));
		}
	}
	public MainWindowsViewModel()
	{
		DomainModel.CollectionChanged += DomainModel_CollectionChanged;
		LoadQuizesCommand = new(
			_ => {
				DomainModel.Load();
				ConfigModeCommand!.RaiseCanExecuteChanged();
				_configurationViewModel?.DomainModelUpdated();
			});

		ConfigModeCommand = new(
			pack => {
				if(_configurationViewModel == null)
				{
					ConfigViewModel=(_cachedConfigurationViewModel??new(this));
					if(pack is QuestionPack p)
						ConfigViewModel.SelectedPack = ConfigViewModel.Packs.First(dp => dp.ID == p.ID);
				}
				OnPropertyChanged(nameof(IsInNoMode));
			},
			_ => _configurationViewModel == null
			);
		ConfigModeCommand.ListenToSource(this, "ConfigViewModel");

		CloseConfigCommand = new(
			_ =>
			{
				_configurationViewModel!.ClosePackEditCommand.Execute(null);
				_cachedConfigurationViewModel = _configurationViewModel;
				ConfigViewModel = null;
				OnPropertyChanged(null);
			},
			_ => _configurationViewModel != null
			);
		CloseConfigCommand.ListenToSource(this, "ConfigViewModel");

		PlayQuizCommand = new(
			_ => {
				(PlayerViewModel??=(_cachedPlayerViewModel??new(this))).PlayQuiz(selectedPackIndex);
				OnPropertyChanged(nameof(IsInNoMode));
			},
			_ => {
				if((PlayerViewModel?.IsPlaying)??false)
					if((PlayerViewModel?.HasFinished)??false)
						return false;

				if(selectedPackIndex < 0)	
					return false;

				if(_configurationViewModel != null)
					if(_configurationViewModel?.SelectedPack is not DomainQuestionPack) 
						return false;

				return true;
			});
		PlayQuizCommand.ListenToSource(this, 
			nameof(IsPlaying), 
			nameof(ConfigViewModel), 
			nameof(SelectedPackModel));

		StopQuizCommand = new(
			_ => { 
				PlayerViewModel?.StopQuiz();
				_cachedPlayerViewModel = _playerViewModel;
				PlayerViewModel = null;
			},
			_ => PlayerViewModel != null 
			);
		StopQuizCommand.ListenToSource(this, nameof(PlayerViewModel));
	}

	public ConfigurationViewModel? ConfigViewModel
	{
		get => _configurationViewModel;//??(ConfigViewModel=new(this));
		set
		{
			if(_configurationViewModel != null) {
				if(_configurationViewModel != value)
					_configurationViewModel.PropertyChanged -= ConfSelectedQuestionChanged;
				else
					return;
			}
			if(value != null)
			{
				_configurationViewModel = value;
				_configurationViewModel.PropertyChanged += ConfSelectedQuestionChanged;
			}
			else
			{
				if(_configurationViewModel != null) 
					_configurationViewModel.PropertyChanged -= ConfSelectedQuestionChanged;

				_configurationViewModel = value;
			}
			OnPropertyChanged();
			OnPropertyChanged(nameof(IsConfiguring));
		}
	}

	public PlayerViewModel? PlayerViewModel
	{
		get => _playerViewModel;
		set
		{
			_playerViewModel = value;
			OnPropertyChanged(null);
			//OnPropertyChanged(nameof(IsPlaying));
		}
	}

	public bool IsPlaying => _playerViewModel != null && _playerViewModel.IsPlaying;
	public bool IsConfiguring => _configurationViewModel != null;
	public bool IsInNoMode => !IsPlaying && !IsConfiguring;
	public bool HasNoQuizes => QuestionPacks.Count == 0;
	public bool HasQuizes => QuestionPacks.Count > 0;

	public RelayCommand LoadQuizesCommand { get; private set;}
	public RelayCommand ConfigModeCommand { get; private set; }
	public RelayCommand CloseConfigCommand { get; private set; }
	public RelayCommand PlayQuizCommand { get; private set; }
	public RelayCommand StopQuizCommand { get; private set; }

}
