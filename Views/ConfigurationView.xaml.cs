
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
	public ConfigurationView()
	{
		DataContext = _viewModel = new ConfigurationViewModel();

		NewQuizCommand = new(_ => new CreatePackDialog(_viewModel.NewPackCommand).ShowDialog());

		EditQuizViewCommand = new(
			_ => EditQuiz(), 
			_viewModel.EditPackCommand.CanExecute);
		_viewModel.EditPackCommand.CanExecuteChanged += (_,_) => EditQuizViewCommand.RaiseCanExecuteChanged();
		InitializeComponent();
	}
	
	public RelayCommand NewQuizCommand { get; private set; }
	public RelayCommand EditQuizViewCommand { get; private set; }
	//private void NewQuiz()
	//{
	//	var newDialog = new CreatePackDialog(_viewModel.NewPackCommand);
	//	_ = newDialog.ShowDialog();
	//}
	private void EditQuiz()
	{
		if(_viewModel.SelectedPack == null) return;

		_viewModel.EditPackCommand.Execute(null);

		PropertiesView.StartEdit(_viewModel.SelectedPack, EditQuizViewCommand);
		//PropertiesView.Content = new EditPackView(_viewModel.SelectedPack, new(_ => PropertiesView.Content = null));
	}
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
//{Binding RelativeSource={RelativeSource Mode=FindAncestor, AncestorLevel=3, AncestorType=local:ConfigurationView}, Path=EditQuizViewCommand }
//{Binding Source=ConfigurationView, Path=EditQuizViewCommand }
//<CollectionViewSource x:Key="dataContext" Source="{Binding}" />
//<CollectionContainer x:Name="newPacksCon" Collection="{Binding Source={StaticResource dataContext}, Path=NewPacks}" />