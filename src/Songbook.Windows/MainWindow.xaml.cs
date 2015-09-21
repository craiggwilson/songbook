﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reactive.Linq;
using Songbook.Windows.ViewModels;
using Microsoft.Win32;

namespace Songbook.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            ViewModel = new MainViewModel();
            InitializeComponent();

            ViewModel.SongsTab.OpenDirectory
                .Subscribe(o => ChooseSongsDirectory());
        }

        public MainViewModel ViewModel { get; }

        private void ChooseSongsDirectory()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                ViewModel.SongsTab.CurrentDirectory = System.IO.Path.GetDirectoryName(dialog.FileName);
            }
        }
    }
}