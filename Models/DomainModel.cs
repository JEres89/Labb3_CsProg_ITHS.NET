

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace Labb3_CsProg_ITHS.NET.Models
{
	public static class DomainModel
	{
		private static int _currentQuestionPackId = 0;
		public static Dictionary<int, QuestionPack> QuestionPacks { get; private set; } = new();

		public static event NotifyCollectionChangedEventHandler? CollectionChanged;

		private static List<QuestionPack> changes = new();
		private static JsonWriter writer;
		static DomainModel() { 
			writer = new("./QuizpackDatabase");
			//Load();
		}

		public static bool Load()
		{
			writer.ReadAll();
			CollectionChanged?.Invoke(QuestionPacks, new(NotifyCollectionChangedAction.Reset));
			return QuestionPacks.Count > 0;
		}
		public static void Apply()
		{
			writer.Write();
			changes.Clear();
		}

		public static QuestionPack AddQuestionPack(QuestionPack questionPack)
		{
			questionPack.ID = _currentQuestionPackId;
			QuestionPacks.Add(_currentQuestionPackId++, questionPack);
			changes.Add(questionPack);
			CollectionChanged?.Invoke(questionPack, new(NotifyCollectionChangedAction.Add, questionPack));

			return questionPack;
		}

		public static QuestionPack EditQuestionPack(QuestionPack editedPack)
		{
			if (QuestionPacks.TryGetValue(editedPack.ID, out QuestionPack? domainPack))
			{
				if(editedPack == domainPack)
				{
					return domainPack;
				}
				domainPack.Difficulty = editedPack.Difficulty;
				domainPack.Name = editedPack.Name;
				domainPack.TimeLimit = editedPack.TimeLimit;
				domainPack.Questions = editedPack.Questions;
				changes.Add(domainPack);

				CollectionChanged?.Invoke(domainPack, new(NotifyCollectionChangedAction.Replace, domainPack, domainPack));

				return domainPack;
			}
			else
			{
				throw new ArgumentException("Invalid question pack ID.");
			}
		}

		public static void DeleteQuestionPack(int questionPackId)
		{

			if (!QuestionPacks.TryGetValue(questionPackId, out var questionPack))
			{
				throw new ArgumentException("Invalid question pack ID.");
			}
			QuestionPacks.Remove(questionPackId);
			CollectionChanged?.Invoke(questionPack, new(NotifyCollectionChangedAction.Remove, questionPack));
			changes.Add(questionPack);
		}

		//public static void AddQuestion(int questionPackId, Question question)
		//{
		//	if (QuestionPacks.TryGetValue(questionPackId, out QuestionPack? pack))
		//	{
		//		pack.Questions.Add(question);
		//	}
		//	else
		//	{
		//		throw new ArgumentException("Invalid question pack ID.");
		//	}
		//}

		//public static void EditQuestion(int questionPackId, int questionIndex, Question newQuestion)
		//{
		//	if (QuestionPacks.TryGetValue(questionPackId, out QuestionPack? pack) && questionIndex >= 0 && questionIndex < pack.Questions.Count)
		//	{
		//		pack.Questions[questionIndex] = newQuestion;
		//	}
		//	else
		//	{
		//		throw new ArgumentException("Invalid question pack ID or question index.");
		//	}
		//}

		//public static void DeleteQuestion(int questionPackId, int questionIndex)
		//{
		//	if (QuestionPacks.TryGetValue(questionPackId, out QuestionPack? pack) && questionIndex >= 0 && questionIndex < pack.Questions.Count)
		//	{
		//		pack.Questions.RemoveAt(questionIndex);
		//	}
		//	else
		//	{
		//		throw new ArgumentException("Invalid question pack ID or question index.");
		//	}
		//}
		private class JsonWriter
		{
			private string _path;

			public JsonWriter(string path)
			{
				_path=Path.GetFullPath(path);
				if(!Directory.Exists(_path))
				{
					Directory.CreateDirectory(_path);
				}
			}

			public void Write()
			{
				foreach(var pack in changes)
				{
					string packPath = $"QuestionPack_{pack.ID}*";
					string fullPackPath = $"{_path}\\QuestionPack_{pack.ID}_{pack.Name}.json";
					var files = Directory.GetFiles(_path, packPath);
					if(files.Length > 0)
					{
						if(!File.Exists(fullPackPath))
						{
							File.Move(files[0],fullPackPath);
							Debug.WriteLine($"Renaming and overwriting file {files[0]}");
						}
						else
						{
							Debug.WriteLine($"Overwriting file {fullPackPath}");
						}
					}
					else
					{
						Debug.WriteLine($"Creating file {fullPackPath}");
					}
					File.WriteAllText(fullPackPath, JsonSerializer.Serialize(pack, options: new() { WriteIndented=true}));
				}
			}
			public void ReadAll()
			{
				Directory.GetFiles(_path).ToList().ForEach(file =>
				{
					if(!file.Contains("QuestionPack_")) return;
					var pack = JsonSerializer.Deserialize<QuestionPack>(File.ReadAllText(file));
					if(pack == null) return;

					QuestionPacks[pack.ID] = pack;
				});
			}
		}
	}
}
