using Labb3_CsProg_ITHS.NET.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Labb3_CsProg_ITHS.NET.Views;
/// <summary>
/// Interaction logic for PackOptionsView.xaml
/// </summary>
public partial class PackOptionsView : UserControl
{
	public string Header { 
		get => HeaderBlock.Text;
		set => HeaderBlock.Text = value;
	}
	public PackOptionsView()
	{
		InitializeComponent();
	}
	//private void OnCreate(object sender, RoutedEventArgs e)
	//{
	//	Window.GetWindow(this).
	//	DialogResult = true;
	//}
	//private void OnCancel(object sender, RoutedEventArgs e)
	//{
	//	Window.GetWindow(this).
	//	DialogResult = false;
	//}
}
