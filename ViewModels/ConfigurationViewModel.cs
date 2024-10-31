using Labb3_CsProg_ITHS.NET.Dialogs;
using Labb3_CsProg_ITHS.NET.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Labb3_CsProg_ITHS.NET.ViewModels;

/// <summary>
/// Coded as if the <see cref="QuestionPack"/>s are stored in a database or other domain model. Therefore I do not make all edits directly to those objects, only when explicitly saving changes.
/// </summary>
internal class ConfigurationViewModel : ViewModelBase
{
	private Question? _selectedQuestion;
	private QuestionPackVariant? _selectedPack;

	public ObservableCollection<DomainQuestionPack> Packs { get; private set; } = new();
	public ObservableCollection<NewQuestionPack> NewPacks { get; private set; } = new();
	public ObservableCollection<ModifiedQuestionPack> ModifiedPacks { get; private set; } = new();
	public ObservableCollection<DeletedQuestionPack> DeletedPacks { get; private set; } = new();

	public QuestionPackVariant? SelectedPack
	{
		get => _selectedPack;
		set
		{
			_selectedPack = value; 
			SelectedQuestion = null;
			OnPropertyChanged();
			OnPropertyChanged(nameof(Questions));
			CanExecutePackChanged.Invoke();
		}
	}

	public ObservableCollection<Question>? Questions => SelectedPack?.Questions;

	public Question? SelectedQuestion 
	{ 
		get => _selectedQuestion;
		set
		{
			_selectedQuestion = value;
			OnPropertyChanged();
			CanExecuteQuestionChanged.Invoke();
		}
	}

	public RelayCommand NewPackCommand { get; }
	/// <summary>
	/// Use when changing the name, difficulty, or time limit of a pack.
	/// </summary>
	public RelayCommand EditPackCommand { get; }
    public RelayCommand DeletePackCommand { get; }
    public RelayCommand UndoPackChangesCommand { get; }

	public RelayCommand NewQuestionCommand { get; }
	public RelayCommand EditQuestionCommand { get; }
	public RelayCommand DeleteQuestionCommand { get; }

	private event Action CanExecutePackChanged;
	private event Action CanExecuteQuestionChanged;

	public ConfigurationViewModel()
    {
        NewPackCommand = new(
			pack => CreatePack(pack));
		//CanExecutePackChanged += NewPackCommand.RaiseCanExecuteChanged;

		EditPackCommand = new(
			_ => EditPack(), 
			_ => SelectedPack is not null);
		CanExecutePackChanged += EditPackCommand.RaiseCanExecuteChanged;

		DeletePackCommand = new(
			_ => DeletePack(), 
			_ => SelectedPack is not null or DeletedQuestionPack);
		CanExecutePackChanged += DeletePackCommand.RaiseCanExecuteChanged;

		UndoPackChangesCommand = new(
			_ => UndoPackChanges(),
			_ => SelectedPack is ModifiedQuestionPack or DeletedQuestionPack);

		NewQuestionCommand = new(
			question => CreateQuestion(question), 
			_ => SelectedPack is not null);
		CanExecutePackChanged += NewQuestionCommand.RaiseCanExecuteChanged;

		EditQuestionCommand = new(
			_ => EditQuestion(),
			_ => SelectedQuestion is not null);
		CanExecuteQuestionChanged += EditQuestionCommand.RaiseCanExecuteChanged;

		DeleteQuestionCommand = new(
			_ => DeleteQuestion(), 
			_ => SelectedQuestion is not null);
		CanExecuteQuestionChanged += DeleteQuestionCommand.RaiseCanExecuteChanged;

#if DEBUG
		for(uint i = 0; i < 6; i++)
		{
			Packs.Add(
				new(new QuestionPack($"Question Pack {i}", (Difficulty)(i%3), 5*i, MockQuestions())));
		}

	}

	private static int questionNum = 0;
	private static List<Question> MockQuestions()
	{
		var questions = new List<Question>();
		for(int i = questionNum; i < questionNum + 3; i++)
		{
			questions.Add(new Question($"Question {i}", "Answer", new[] { "Wrong", "Wrong", "Wrong" }));
		}
		questionNum += 3;
		return questions;
	}
#else
	}
