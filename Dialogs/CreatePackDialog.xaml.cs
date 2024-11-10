using Labb3_CsProg_ITHS.NET.ViewModels;
using Labb3_CsProg_ITHS.NET.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Labb3_CsProg_ITHS.NET.Dialogs;
/// <summary>
/// Interaction logic for CreatePackDialog.xaml
/// </summary>
public partial class CreatePackDialog : Window
{

	public CreatePackDialog(RelayCommand createCommand)
	{
		var newCreateCommand = new RelayCommand(pack =>
		{
			createCommand.Execute(pack);
			DialogResult = true;
		});
		var cancelCommand = new RelayCommand(_ => DialogResult = false);
		DataContext = new ConfigurePackViewModel(newCreateCommand, cancelCommand);

		Closing += (_, _) => DataContext = null;

		InitializeComponent();
	}

}
