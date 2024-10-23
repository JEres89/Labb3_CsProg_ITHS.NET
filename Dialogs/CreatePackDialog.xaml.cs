using Labb3_CsProg_ITHS.NET.ViewModels;
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
		DataContext = new CreatePackViewModel(createCommand);

		InitializeComponent();
	}
}
