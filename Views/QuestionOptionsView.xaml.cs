using System.Windows.Controls;

namespace Labb3_CsProg_ITHS.NET.Views
{
	/// <summary>
	/// Interaction logic for QuestionOptionsView.xaml
	/// </summary>
	public partial class QuestionOptionsView : UserControl
	{
		public string Header
		{
			get => HeaderBlock.Text;
			set => HeaderBlock.Text = value;
		}
        public bool IsEditing { get; set; }
        public QuestionOptionsView()
		{
			InitializeComponent();
		}

	}
}
