using Dolphin.Enum;
using Dolphin.Ui.Dialog;
using MvvmDialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Unity;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Ui
{
    public class HotkeyTabViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly ISettingsService settingsService;
        private readonly IUnityContainer unityContainer;

        private IDictionary<string, Hotkey> hotkeys;

        public HotkeyTabViewModel(IUnityContainer unityContainer, IDialogService dialogService, ISettingsService settingsService)
        {
            this.unityContainer = unityContainer;
            this.dialogService = dialogService;
            this.settingsService = settingsService;

            hotkeys = new ObservableDictionary<string, Hotkey>();
            foreach (var item in settingsService.Settings.Hotkeys)
            {
                hotkeys.Add(item.Key.ToString(), item.Value);
            }
        }

        public ICommand ChangeHotkeyCommand => new RelayCommand((index) => ShowDialog("CubeConverter")); // TODO

        public IDictionary<string, Hotkey> Hotkeys
        {
            get => hotkeys;
            set
            {
                hotkeys = value;
                RaisePropertyChanged("Hotkeys");
            }
        }

        private void ShowDialog(string actionAllocationToChange)
        {
            var oldHotkey = hotkeys[actionAllocationToChange];

            var dialogViewModel = unityContainer.Resolve<ChangeHotkeyDialogViewModel>("changeHotkey");
            dialogViewModel.SetHotkey(oldHotkey);

            bool? success = dialogService.ShowDialog(this, dialogViewModel);
            if (success == true && oldHotkey != dialogViewModel.Hotkey)
            {
                var hotkey = dialogViewModel.Hotkey;

                foreach (var item in hotkeys)
                {
                    if (item.Value == hotkey)
                    {
                        settingsService.SetHotkeyValue((ActionName)System.Enum.Parse(typeof(ActionName), item.Key), null);
                        hotkeys[item.Key] = null;
                    }
                }

                settingsService.SetHotkeyValue((ActionName)System.Enum.Parse(typeof(ActionName), actionAllocationToChange), hotkey);
                hotkeys[actionAllocationToChange] = hotkey;
            }
        }
    }
}