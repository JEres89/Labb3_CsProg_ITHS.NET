using Labb3_CsProg_ITHS.NET.Dialogs;
using Labb3_CsProg_ITHS.NET.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Labb3_CsProg_ITHS.NET.ViewModels;
internal class ConfigurationViewModel : ViewModelBase
{
    public ObservableCollection<QuestionPack> Packs { get; set; } = new();
    public QuestionPack? SelectedPack { get; set; }

    public ObservableCollection<Question>? Questions => SelectedPack?.Questions;
    public Question? SelectedQuestion { get; set; }

    public RelayCommand NewPackCommand { get; }
    public RelayCommand DeletePackCommand { get; }
	public ConfigurationViewModel()
    {
        NewPackCommand = new(_ => CreatePack());
		DeletePackCommand = new(_ => DeletePack(), _ => CanDeletePack());
	}
    private void CreatePack()
    {

        var createDialog = new CreatePackDialog();

        if (createDialog.ShowDialog()!.Value)
        {
            //createDialog.
		}
    }
    
    private void DeletePack()
    {
        if(SelectedPack == null) return;

        Packs.Remove(SelectedPack);
        SelectedPack = null;
    }
    
    private bool CanDeletePack()
    {
        return SelectedPack != null;
	}
}