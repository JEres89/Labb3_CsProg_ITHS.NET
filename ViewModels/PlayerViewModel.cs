using Labb3_CsProg_ITHS.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace Labb3_CsProg_ITHS.NET.ViewModels;
public class PlayerViewModel : ViewModelBase
{
	private readonly Random _rand = new();

	private QuestionPack? _runningQuiz;
	private int _currentQuestionIndex = -1;
	private int _score = 0;
	private Question? _currentQuestion;

	//private Question? CurrentQuestion
	//{
	//	get => _currentQuestion;
	//	set
	//	{
	//		_currentQuestion=value;
	//		//QuestionChanged();
	//		//OnPropertyChanged(null);
	//	}
	//}


	private DispatcherTimer _timer;
	private uint _timeLeft;

	//private string? _questionText;

	private readonly string[] _answers = new string[4];
	//private string _answerOptionTwo;
	//private string _answerOptionThree;
	//private string _answerOptionFour;

	private int correctAnswer;

	private bool? _isCorrect;

	private Brush? correctnessBrush1;
	private Brush? correctnessBrush2;
	private Brush? correctnessBrush3;
	private Brush? correctnessBrush4;
	private Action<Brush?>[] brushSetters;

	private static Brush correctBrush = new SolidColorBrush(new() { G = 200, A = 255});
	private static Brush incorrectBrush = new SolidColorBrush(new() { R = 200, A = 255 });


	public PlayerViewModel(MainWindowsViewModel mainViewModel)
	{
		MainViewModel = mainViewModel;

		StartQuizCommand =		new(StartQuestion,	_ => IsPlaying);
		StartQuizCommand.ListenToSource(this, nameof(IsPlaying));

		SelectAnswerCommand =	new(SelectAnswer,	_ => _isCorrect == null);
		SelectAnswerCommand.ListenToSource(this, nameof(IsCorrect));

		NextQuestionCommand =	new(
			NextQuestion,	
			_ => _isCorrect != null);
		NextQuestionCommand.ListenToSource(this, nameof(IsCorrect));

		_timer = new(DispatcherPriority.Send);

		brushSetters = [
			b => CorrectnessBrush1 = b,
			b => CorrectnessBrush2 = b,
			b => CorrectnessBrush3 = b,
			b => CorrectnessBrush4 = b
		];

		OnPropertyChanged(null);
	}

	public bool IsPlaying => _runningQuiz != null;
    public bool HasNotStarted => !HasFinished && _runningQuiz != null && _currentQuestion == null;
    public bool HasQuestion => _currentQuestion != null;
    public bool HasFinished => IsPlaying && _currentQuestionIndex >= _runningQuiz!.Questions.Count;

	public MainWindowsViewModel MainViewModel { get; set; }

	public RelayCommand StartQuizCommand { get; }
	public RelayCommand SelectAnswerCommand { get; }
	public RelayCommand NextQuestionCommand { get; }

	public string? QuizName => _runningQuiz?.Name;
	public string? Difficulty => _runningQuiz?.Difficulty.ToString();
	public uint? TimeLimit => _runningQuiz?.TimeLimit;
    public int? NumberOfQuestions => _runningQuiz?.Questions.Count;
    public int CurrentQuestion => _currentQuestionIndex+1;

    public string NextBtnText { get; set; } = "Next Question";
    public int Score
	{
		get => _score;
		set
		{
			_score=value;
			OnPropertyChanged();
		}
	}
	public uint TimeLeft
	{
		get => _timeLeft;
		set
		{
			_timeLeft=value;
			OnPropertyChanged();
		}
	}


	public string? QuestionText => _currentQuestion?.QuestionText;

	public string AnswerOption1 => _answers?[0]??string.Empty;
	public Brush? CorrectnessBrush1
	{
		get => correctnessBrush1; 
		set
		{
			correctnessBrush1=value;
			OnPropertyChanged();
		}
	}

	public string AnswerOption2 => _answers?[1]??string.Empty;
	public Brush? CorrectnessBrush2
	{
		get => correctnessBrush2; set
		{
			correctnessBrush2=value;
			OnPropertyChanged();
		}
	}

	public string AnswerOption3 => _answers?[2]??string.Empty;
	public Brush? CorrectnessBrush3
	{
		get => correctnessBrush3;
		set
		{
			correctnessBrush3=value;
			OnPropertyChanged();
		}
	}
	public string AnswerOption4 => _answers?[3]??string.Empty;
	public Brush? CorrectnessBrush4
	{
		get => correctnessBrush4; 
		set
		{
			correctnessBrush4=value;
			OnPropertyChanged();
		}
	}

	public bool? IsCorrect
	{
		get => _isCorrect; 
		private set
		{
			_isCorrect = value;
			if(_isCorrect??false)
			{
				Score++;
			}
			OnPropertyChanged();
		}
	}

	public void PlayQuiz(int id)
	{
		if(!DomainModel.QuestionPacks.TryGetValue(id, out _runningQuiz))
		{
			return;
		}

		_currentQuestionIndex = 0;
		Score = 0;
		_timer.Interval = TimeSpan.FromSeconds(1);
		_timer.Tick += Timer_Tick;
		NextBtnText = "Next Question";

		OnPropertyChanged(null);
	}

