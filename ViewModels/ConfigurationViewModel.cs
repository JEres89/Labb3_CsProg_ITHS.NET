using Labb3_CsProg_ITHS.NET.Dialogs;
using Labb3_CsProg_ITHS.NET.Models;
using System.Collections.ObjectModel;
using System.Runtime.Intrinsics.Arm;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Labb3_CsProg_ITHS.NET.ViewModels;

/// <summary>
/// Coded as if the <see cref="QuestionPack"/>s are stored in a database or other domain model. Therefore I do not make all edits directly to those objects, only when explicitly saving changes.
/// </summary>
public class ConfigurationViewModel : ViewModelBase
{
	private Question? _selectedQuestion;
	private QuestionPackVariant? _selectedPack;
	private ConfigurePackViewModel? _configurePackViewModel;

    public MainWindowsViewModel MainViewModel { get; private set; }

    public ObservableCollection<DomainQuestionPack> Packs { get; private set; } = new();
	public ObservableCollection<NewQuestionPack> NewPacks { get; private set; } = new();
	public ObservableCollection<ModifiedQuestionPack> ModifiedPacks { get; private set; } = new();
	public ObservableCollection<DeletedQuestionPack> DeletedPacks { get; private set; } = new();
	
    public bool IsNotEditingQuiz => ConfigurePackViewModel == null;
    public bool IsEditingQuiz => ConfigurePackViewModel != null;
    //public bool IsViewingQuiz => IsNotEditingQuiz && SelectedPack != null && SelectedQuestion == null;
    public bool IsViewingQuestion => ConfigurePackViewModel == null && SelectedQuestion != null;
    //public bool IsEditingQuestion => ConfigurePackViewModel != null;

