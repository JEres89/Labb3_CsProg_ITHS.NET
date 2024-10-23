namespace Labb3_CsProg_ITHS.NET.Models;
public class QuestionPack
{
	public QuestionPack(string name, Difficulty difficulty, uint timeLimit, List<Question>? questions = null)
	{
		Name = name;
		Difficulty = difficulty;
		TimeLimit = timeLimit;
		Questions = questions??new();
	}
	
	public string Name { get; set; }
    public Difficulty Difficulty { get; set; }
    public uint TimeLimit { get; set; }
    public List<Question> Questions { get; set; }



	public void AddQuestion(Question question)
	{
		Questions.Add(question);
	}
	public void DeleteQuestion(Question question)
	{
		Questions.Remove(question);
	}
}

public enum Difficulty
{
	Easy	= 0,
	Medium	= 1,
	Hard	= 2
}
