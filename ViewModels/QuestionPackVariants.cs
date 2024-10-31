using Labb3_CsProg_ITHS.NET.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Labb3_CsProg_ITHS.NET.ViewModels
{
	public class DomainQuestionPack : QuestionPackVariant/*, QuestionPackVariant*/
	{
		private readonly ObservableCollection<Question> questions;

		public override QuestionPack DomainPack { get; protected set; }
		public DomainQuestionPack(QuestionPack domainPack)
		{
			DomainPack = domainPack;
			questions = new (domainPack.Questions);
		}
		public DomainQuestionPack(QuestionPackVariant domainPack)
		{
			DomainPack = domainPack.DomainPack;
			questions = new(DomainPack.Questions);
		}

		public override string Name { 
			get => DomainPack.Name;
			set { }
		}
		public override Difficulty Difficulty { 
			get => DomainPack.Difficulty;
			set { }
		}
		public override uint TimeLimit { 
			get => DomainPack.TimeLimit;
			set { }
		}
		public override ObservableCollection<Question> Questions { 
			get => questions;
			set { }
		}
	}

	public class NewQuestionPack : QuestionPackVariant/*, QuestionPackVariant*/
	{
		private QuestionPack domainPack;

		public NewQuestionPack(string name, Difficulty difficulty, uint timeLimit, ObservableCollection<Question>? questions = null)
		{
			domainPack = new(name, difficulty, timeLimit, null);
			Questions = questions??new();
		}

		public override QuestionPack DomainPack {
			get => domainPack ??= new(Name, Difficulty, TimeLimit, Questions.ToList());
			protected set { } }

		public override string Name
		{
			get => domainPack.Name;
			set
			{
				domainPack.Name = value;
				OnPropertyChanged();
			}
		}
		public override Difficulty Difficulty
		{
			get => domainPack.Difficulty; 
			set
			{
				domainPack.Difficulty = value;
				OnPropertyChanged();
			}
		}
		public override uint TimeLimit
		{
			get => domainPack.TimeLimit;
			set
			{
				domainPack.TimeLimit = value;
				OnPropertyChanged();
			}
		}
		public override ObservableCollection<Question> Questions { get; set; }
	}

	public class ModifiedQuestionPack : QuestionPackVariant/*, QuestionPackVariant*/
	{
		private string? name;
		private Difficulty? difficulty;
		private uint? timeLimit;
		private ObservableCollection<Question>? questions;

		public ModifiedQuestionPack(QuestionPackVariant domainPack)
		{
			DomainPack = domainPack.DomainPack;
			//ID = domainPack.ID;
			//Name = domainPack.Name;
			//Difficulty = domainPack.Difficulty;
			//TimeLimit = domainPack.TimeLimit;
			//Questions = new();
			//domainPack.Questions.ForEach(q => Questions.Add(new(q)));
		}
		public ModifiedQuestionPack(QuestionPack domainPack, string? modifiedName = null, Difficulty? modifiedDifficulty = null, uint? modifiedTimeLimit = null, ObservableCollection<Question>? modifiedQuestions = null)
		{
			DomainPack = domainPack;

			name = modifiedName;
			difficulty = modifiedDifficulty;
			timeLimit = modifiedTimeLimit;
			questions = modifiedQuestions;
			//domainPack.Questions.ForEach(q => Questions.Add(new(q)));
		}

		public override QuestionPack DomainPack { get; protected set; }

		public override string Name
		{
			get => name ?? DomainPack.Name;
			set
			{
				name = value;
				OnPropertyChanged();
				OnPropertyChanged("IsChanged");
			}
		}
		public override Difficulty Difficulty
		{
			get => difficulty ?? DomainPack.Difficulty;
			set
			{
				difficulty = value;
				OnPropertyChanged();
				OnPropertyChanged("IsChanged");
			}
		}
		public override uint TimeLimit
		{
			get => timeLimit ?? DomainPack.TimeLimit;
			set
			{
				timeLimit = value;
				OnPropertyChanged();
				OnPropertyChanged("IsChanged");
			}
		}
		public override ObservableCollection<Question> Questions
		{
			get
			{
				if (questions == null)
				{
					questions = new ObservableCollection<Question>();
					DomainPack.Questions.ForEach(q => questions.Add(new(q)));
				}
				return questions;
			}

			set
			{
				questions = value;
				OnPropertyChanged();
				OnPropertyChanged("IsChanged");
			}
		}

	}

	public class DeletedQuestionPack : QuestionPackVariant/*, QuestionPackVariant*/
	{
		private ObservableCollection<Question>? questions;

		public DeletedQuestionPack(QuestionPack domainPack)
		{
			DomainPack = domainPack;
		}
		public DeletedQuestionPack(QuestionPackVariant pack)
		{
			DomainPack = pack.DomainPack;
		}

		public override QuestionPack DomainPack { get; protected set; }
		public override string Name { 
			get => DomainPack.Name;
			set { }
		}
		public override Difficulty Difficulty { 
			get => DomainPack.Difficulty;
			set { }
		}
		public override uint TimeLimit { 
			get => DomainPack.TimeLimit;
			set { }
		}
		public override ObservableCollection<Question> Questions { 
			get => questions??=new(DomainPack.Questions);
			set { }
		}
	}

	// Had to create both an abstract class and an interface because neither could implement everything I needed.
	public abstract class QuestionPackVariant : INotifyPropertyChanged
	{
		public abstract QuestionPack DomainPack { get; protected set; }
		public int ID => DomainPack.ID;
		public abstract string Name { get; set; }
		public abstract Difficulty Difficulty { get; set; }
		public abstract uint TimeLimit { get; set; }
		public abstract ObservableCollection<Question> Questions { get; set; }

		public event PropertyChangedEventHandler? PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
	//public interface IModifiablePackVariant
	//{
	//	public abstract bool IsChanged { get; }
	//	public abstract string SetName { set; }
	//	public abstract Difficulty SetDifficulty { set; }
	//	public abstract uint SetTimeLimit { set; }
	//	public abstract ObservableCollection<Question> SetQuestions { set; }
	//}
	//public interface QuestionPackVariant
	//{
	//       public int ID { get; }
	//	public string Name { get; }
	//	public Difficulty Difficulty { get; }
	//	public  uint TimeLimit { get; }
	//	public ObservableCollection<Question> Questions { get; }
	//}
}
