using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Window = Dolphin.Enum.Window;

namespace Dolphin.DevUi
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ICaptureWindowService captureWindowService;

        private readonly ISaveImageService imageService;

        private string status;

        public MainViewModel(ICaptureWindowService captureWindowService, ISaveImageService imageService)
        {
            this.imageService = imageService;
            this.captureWindowService = captureWindowService;

            // Set defaults
            SelectedExtraInformation = PossibleExtraInformation.FirstOrDefault();
            SelectedWindow = PossibleWindow.FirstOrDefault();
            SelectedWorldLocation = PossibleLocation.FirstOrDefault();
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

        public ICommand ClipExtraInformation => new RelayCommand((o) => ExecuteSaveAction(imageService.SaveExtraInformation, SelectedExtraInformation));

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

        public IList<ExtraInformation> PossibleExtraInformation => EnumHelper.GetValues<ExtraInformation>().ToList();

        public IList<WorldLocation> PossibleLocation => EnumHelper.GetValues<WorldLocation>().ToList();

        public IList<Window> PossibleWindow => EnumHelper.GetValues<Window>().ToList();

        public ExtraInformation SelectedExtraInformation { get; set; }

        public Window SelectedWindow { get; set; }

        public WorldLocation SelectedWorldLocation { get; set; }

        public string Status
        {
            get => status;
            set
            {
                status = value;
                RaisePropertyChanged(nameof(Status));
            }
        }

        private void ExecuteSaveAction(Action<Bitmap> action)
        {
            var image = captureWindowService.CaptureWindow("Diablo III64");

            if (image != null)
            {
                action.Invoke(image);
                Execute.AndForgetAsync(() =>
                {
                    Status = "Saved!";
                    Thread.Sleep(500);
                    Status = "";
                });
            }
            else
            {
                Execute.AndForgetAsync(() =>
                {
                    Status = "No active Diablo III process found.";
                    Thread.Sleep(500);
                    Status = "";
                });
            }
        }

        private void ExecuteSaveAction<T>(Action<Bitmap, T> action, T parameter)
        {
            ExecuteSaveAction((x) => action.Invoke(x, parameter));
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}