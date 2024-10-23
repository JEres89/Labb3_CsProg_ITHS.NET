using Labb3_CsProg_ITHS.NET.Dialogs;
using Labb3_CsProg_ITHS.NET.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Labb3_CsProg_ITHS.NET.ViewModels;
internal class ConfigurationViewModel : ViewModelBase
{
    public ObservableCollection<QuestionPack> Packs { get; set; } = new();
    public QuestionPack? SelectedPack { get; set; }

    public ObservableCollection<Question>? Questions { get; set; }
    public Question? SelectedQuestion { get; set; }

    public RelayCommand NewPackCommand { get; }
    public RelayCommand DeletePackCommand { get; }

	public RelayCommand NewQuestionCommand { get; }
	public RelayCommand DeleteQuestionCommand { get; }

	public ConfigurationViewModel()
    {
        NewPackCommand = new(pack => CreatePack(pack));
		DeletePackCommand = new(_ => DeletePack(), _ => SelectedPack != null);
		NewQuestionCommand = new(_ => CreateQuestion(), _ => SelectedPack != null);
		DeleteQuestionCommand = new(_ => DeleteQuestion(), _ => SelectedPack != null && SelectedQuestion != null);
	}

    private void SelectPack(QuestionPack pack)
	{
		SelectedPack = pack;
		SelectedQuestion = null;
		Questions = new(pack.Questions);
		
	}
	private void CreatePack(object? viewModel)
    {
		if(viewModel is CreatePackViewModel createPackViewModel)
		{
			var newPack = new QuestionPack(createPackViewModel.Name, createPackViewModel.Difficulty, createPackViewModel.TimeLimit);
			Packs.Add(newPack);
			SelectedPack = newPack;
			SelectedQuestion = null;
		}
		// View behavior

		//		var createDialog = new CreatePackDialog();
		//      if (createDialog.ShowDialog()!.Value)
		//      {
		//          if(createDialog.DataContext is QuestionPack pack)
		//			{
		//				Packs.Add(pack);
		//				SelectedPack = pack;
		//              SelectedQuestion = null;
		//			}
		//		}
	}

	private void DeletePack()
    {
        if(SelectedPack == null) return;

        Packs.Remove(SelectedPack);
        SelectedPack = null;
    }

    private void CreateQuestion()
    {
        var newQuestion = new Question();
		SelectedPack!.Questions.Add(newQuestion);
		SelectedQuestion = newQuestion;
	}

	private void AddQuestion()
	{
		SelectedPack!.AddQuestion(SelectedQuestion!);
	}
	private void DeleteQuestion()
	{
		SelectedPack!.DeleteQuestion(SelectedQuestion!);
		SelectedQuestion = null;
	}
}