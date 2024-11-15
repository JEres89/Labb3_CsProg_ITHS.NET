using Labb3_CsProg_ITHS.NET.ViewModels;
using System.Windows;

namespace Labb3_CsProg_ITHS.NET.Dialogs
{
	/// <summary>
	/// Interaction logic for DownloadOTDBDialog.xaml
	/// </summary>
	public partial class DownloadOTDBDialog : Window
    {
        public DownloadOTDBDialog(RelayCommand saveCommand)
        {
			var model = DownloadDialogViewModel.Instance;
			DataContext = model;

			var newSaveCommand = new RelayCommand(list =>
			{
				saveCommand.Execute(list);
				DialogResult = true;
			});
			model.SaveQuestionsCommand = newSaveCommand;
			InitializeComponent();
        }
    }
}
