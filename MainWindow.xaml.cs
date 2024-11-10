using Labb3_CsProg_ITHS.NET.ViewModels;
using System.Windows;

namespace Labb3_CsProg_ITHS.NET;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		DataContext = new MainWindowsViewModel();
		InitializeComponent();
	}
}
