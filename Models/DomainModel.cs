using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_CsProg_ITHS.NET.Models
{
	public static class DomainModel
	{
		private static int _currentQuestionPackId = 0;
		public static Dictionary<int, QuestionPack> QuestionPacks { get; private set; } = new();

		public static void AddQuestionPack(QuestionPack questionPack)
		{
			questionPack.ID = _currentQuestionPackId;
			QuestionPacks.Add(_currentQuestionPackId++, questionPack);
		}

		public static void EditQuestionPack(int questionPackId, QuestionPack editedQuestionPack)
		{
			if (QuestionPacks.TryGetValue(questionPackId, out QuestionPack? pack))
			{
				editedQuestionPack.Difficulty = pack.Difficulty;
				editedQuestionPack.Name = pack.Name;
				editedQuestionPack.TimeLimit = pack.TimeLimit;
				editedQuestionPack.Questions = pack.Questions;
			}
			else
			{
				throw new ArgumentException("Invalid question pack ID.");
			}
		}

		public static void DeleteQuestionPack(int questionPackId)
		{
			if (!QuestionPacks.Remove(questionPackId))
			{
				throw new ArgumentException("Invalid question pack ID.");
			}
		}

		public static void AddQuestion(int questionPackId, Question question)
		{
			if (QuestionPacks.TryGetValue(questionPackId, out QuestionPack? pack))
			{
				pack.Questions.Add(question);
			}
			else
			{
				throw new ArgumentException("Invalid question pack ID.");
			}
		}

		public static void EditQuestion(int questionPackId, int questionIndex, Question newQuestion)
		{
			if (QuestionPacks.TryGetValue(questionPackId, out QuestionPack? pack) && questionIndex >= 0 && questionIndex < pack.Questions.Count)
			{
				pack.Questions[questionIndex] = newQuestion;
			}
			else
			{
				throw new ArgumentException("Invalid question pack ID or question index.");
			}
		}

		public static void DeleteQuestion(int questionPackId, int questionIndex)
		{
			if (QuestionPacks.TryGetValue(questionPackId, out QuestionPack? pack) && questionIndex >= 0 && questionIndex < pack.Questions.Count)
			{
				pack.Questions.RemoveAt(questionIndex);
			}
			else
			{
				throw new ArgumentException("Invalid question pack ID or question index.");
			}
		}
	}
}
