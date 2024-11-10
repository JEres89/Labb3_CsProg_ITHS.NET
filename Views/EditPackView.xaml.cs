using Labb3_CsProg_ITHS.NET.Models;
using Labb3_CsProg_ITHS.NET.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Labb3_CsProg_ITHS.NET.Views
{
	/// <summary>
	/// Interaction logic for EditPackView.xaml
	/// </summary>
	public partial class EditPackView : UserControl
    {
		//public bool IsActive => Pack != null;
		public ConfigurePackViewModel? ViewModel
		{
			get
			{
				if(DataContext is ConfigurePackViewModel viewModel)
				{
					return viewModel;
				}
				return null;
			}
			set => DataContext = value;
		}
        //public RelayCommand? CloseCommand { get; set; }
        public EditPackView(/*QuestionPackVariant pack, RelayCommand cancelCommand*/)
        {
			//var cancelCommand = new ((_) => DataContext = null);
			//DataContext = new ConfigurePackViewModel(Pack, CancelCommand);
			//DataContextChanged += ConfigurationView_DataContextChanged;
			InitializeComponent();
		}

		//private void ConfigurationView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		//{
		//	if(DataContext is ConfigurationViewModel cfg)
		//	{
		//	}
		//	//InitializeComponent();
		//}

		//public void StartEdit(/*ConfigurePackViewModel viewModel*/)
		//{
		//	//DataContext = ViewModel = viewModel;
		//	//viewModel.CloseCommand = CloseCommand;
		//	Visibility = System.Windows.Visibility.Visible;
		//	InitializeComponent();
		//}
		//public void Close()
		//{
		//	Visibility = System.Windows.Visibility.Hidden;
		//	ViewModel!.CloseCommand = null;
		//	ViewModel = null;
		//	//CloseCommand = null;
		//}

	}
}
