using Labb3_CsProg_ITHS.NET.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Web;

namespace Labb3_CsProg_ITHS.NET.ViewModels
{
    class DownloadDialogViewModel : ViewModelBase
	{
		private static DownloadDialogViewModel? instance;
		private const string api_url = "https://opentdb.com/api.php?";
		private const string api_categories = "https://opentdb.com/api_category.php";
		private const char separator = '&';

		private const string api_getAmount = "amount=";
		private const string api_getCategory = "category=";
		private const string api_getDifficulty = "difficulty=";

		//only support multiple choice
		private const string api_multiple = "type=multiple";

		private int amount;
		private (int id, string name)? category;
		private Difficulty? difficulty;
		private bool setCategory;
		private bool setDifficulty;

		private DownloadDialogViewModel() { 
			DownloadQuestionsCommand.ListenToSource(this, nameof(Amount));
		}
		public static DownloadDialogViewModel Instance => instance??= new DownloadDialogViewModel();

		public ObservableCollection<(int id, string name)> Categories { get; set; } = new();
		public List<Question>? NewQuestions { get; set; }

		public RelayCommand? SaveQuestionsCommand { get; set; }
		public RelayCommand DownloadQuestionsCommand { get; set; } = new RelayCommand(
			_ => {
				Instance.NewQuestions = Instance.GetQuestions();
				Instance.SaveQuestionsCommand?.Execute(Instance.NewQuestions);
			},
			_ => Instance.Amount > 0);


		public int Amount
		{
			get => amount;
			set
			{
				amount=value;
				OnPropertyChanged();
			}
		}

		public bool SetCategory
		{
			get => setCategory; 
			set
			{
				setCategory=value;
				if(Categories.Count == 0)
				{
					GetCategories();
				}
				OnPropertyChanged();
			}
		}
		public (int id, string name)? Category
		{
			get => category;
			set
			{
				category = value;
				OnPropertyChanged();
			}
		}

		public bool SetDifficulty
		{
			get => setDifficulty; 
			set
			{
				setDifficulty=value;
				//if(value)
				//	Difficulty = Models.Difficulty.Easy;
				//else 
				//	Difficulty = null;
				OnPropertyChanged();
			}
		}
		public Difficulty? Difficulty
		{
			get => difficulty;
			set
			{
				difficulty = value;
				OnPropertyChanged();
			}
		}

		public double Minimum => ConfigurePackViewModel.DiffSliderValuesStatic[0];
		public double Maximum => ConfigurePackViewModel.DiffSliderValuesStatic[^1];
		public DoubleCollection DiffSliderValues => ConfigurePackViewModel.DiffSliderValuesStatic;

		private async void GetCategories()
		{
			HttpClient client = new();
			HttpResponseMessage response = await client.GetAsync(api_categories);

			if(response.IsSuccessStatusCode)
			{
				JsonNode? json = JsonNode.Parse(response.Content.ReadAsStream());
				if(json == null)
					return;
				JsonNode? categories = json["trivia_categories"];
				if(categories is JsonArray array)
				{
					foreach(JsonObject item in array)
					{
						if(item!.TryGetPropertyValue("id", out var id) && item.TryGetPropertyValue("name", out var name))
						{
							(int, string) category = (id!.AsValue().GetValue<int>(), name!.AsValue().GetValue<string>());

							Categories.Add(category);
						}
					}
				}
			}
		}

		private List<Question>? GetQuestions()
		{
			string url = $"{api_url}{api_getAmount}{amount}{separator}{api_multiple}{(setCategory?separator+api_getCategory+category.Value.id:"")}{(setDifficulty ? separator+api_getDifficulty+difficulty!.ToString().ToLower() : "")}";


			HttpClient client = new();
			HttpResponseMessage response = client.GetAsync(url).Result;

			if(response.IsSuccessStatusCode)
			{
				JsonNode? json = JsonNode.Parse(response.Content.ReadAsStream());
				if(json == null)
					return null;
				JsonNode? questions = json["results"];

				List<Question> questionList = new List<Question>();
				if(questions is JsonArray array)
				{
					foreach(JsonObject item in array)
					{
						Difficulty? difficulty = null;
						string category, questionText, correctAnswer;
						string[] incorrectAnswers;

						try
						{
							//if(item.TryGetPropertyValue("type", out var type))
							//{
							//	question.Type = type!.AsValue().GetValue<string>();
							//}
							if(item!.TryGetPropertyValue("difficulty", out var jsondifficulty))
							{
								string diffstr = jsondifficulty!.AsValue().GetValue<string>();
								difficulty = diffstr switch
								{
									"easy" => Models.Difficulty.Easy,
									"medium" => Models.Difficulty.Medium,
									"hard" => Models.Difficulty.Hard,
									_ => null
								};
							}
							if(item.TryGetPropertyValue("category", out var jsoncategory))
							{
								category = HttpUtility.HtmlDecode(jsoncategory!.AsValue().GetValue<string>());
							}
							else
							{
								continue;
							}
							if(item.TryGetPropertyValue("question", out var jsonquestionText))
							{
								questionText = HttpUtility.HtmlDecode(jsonquestionText!.AsValue().GetValue<string>());
							}
							else
							{
								continue;
							}
							if(item.TryGetPropertyValue("correct_answer", out var jsoncorrectAnswer))
							{
								correctAnswer = HttpUtility.HtmlDecode(jsoncorrectAnswer!.AsValue().GetValue<string>());
							}
							else
							{
								continue;
							}
							if(item.TryGetPropertyValue("incorrect_answers", out var jsonincorrectAnswers))
							{
								incorrectAnswers = [
									HttpUtility.HtmlDecode(jsonincorrectAnswers[0].AsValue().GetValue<string>()),
									HttpUtility.HtmlDecode(jsonincorrectAnswers[1].AsValue().GetValue<string>()),
									HttpUtility.HtmlDecode(jsonincorrectAnswers[2].AsValue().GetValue<string>())];
							}
							else
							{
								continue;
							}
						}
						catch(Exception e)
						{
							continue;
						}
						Question question = new(difficulty, category, questionText, correctAnswer, incorrectAnswers);

						questionList.Add(question);
					}
					return questionList;
				}
			}
			return null;
		}
	}
}