#endif

	//private void SelectPack(QuestionPack pack)
	//{
	//	if(SelectedPack != null && !Packs.Contains(SelectedPack))
	//	{
	//		SelectedPack.Questions = new(Questions!);
	//	}

	//	SelectedPack = pack;
	//	SelectedQuestion = null;
	//	Questions = new(pack.Questions);
	//}

	private void CreatePack(object? packModel)
	{
		if (packModel is not NewQuestionPack newPack) return;

		NewPacks.Add(newPack);
		SelectedPack = newPack;
	}

	/// <summary>
	/// 
	/// </summary>
	private void EditPack()
	{
		if (SelectedPack == null) return;

		switch (SelectedPack)
		{
			case NewQuestionPack newPack:
			case ModifiedQuestionPack modifiedPack:
				SelectedQuestion = null;
				break;

			case DeletedQuestionPack deletedPack:
				DeletedPacks.Remove(deletedPack); 
				var restoredPack = new ModifiedQuestionPack(deletedPack.DomainPack);
				ModifiedPacks.Add(restoredPack);
				SelectedPack = restoredPack;
				SelectedQuestion = null;
				break;

			case DomainQuestionPack domainPack:
				StartModifyPack(domainPack);
				break;

			default:
				break;
		}
	}

	private void DeletePack()
    {
        if(SelectedPack == null) return;

		switch(SelectedPack)
		{
			case NewQuestionPack newPack:
				NewPacks.Remove(newPack);
				DeletedPacks.Add(new DeletedQuestionPack(newPack));
				SelectedPack = null;
				break;

			case ModifiedQuestionPack modifiedPack:
				ModifiedPacks.Remove(modifiedPack);
				Packs.Add(new(modifiedPack));
				SelectedPack = null;
				break;

			case DomainQuestionPack domainPack:
				Packs.Remove(domainPack);
				DeletedPacks.Add(new DeletedQuestionPack(domainPack));
				SelectedPack = null;
				break;

			default:
				break;
		}
	}
	private void UndoPackChanges()
	{
		switch(SelectedPack)
		{
			case ModifiedQuestionPack modifiedPack:
				ModifiedPacks.Remove(modifiedPack);
				Packs.Add(new(modifiedPack));
				SelectedPack = modifiedPack;
				break;

			case DeletedQuestionPack deletedPack:
				DeletedPacks.Remove(deletedPack);
				Packs.Add(new(deletedPack));
				SelectedPack = deletedPack;
				break;

			default:
				break;
		}
	}

	private void StartModifyPack(DomainQuestionPack packToModify)
	{
        if(packToModify == null) return;

		Packs.Remove(packToModify);
		var modifiedPack = new ModifiedQuestionPack(packToModify);
		ModifiedPacks.Add(modifiedPack);
		SelectedPack = modifiedPack;
		SelectedQuestion = null;
	}

	private void StartModifyQuestion()
	{
		if(SelectedPack == null || SelectedQuestion == null) return;
		if(SelectedPack is ModifiedQuestionPack or NewQuestionPack) return;

		int index = Questions!.IndexOf(SelectedQuestion);

		EditPack();

		SelectedQuestion = Questions[index];
	}

	//private void SelectQuestion(Question question)
	//{
	//	SelectedQuestion = question;
	//}

	/// <summary>
	/// Adds a newly created question to the selected packs questions.
	/// </summary>
	/// <param name="questionModel"></param>
	private void CreateQuestion(object? questionModel)
    {
		if (questionModel is not Question question) return;
		EditPack();

		Questions!.Add(question);
		SelectedQuestion = question;
	}

	/// <summary>
	/// Unlocks editing of the selected question.
	/// </summary>s
	/// <param name="questionModel"></param>
	private void EditQuestion()
	{
		if (SelectedQuestion == null) return;

		StartModifyQuestion();
	}

	//private void AddQuestion(object? questionModel)
	//{
	//	if (questionModel is not Question question) return;
	//	EditPack();

	//	Questions!.Add(question);
	//	SelectedQuestion = question;
	//	StartModifyQuestion();
	//}

	private void DeleteQuestion()
	{
		if (SelectedQuestion == null) return;
		StartModifyQuestion();

		Questions!.Remove(SelectedQuestion!);
		SelectedQuestion = null;
	}
}