    public ConfigurePackViewModel? ConfigurePackViewModel
	{
		get => _configurePackViewModel;
		set
		{
			_configurePackViewModel = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(IsNotEditingQuiz));
			OnPropertyChanged(nameof(IsEditingQuiz));
			//OnPropertyChanged(nameof(IsViewingQuiz));
			OnPropertyChanged(nameof(IsViewingQuestion));
			CanExecutePackChanged.Invoke();
			CanExecuteQuestionChanged.Invoke();
		}
	}
	public QuestionPackVariant? SelectedPack
	{
		get => _selectedPack;
		set
		{
			if(ConfigurePackViewModel != null)
			{
				OnPropertyChanged();
				return;
			}
			_selectedPack = value;
			SelectedQuestion = null;
			OnPropertyChanged();
			OnPropertyChanged(nameof(Questions));
			//OnPropertyChanged(nameof(IsViewingQuiz));
			CanExecutePackChanged.Invoke();
		}
	}

	public ObservableCollection<Question>? Questions => SelectedPack?.Questions;

	public Question? SelectedQuestion 
	{ 
		get => _selectedQuestion;
		set
		{
			if(ConfigurePackViewModel != null)
			{
				OnPropertyChanged();
				return;
			}
			_selectedQuestion = value;
			OnPropertyChanged();
			//OnPropertyChanged(nameof(IsViewingQuiz));
			OnPropertyChanged(nameof(IsViewingQuestion));
			CanExecuteQuestionChanged.Invoke();
		}
	}


	public RelayCommand SaveChangesCommand { get; }

	public RelayCommand? NewQuiz_ViewCommand { get; private set; }
	//public RelayCommand? EditQuiz_ViewCommand { get; set; }
	public RelayCommand? NewQuestion_ViewCommand { get; private set; }
	public RelayCommand? ImportQuestions_ViewCommand { get; private set; }

	public RelayCommand NewPackCommand { get; }
    public RelayCommand ClosePackEditCommand { get; }
	public RelayCommand EditPackCommand { get; }
    public RelayCommand DeletePackCommand { get; }
    public RelayCommand UndoPackChangesCommand { get; }

	public RelayCommand ImportQuestionsCommand { get; }
	public RelayCommand NewQuestionCommand { get; }
	public RelayCommand EditQuestionsCommand { get; }
	public RelayCommand DeleteQuestionCommand { get; }

	private event Action CanExecutePackChanged;
	private event Action CanExecuteQuestionChanged;

	// TODO: move adding listening to RelayCommand.ListenToSource
	public ConfigurationViewModel(MainWindowsViewModel mainVM)
    {

		MainViewModel = mainVM;

		SaveChangesCommand = new(
			pack => {
				ConfigurePackViewModel?.SaveCommand?.Execute(null);
				SaveToDomain(pack);
			},
			_ => NewPacks.Count > 0 || ModifiedPacks.Count > 0 || DeletedPacks.Count > 0);

		NewPackCommand = new(
			pack => CreatePack(pack),
			_ => ConfigurePackViewModel != null);


		ClosePackEditCommand = new(
			_ => ConfigurePackViewModel = null);
		CanExecutePackChanged += ClosePackEditCommand.RaiseCanExecuteChanged;
		CanExecuteQuestionChanged += ClosePackEditCommand.RaiseCanExecuteChanged;


		EditPackCommand = new(
			_ => { 
				EditPack();
				SelectedQuestion = null;
				ConfigurePackViewModel = new(SelectedPack!, ClosePackEditCommand);
			}, 
			_ => { return 
				ConfigurePackViewModel is null && 
				SelectedPack is not null;
			});
		CanExecutePackChanged += EditPackCommand.RaiseCanExecuteChanged;


		DeletePackCommand = new(
			_ => DeletePack(), 
			_ => { return
				ConfigurePackViewModel is null &&
				SelectedPack is not null or DeletedQuestionPack;
			});
		CanExecutePackChanged += DeletePackCommand.RaiseCanExecuteChanged;


		EditQuestionsCommand = new(
			_ => EditQuestions(),
			_ => { return
				ConfigurePackViewModel is null && 
				SelectedPack is not null && 
				!SelectedPack.CanEditQuestions;
			});
		CanExecutePackChanged += EditQuestionsCommand.RaiseCanExecuteChanged;


		UndoPackChangesCommand = new(
			_ => UndoPackChanges(),
			_ => { return
				SelectedPack is DeletedQuestionPack ||
				(SelectedPack is ModifiedQuestionPack qp && qp.IsChanged);
			});
		CanExecutePackChanged += UndoPackChangesCommand.RaiseCanExecuteChanged;
		CanExecuteQuestionChanged += UndoPackChangesCommand.RaiseCanExecuteChanged;


		ImportQuestionsCommand = new(
			list =>
			{
				if(list is not List<Question> questions) return;
				foreach(var question in questions)
				{
					Questions!.Add(question);
				}
			},
			_ =>
			{
				return
					SelectedPack is not null or DomainQuestionPack or DeletedQuestionPack &&
					SelectedPack.CanEditQuestions;
			});
		ImportQuestionsCommand.ListenToSource(this, nameof(SelectedPack));

		NewQuestionCommand = new(
			question => CreateQuestion(question), 
			_ => { return
				ConfigurePackViewModel is null && 
				SelectedPack is not null && 
				SelectedPack.CanEditQuestions;
			});
		NewQuestionCommand.CanExecuteChanged += (_, _) => NewQuestion_ViewCommand?.RaiseCanExecuteChanged();
		CanExecutePackChanged += NewQuestionCommand.RaiseCanExecuteChanged;


		DeleteQuestionCommand = new(
			_ => DeleteQuestion(), 
			_ => { return 
				ConfigurePackViewModel is null && 
				SelectedQuestion is not null && 
				(SelectedPack?.CanEditQuestions??false);
			});
		CanExecuteQuestionChanged += DeleteQuestionCommand.RaiseCanExecuteChanged;

		foreach(var pack in DomainModel.QuestionPacks.Values)
		{
			Packs.Add(new(pack));
		}
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="command"></param>
	/// <param name="num"></param>
	public void SetViewCommand(RelayCommand command, int num)
	{
		switch(num)
		{
			case 1:
				NewQuiz_ViewCommand = command;
				//CanExecutePackChanged += command.RaiseCanExecuteChanged;
				OnPropertyChanged(nameof(NewQuiz_ViewCommand));
				break;

			case 2:
				NewQuestion_ViewCommand = command;
				CanExecutePackChanged += command.RaiseCanExecuteChanged;
				OnPropertyChanged(nameof(NewQuestion_ViewCommand));
				break;

			case 3:
				ImportQuestions_ViewCommand = command;
				CanExecutePackChanged += command.RaiseCanExecuteChanged;
				OnPropertyChanged(nameof(ImportQuestions_ViewCommand)); 
				break;

			default:
				break;
		}
	}

	public void DomainModelUpdated()
	{
		Packs.Clear();

		foreach(var pack in DomainModel.QuestionPacks.Values)
		{
			Packs.Add(new(pack));
		}
	}
	private void SaveToDomain(object? pack = null)
	{
		SelectedPack = null;
		if(pack is not QuestionPackVariant questionPack)
		{
			foreach(var modified in ModifiedPacks)
			{
				Packs.Add(new(DomainModel.EditQuestionPack(modified.ToDomainPack())));
			}
			ModifiedPacks.Clear();

			foreach(var deleted in DeletedPacks)
			{
				DomainModel.DeleteQuestionPack(deleted.ID);
			}
			DeletedPacks.Clear();

			foreach(var newPack in NewPacks)
			{
				Packs.Add(new(DomainModel.AddQuestionPack(newPack.ToDomainPack())));
			}
			NewPacks.Clear();
		}
		else
		{
			switch(questionPack)
			{
				case NewQuestionPack newPack:
					Packs.Add(new(DomainModel.AddQuestionPack(newPack.ToDomainPack())));
					NewPacks.Remove(newPack);
					break;

				case ModifiedQuestionPack modifiedPack:
					Packs.Add(new(DomainModel.EditQuestionPack(modifiedPack.ToDomainPack())));
					ModifiedPacks.Remove(modifiedPack);
					break;

				case DeletedQuestionPack deletedPack:
					DomainModel.DeleteQuestionPack(deletedPack.ID);
					DeletedPacks.Remove(deletedPack);
					break;

				default:
					break;
			}
		}
		DomainModel.Apply();
	}

	private void AddPack(QuestionPackVariant pack)
	{
		switch(pack)
		{
			case DomainQuestionPack domainPack:
				Packs.Add(domainPack);
				break;
			case NewQuestionPack newPack:
				NewPacks.Add(newPack);
				break;

			case ModifiedQuestionPack modifiedPack:
				ModifiedPacks.Add(modifiedPack);
				break;
			default:
				break;
		}
		SelectedPack = pack;
	}

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
				return;

			case DeletedQuestionPack deletedPack:
				RestoreDeletedPack(deletedPack);
				EditPack();
				break;

			case DomainQuestionPack domainPack:
				StartModifyPack(domainPack);
				break;

			default:
				break;
		}
		CanExecutePackChanged?.Invoke();
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
				DeletedPacks.Add(new DeletedQuestionPack(modifiedPack));
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
	private void RestoreDeletedPack(DeletedQuestionPack? deletedPack)
	{
		if(deletedPack == null)
		{
			if(SelectedPack is not DeletedQuestionPack selected) return;
			else deletedPack = selected;
		}

		DeletedPacks.Remove(deletedPack);
		var restoredPack = deletedPack.RestorePack();
		AddPack(restoredPack);
		SelectedPack = restoredPack;
		SelectedQuestion = null;
	}
	private void UndoPackChanges()
	{
		if(ConfigurePackViewModel is not null)
		{
			ConfigurePackViewModel!.CancelCommand.Execute(null);
		}
		switch(SelectedPack)
		{
			case ModifiedQuestionPack modifiedPack:
				ModifiedPacks.Remove(modifiedPack);
				DomainQuestionPack domainPack = new(modifiedPack);
				Packs.Add(domainPack);
				SelectedPack = domainPack;
				break;

			case DeletedQuestionPack deletedPack:
				RestoreDeletedPack(deletedPack);
				break;

			default:
				break;
		}
	}

	private void StartModifyPack(DomainQuestionPack packToModify)
	{
        if(packToModify == null) return;

		var modifiedPack = new ModifiedQuestionPack(packToModify);
		ModifiedPacks.Add(modifiedPack);
		SelectedPack = modifiedPack;
		Packs.Remove(packToModify);
		SelectedQuestion = null;
	}

	/// <summary>
	/// Unlocks editing of the selected question.
	/// </summary>
	/// <param name="questionModel"></param>
	private void EditQuestions()
	{

		if(SelectedPack == null) return;
		int index = SelectedQuestion == null ? -1 : Questions!.IndexOf(SelectedQuestion);

		EditPack();

		if(SelectedPack.StartEditQuestions){
			SelectedQuestion = index < 0 ? null : Questions![index];
			CanExecutePackChanged?.Invoke();
			//OnPropertyChanged(nameof(Questions));
			return;
		}
		//else
		//{
		//	SelectedQuestion = index < 0 ? null : Questions![index];
		//	CanExecutePackChanged?.Invoke();
		//}
	}

	/// <summary>
	/// Adds a newly created question to the selected packs questions.
	/// </summary>
	/// <param name="questionModel"></param>
	private void CreateQuestion(object? questionModel)
    {
		if (questionModel is not Question question || (!SelectedPack?.CanEditQuestions??false)) return;

		Questions!.Add(question);
		SelectedQuestion = question;
	}


	//private void AddQuestion(object? questionModel)
	//{
	//	if (questionModel is not Question question) return;
	//	EditPack();

	//	Questions!.Add(question);
	//	SelectedQuestion = question;
	//	StartModifyQuestions();
	//}

	private void DeleteQuestion()
	{
		if (SelectedQuestion == null || (!SelectedPack?.CanEditQuestions??false)) return;

		Questions!.Remove(SelectedQuestion!);
		SelectedQuestion = null;
	}

	public static Visibility IsNull(ConfigurationViewModel? model) => model==null ? Visibility.Hidden : Visibility.Visible;

	//public static implicit operator Visibility(ConfigurationViewModel? model) Visibility.Visible;

	//public static explicit operator Visibility(ConfigurationViewModel model) =>  Visibility.Visible;

	public static implicit operator Visibility(ConfigurationViewModel? model) => model==null ? Visibility.Hidden : Visibility.Visible;
}