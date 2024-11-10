
using Labb3_CsProg_ITHS.NET.Dialogs;
//using Labb3_CsProg_ITHS.NET.Models;
using Labb3_CsProg_ITHS.NET.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Labb3_CsProg_ITHS.NET.Views;
/// <summary>
/// Interaction logic for ConfigurationView.xaml
/// </summary>
public partial class ConfigurationView : UserControl
{
	private ConfigurationViewModel _viewModel;

	public ConfigurationViewModel ViewModel => _viewModel;

	public ConfigurationView()
	{


		DataContextChanged += ConfigurationView_DataContextChanged;
		InitializeComponent();
	}

	private void ConfigurationView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if(DataContext is ConfigurationViewModel cfg)
		{
			_viewModel = cfg;
		}
		else
		{
			throw new("Invalid DataContext");
		}

		_viewModel.NewQuiz_ViewCommand = new(
			_ => new CreatePackDialog(ViewModel.NewPackCommand).ShowDialog()
			);

		_viewModel.NewQuestion_ViewCommand = new(
			_ => new CreateQuestionDialog(ViewModel.NewQuestionCommand).ShowDialog(),
			_ => ViewModel.NewQuestionCommand.CanExecute(_));
	}

		//_viewModel.EditQuiz_ViewCommand = new(
		//	_ => {
		//		ViewModel.EditPackCommand.Execute(null);
		//		EditQuiz(); },
		//	_ => ViewModel.EditPackCommand.CanExecute(null)
		//	);
	//}

	//public RelayCommand NewQuiz_ViewCommand { get; private set; }
	//public RelayCommand EditQuiz_ViewCommand { get; private set; }
	//public RelayCommand NewQuestion_ViewCommand { get; private set; }
	//public RelayCommand EditQuestion_ViewCommand { get; private set; }
	//Selected="{Binding EditQuestion_ViewCommand, RelativeSource={RelativeSource Mode=FindAncestor, AncestorLevel=1, AncestorType=local:ConfigurationView} }"

	//private void NewQuiz()
	//{
	//	var newDialog = new CreatePackDialog(_viewModel.NewPackCommand);
	//	_ = newDialog.ShowDialog();
	//}
	//private void EditQuiz()
	//{
	//	//_viewModel.ConfigurePackViewModel!.CloseCommand = new(_ =>
	//	//{
	//	//	QuizPropertiesEdit.Close();
	//	//	_viewModel.ConfigurePackViewModel = null;
	//	//});
	//	//QuizPropertiesEdit.ViewModel = _viewModel.ConfigurePackViewModel;
	//	//QuizPropertiesEdit.StartEdit();
	//	//QuizPropertiesEdit.Content = new EditPackView(_viewModel.SelectedPack, new(_ => QuizPropertiesEdit.Content = null));
	//}
}

//public class QuizTemplateSelector : DataTemplateSelector
//{
//	private static DataTemplate? _newPacks;
//	private static DataTemplate? _modifiedPacks;
//	private static DataTemplate? _deletedPacks;
//	private static DataTemplate? _packs;

//	private DataTemplate NewPacksTemplate
//	{
//		get
//		{
//			if (_newPacks != null)
//			{
//				return _newPacks;
//			}
//			_newPacks = new();
//			_newPacks.DataType = typeof(QuestionPack);
//			_newPacks.VisualTree = new FrameworkElementFactory(typeof(TextBlock));
//			//var markerText = new FrameworkElementFactory(typeof(TextBlock));
//			//markerText.Text = "+";
//			_newPacks.VisualTree.AppendChild(new FrameworkElementFactory("+"));

//			var nameBind = new FrameworkElementFactory(typeof(TextBlock));
//			nameBind.SetBinding(TextBlock.TextProperty, new Binding("Name"));
//			_newPacks.VisualTree.AppendChild(nameBind);

//			return _newPacks;
//		}
//	}
//	public override DataTemplate SelectTemplate(object item, DependencyObject container)
//	{
//		if(container is ContentPresenter p && item is QuestionPack q)
//		{
//			if(p.TemplatedParent is Grid g)
//			{
//				if (g.DataContext is ConfigurationViewModel m)
//				{
//					if (m.NewPacks.Contains(q))
//					{
//						return NewPacksTemplate;
//					}
//				}
//			}
//			else if (p.TemplatedParent is ComboBoxItem i)
//			{
//				var viewModel = p.FindName("configView"	) as ConfigurationViewModel;
//				if (viewModel.NewPacks.Contains(q))
//				{
//					return NewPacksTemplate;
//				}
//			}
//		}

//		return base.SelectTemplate(item, container);
//	}
//}
