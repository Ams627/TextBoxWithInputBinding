﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using MvvmFoundation.Wpf;

namespace TextBoxWithInputBinding
{
    public class Station
    {
        public string Name { get; set; }
        public string CRS { get; set; }
        public string NLC { get; set; }
        public override string ToString()
        {
            return $"{Name} {CRS} - ({NLC})";
        }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // set focus to first item in tab order on MainWindow:
            Loaded += (s, e) => MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

            Pressed1 = new RelayCommand(() =>
            {
                System.Diagnostics.Debug.WriteLine("pressed in mainwindow");
            });

            Wonk = new RelayCommand(() =>
            {
                System.Diagnostics.Debug.WriteLine("enter pressed");
            });

        }

        public RelayCommand Wonk { get; set; }
        public RelayCommand<Station> AddStation { get; set; } = new RelayCommand<Station>((station) =>
        {
            System.Diagnostics.Debug.WriteLine($"Station {station} added");
        });


        public RelayCommand Pressed1 { get; set; }

        public ObservableCollection<Station> StationList { get; set; } = new ObservableCollection<Station>()
        {
            new Station()
            {
                CRS = "POO",
                NLC = "5883",
                Name = "Poole"
            },
            new Station()
            {
                CRS = "BSM",
                NLC = "5881",
                Name = "Branksome"
            }
        };
    }
}