	public void StopQuiz()
	{
		_runningQuiz = null;
		_timer.Stop();
		_timer.Tick -= Timer_Tick;
		_currentQuestionIndex = -1;
		OnPropertyChanged(null);
		//QuizChanged();
	}


	private void Timer_Tick(object? sender, EventArgs e)
	{
		TimeLeft--;
		if(_timeLeft < 1)
		{
			_isCorrect = false;
			StopQuestion();
		}
	}

	private void StartQuestion(object? _ = null)
	{
		ResetBrushes();
		_isCorrect = null;
		_currentQuestion = _runningQuiz!.Questions[_currentQuestionIndex];
		SetAnswerOptions();
		
		_timeLeft = _runningQuiz!.TimeLimit;

		OnPropertyChanged(null);

		_timer.Start();

		/// <summary>
		/// Ensures that the correct answer is not always in the same position
		/// </summary>
		void SetAnswerOptions()
		{
			HashSet<int> answerNumbers = [0, 1, 2, 3];

			for(int answerIndex = 0; answerIndex < 4; answerIndex++)
			{
				int nextAnswer = _rand.Next(0, answerNumbers.Count);
				

				SetAnswerOption(answerIndex, nextAnswer = answerNumbers.ElementAt(nextAnswer));
				answerNumbers.Remove(nextAnswer);
			}
		}
		void SetAnswerOption(int answerIndex, int answerNumber)
		{
			_answers[answerIndex] = answerNumber switch
			{
				0 => _currentQuestion.CorrectAnswer,
				1 => _currentQuestion.IncorrectAnswerOne,
				2 => _currentQuestion.IncorrectAnswerTwo,
				3 => _currentQuestion.IncorrectAnswerThree,
				_ => throw new ArgumentOutOfRangeException(nameof(answerNumber))
			};
			if(answerNumber == 0)
			{
				correctAnswer = answerIndex;
			}
		}
	}

	private void StopQuestion()
	{
		_timer.Stop();
		//IsCorrect = IsCorrect??false;
	}

	private void NextQuestion(object? _ = null)
	{
		_currentQuestionIndex++;
		if(_currentQuestionIndex >= _runningQuiz!.Questions.Count)
		{
			//_runningQuiz = null;
			_timer.Stop();
			_currentQuestion = null;
			OnPropertyChanged(null);

			return;
		}
		if(_currentQuestionIndex+1 == _runningQuiz!.Questions.Count)
		{
			NextBtnText = "Finish Quiz";
		}
		StartQuestion();
	}

	private void SelectAnswer(object? answer)
	{
		if(_isCorrect != null || answer is not int value)
		{
			return;
		}

		StopQuestion();
		if(value == correctAnswer)
		{
			brushSetters[correctAnswer].Invoke(correctBrush);
			IsCorrect = true;
		}
		else
		{
			brushSetters[correctAnswer].Invoke(correctBrush);
			brushSetters[value].Invoke(incorrectBrush);
			IsCorrect = false;
		}
	}
	private void ResetBrushes()
	{
		foreach(var brush in brushSetters)
		{
			brush.Invoke(null);
		}
	}

	// OnPropertyChanged(null); triggers all properties to update
	//private void QuizChanged()
	//{
	//	OnPropertyChanged(nameof(QuizName));
	//	OnPropertyChanged(nameof(Difficulty));
	//	OnPropertyChanged(nameof(TimeLimit));
	//	OnPropertyChanged(nameof(NumberOfQuestions));

	//	OnPropertyChanged(nameof(IsPlaying));
	//	OnPropertyChanged(nameof(HasNotStarted));
	//}

	// OnPropertyChanged(null); triggers all properties to update
	//private void QuestionChanged()
	//{
	//	OnPropertyChanged(nameof(TimeLeft));
	//	OnPropertyChanged(nameof(QuestionText));
	//	OnPropertyChanged(nameof(IsCorrect));
	//	OnPropertyChanged(nameof(AnswerOptionOne));
	//	OnPropertyChanged(nameof(AnswerOption2));
	//	OnPropertyChanged(nameof(AnswerOption3));
	//	OnPropertyChanged(nameof(AnswerOption4));

	//	OnPropertyChanged(nameof(HasQuestion));
	//	OnPropertyChanged(nameof(HasNotStarted));
	//}


	/// <summary>
	/// Ensures that the correct answer is not always in the same position
	///// </summary>
	//private void SetAnswerOptions()
	//{
	//	HashSet<int> answerNumbers = [0,1,2,3];

	//	for(int answerIndex = 0; answerIndex < 4; answerIndex++)
	//	{
	//		int nextAnswer = _rand.Next(0, answerNumbers.Count);
	//		SetAnswerOption(answerIndex, answerNumbers.ElementAt(nextAnswer));
	//		answerNumbers.Remove(nextAnswer);
	//	}
	//}
	//private void SetAnswerOption(int answerIndex, int answerNumber)
	//{
	//	_answers[answerIndex] = answerNumber switch
	//	{
	//		0 => _currentQuestion!.CorrectAnswer,
	//		1 => _currentQuestion!.IncorrectAnswerOne,
	//		2 => _currentQuestion!.IncorrectAnswerTwo,
	//		3 => _currentQuestion!.IncorrectAnswerThree,
	//		_ => throw new ArgumentOutOfRangeException(nameof(answerNumber))
	//	};
	//	if(answerNumber == 0)
	//	{
	//		correctAnswer = answerIndex;
	//	}
	//}
}
