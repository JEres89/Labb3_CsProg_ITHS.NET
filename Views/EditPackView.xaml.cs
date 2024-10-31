using Labb3_CsProg_ITHS.NET.Models;
using Labb3_CsProg_ITHS.NET.ViewModels;
using System.Windows.Controls;

namespace Labb3_CsProg_ITHS.NET.Views
{
	/// <summary>
	/// Interaction logic for EditPackView.xaml
	/// </summary>
	public partial class EditPackView : UserControl
    {
        public bool IsActive => Pack != null;
        public QuestionPackVariant? Pack { get; set; }
        public RelayCommand? CancelCommand { get; set; }
        public EditPackView(/*QuestionPackVariant pack, RelayCommand cancelCommand*/)
        {
			//var cancelCommand = new ((_) => DataContext = null);
			//DataContext = new ConfigurePackViewModel(Pack, CancelCommand);
			//InitializeComponent();
		}

		public void StartEdit(QuestionPackVariant pack, RelayCommand cancelCommand)
		{
			Pack = pack;
			CancelCommand = cancelCommand;
			DataContext = new ConfigurePackViewModel(Pack, CancelCommand);
			InitializeComponent();

		}

	}
}
