using Labb3_CsProg_ITHS.NET.ViewModels;
using System.Windows;

namespace Labb3_CsProg_ITHS.NET.Dialogs;
/// <summary>
/// Interaction logic for CreatePackDialog.xaml
/// </summary>
public partial class CreatePackDialog : Window
{
	public CreatePackDialog()
	{
		DataContext = new CreatePackViewModel();
		InitializeComponent();
	}
}
