using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Window = Dolphin.Enum.Window;

namespace Dolphin.DevUi
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ICaptureWindowService captureWindowService;

        private readonly ISaveImageService imageService;

        public MainViewModel(ICaptureWindowService captureWindowService, ISaveImageService imageService)
        {
            this.imageService = imageService;
            this.captureWindowService = captureWindowService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ChangeOutputDirectory => new RelayCommand((o) =>
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                OutputDirectory = dialog.SelectedPath;
            }
        });

        public ICommand ClipHealthbar => new RelayCommand((o) => ExecuteSaveAction(imageService.SaveHealthbar));

        public ICommand ClipPlayerClass => new RelayCommand((o) => ExecuteSaveAction(imageService.SavePlayerClass));

        public ICommand ClipPlayerSkills => new RelayCommand((o) =>
        {
            ExecuteSaveAction(imageService.SavePlayerSkills);
            ExecuteSaveAction(imageService.SavePlayerSkillsMouse);
        });

        public ICommand ClipPlayerSkillsActive => new RelayCommand((o) => ExecuteSaveAction(imageService.SavePlayerSkillsActive));

        public ICommand ClipPrimaryResource => new RelayCommand((o) => ExecuteSaveAction(imageService.SavePlayerResourcePrimary));

        public ICommand ClipPrimaryResourceDH => new RelayCommand((o) => ExecuteSaveAction(imageService.SavePlayerResourcePrimaryDemonHunter));

        public ICommand ClipSecondaryResourceDH => new RelayCommand((o) => ExecuteSaveAction(imageService.SavePlayerResourceSecondaryDemonHunter));

        public ICommand ClipUrshiGemUp => new RelayCommand((o) => ExecuteSaveAction(imageService.SaveUrshiGemUp, SelectedGemUp));

        public ICommand ClipWindow => new RelayCommand((o) => ExecuteSaveAction(imageService.SaveWindow, SelectedWindow));

        public ICommand ClipWorldLocation => new RelayCommand((o) => ExecuteSaveAction(imageService.SaveWorldLocation, SelectedWorldLocation));

        public ICommand OpenInExplorer => new RelayCommand((o) => System.Diagnostics.Process.Start(OutputDirectory));

        public string OutputDirectory
        {
            get => Path.GetFullPath(Properties.Settings.Default.OutputDirectory);
            set
            {
                Properties.Settings.Default.OutputDirectory = value;
                RaisePropertyChanged(nameof(OutputDirectory));
            }
        }

        public IList<int> PossibleGemUp => new List<int> { 1, 2, 3, 4, 5 };

        public IList<WorldLocation> PossibleLocation => new List<WorldLocation> { WorldLocation.Grift, WorldLocation.Menu };

        public IList<Window> PossibleWindow => new List<Window> { Window.AcceptGrift, Window.Kadala, Window.Obelisk, Window.StartGame, Window.Urshi };

        public int SelectedGemUp { get; set; } = 5;

        public Window SelectedWindow { get; set; } = Window.Urshi;

        public WorldLocation SelectedWorldLocation { get; set; } = WorldLocation.Grift;

        private void ExecuteSaveAction(Action<Bitmap> action)
        {
            var image = captureWindowService.CaptureWindow("Diablo III64");

            if (image != null)
            {
                action.Invoke(image);
                //MessageBox.Show("Saved.");
            }
            else
            {
                MessageBox.Show("No active Diablo III process found.");
            }
        }

        private void ExecuteSaveAction<T>(Action<Bitmap, T> action, T parameter)
        {
            var image = captureWindowService.CaptureWindow("Diablo III64");

            if (image != null)
            {
                action.Invoke(image, parameter);
                //MessageBox.Show("Saved.");
            }
            else
            {
                MessageBox.Show("No active Diablo III process found.");
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}