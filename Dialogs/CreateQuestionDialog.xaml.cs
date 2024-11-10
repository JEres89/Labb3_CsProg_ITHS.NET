using Labb3_CsProg_ITHS.NET.Models;
using Labb3_CsProg_ITHS.NET.ViewModels;
using Labb3_CsProg_ITHS.NET.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Labb3_CsProg_ITHS.NET.Dialogs;
/// <summary>
/// Interaction logic for CreatePackDialog.xaml
/// </summary>
public partial class CreateQuestionDialog : Window
{

    public RelayCommand SaveCommand { get; set; }
    public Question SelectedQuestion { get; set; }
    public CreateQuestionDialog(RelayCommand createCommand)
	{
		SaveCommand = new RelayCommand(question =>
		{
			createCommand.Execute(question);
			DialogResult = true;
		});

		SelectedQuestion = new Question();
		DataContext = this;

		Closing += (_, _) => SelectedQuestion = null;

		InitializeComponent();
	}

}
