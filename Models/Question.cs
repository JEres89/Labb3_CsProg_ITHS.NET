using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_CsProg_ITHS.NET.Models;
public class Question
{
	public Question(string questionText, string correctAnswer, string[] incorrectAnswers) : this(0, 0, "General", questionText, correctAnswer, incorrectAnswers) { }

	public Question(int type, int difficulty, string category, string questionText, string correctAnswer, string[] incorrectAnswers)
	{
		Type = type;
		Difficulty = difficulty;
		Category = category;
		QuestionText = questionText;
		CorrectAnswer = correctAnswer;
		IncorrectAnswers = incorrectAnswers;
	}

	public int Type { get; set; }

    public int Difficulty { get; set; }
    public string Category { get; set; }
	public string QuestionText { get; set; }

	public string CorrectAnswer { get; set; }
	public string[] IncorrectAnswers { get; set; }
}
