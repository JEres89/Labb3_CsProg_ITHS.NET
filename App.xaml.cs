﻿using Labb3_CsProg_ITHS.NET.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Labb3_CsProg_ITHS.NET;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

	public static RelayCommand DefaultCommand { get; } = new(_ => { }, _ => false);
}

