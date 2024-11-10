

namespace Labb3_CsProg_ITHS.NET.ViewModels;

public class MainWindowsViewModel : ViewModelBase
{
	private ConfigurationViewModel? _configurationViewModel;
	private PlayerViewModel? _playerViewModel;

	public ConfigurationViewModel ConfigViewModel
	{
		get => _configurationViewModel??=new(this);
		set
		{
			_configurationViewModel = value;
			OnPropertyChanged();
		}
	}

	public PlayerViewModel PlayerViewModel
	{
		get => _playerViewModel??=new();
		set
		{
			_playerViewModel = value;
			OnPropertyChanged();
		}
	}

	public RelayCommand PlayQuizCommand { get; private set; }
}
