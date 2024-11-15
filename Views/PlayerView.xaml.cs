using Labb3_CsProg_ITHS.NET.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labb3_CsProg_ITHS.NET.Views;
/// <summary>
/// Interaction logic for PlayerView.xaml
/// </summary>
public partial class PlayerView : UserControl
{
	public PlayerView()
	{
		InitializeComponent();
	}

	private void answerButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if(sender is Button button && button.CommandParameter is int answerNumber)
		{
			if(button.IsEnabled)
			{
				button.UpdateDefaultStyle();
			}
			else
			{
				switch(answerNumber)
				{
					case 1:
						button.Background = (DataContext as PlayerViewModel).CorrectnessBrush1;
						button.BorderBrush = (DataContext as PlayerViewModel).CorrectnessBrush1;
						break;
					case 2:
						button.Background = (DataContext as PlayerViewModel).CorrectnessBrush2;
						break;
					case 3:
						button.Background = (DataContext as PlayerViewModel).CorrectnessBrush3;
						break;
					case 4:
						button.Background = (DataContext as PlayerViewModel).CorrectnessBrush4;
						break;
				}
			}
		}
	}
}
