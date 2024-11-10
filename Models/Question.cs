using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_CsProg_ITHS.NET.Models;

[DebuggerDisplay("{QuestionText}")]
public class Question
{
	public Question()
	{
		Category = "General";
		QuestionText = "What is the meaning of life?";
		CorrectAnswer = "42";
		IncorrectAnswerOne = "24";
		IncorrectAnswerTwo = "21";
		IncorrectAnswerThree = "12";
		//, "21", "12" };
	}
	public Question(Question question) : this(/*question.Type,*/ question.Difficulty, question.Category, question.QuestionText, question.CorrectAnswer, question.IncorrectAnswerOne, question.IncorrectAnswerTwo, question.IncorrectAnswerThree) { }

	public Question(string questionText, string correctAnswer, string incorrectAnswerOne, string incorrectAnswerTwo, string incorrectAnswerThree) : this(/*0,*/ null, "General", questionText, correctAnswer, incorrectAnswerOne, incorrectAnswerTwo, incorrectAnswerThree) { }

	public Question(/*int type,*/ Difficulty? difficulty, string category, string questionText, string correctAnswer, string incorrectAnswerOne, string incorrectAnswerTwo, string incorrectAnswerThree)
	{
		//Type = type;
		Difficulty = difficulty;
		Category = category;
		QuestionText = questionText;
		CorrectAnswer = correctAnswer;
		IncorrectAnswerOne = incorrectAnswerOne;
		IncorrectAnswerTwo = incorrectAnswerTwo;
		IncorrectAnswerThree = incorrectAnswerThree;
	}

	//public int Type { get; set; }

    public Difficulty? Difficulty { get; set; }
    public string Category { get; set; }
	public string QuestionText { get; set; }

	public string CorrectAnswer { get; set; }
	public string IncorrectAnswerOne { get; set; }
	public string IncorrectAnswerTwo { get; set; }
	public string IncorrectAnswerThree { get; set; }
}
