//using Labb3_CsProg_ITHS.NET.Models;
using Labb3_CsProg_ITHS.NET.Dialogs;
using Labb3_CsProg_ITHS.NET.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Labb3_CsProg_ITHS.NET.Views;
/// <summary>
/// Interaction logic for ConfigurationView.xaml
/// </summary>
public partial class ConfigurationView : UserControl
{
	public ConfigurationView()
	{
		InitializeComponent();
		DataContext = new ConfigurationViewModel();
	}

	//public void SetQuestionPack(QuestionPack questionPack)
	//{
	//	((ConfigurationViewModel)DataContext).SelectedPack = questionPack;
	//}

	private void NewQuizBtn_Click(object sender, RoutedEventArgs e)
	{
		var newDialog = new CreatePackDialog(((ConfigurationViewModel)DataContext).NewPackCommand);
		newDialog.ShowDialog();
	}
}
