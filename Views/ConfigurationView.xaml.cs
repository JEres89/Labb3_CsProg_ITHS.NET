//using Labb3_CsProg_ITHS.NET.Models;
using Labb3_CsProg_ITHS.NET.ViewModels;
using System.Windows.Controls;

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
}
