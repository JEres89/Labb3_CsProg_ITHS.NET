using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_CsProg_ITHS.NET.Models;
public class QuestionPack
{
	public QuestionPack(string name, int difficulty, int timeLimit, ObservableCollection<Question>? questions = null)
	{
		Name = name;
		Difficulty = difficulty;
		TimeLimit = timeLimit;
		Questions = questions??new();
	}

	public string Name { get; set; }
    public int Difficulty { get; set; }
    public int TimeLimit { get; set; }
    public ObservableCollection<Question> Questions { get; set; }


}

public enum Difficulty
{
	Easy = 0,
	Medium = 1,
	Hard = 2
}
