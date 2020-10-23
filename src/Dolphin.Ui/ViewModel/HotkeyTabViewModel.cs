using Dolphin.Enum;
using Dolphin.Service;
using Dolphin.Ui.Dialog;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Unity;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Ui.ViewModel
{
    public class HotkeyTabViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly ISettingsService settingsService;
        private readonly IUnityContainer unityContainer;
        private readonly IEventPublisher<HotkeyPressedEvent> hotkeyListener;
        private ICommand changeHotkeyCommand;

        private ItemType selectedItem;

        private uint spareColumns;
        private bool empowered;
        private bool pickGem;
        private ConvertingSpeed convertingSpeed;

        public HotkeyTabViewModel(IUnityContainer unityContainer, IDialogService dialogService, ISettingsService settingsService, [Dependency("hotkeyListener")] IEventPublisher<HotkeyPressedEvent> hotkeyListener)
        {
            this.unityContainer = unityContainer;
            this.dialogService = dialogService;
            this.settingsService = settingsService;
            this.hotkeyListener = hotkeyListener;

            Hotkeys = new ObservableDictionary<ActionName, Hotkey>(settingsService.Settings.Hotkeys);
            ItemTypes = System.Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToList();

            selectedItem = settingsService.Settings.MacroSettings.SelectedGambleItem;
            spareColumns = settingsService.Settings.MacroSettings.SpareColumns;
            empowered = settingsService.Settings.MacroSettings.Empowered;
            pickGem = settingsService.Settings.MacroSettings.PickGemYourself;
            convertingSpeed = settingsService.Settings.MacroSettings.ConvertingSpeed;
        }

        public ConvertingSpeed ConvertingSpeed
        {
            get => convertingSpeed;
            set
            {
                convertingSpeed = value;
                settingsService.Settings.MacroSettings.ConvertingSpeed = value;
                RaisePropertyChanged(nameof(ConvertingSpeed));
            }
        }

        public ICommand ChangeHotkeyCommand
        {
            get
            {
                changeHotkeyCommand = changeHotkeyCommand ?? new RelayCommand((o) => ShowChangeHotkeyDialog((ActionName)o));
                return changeHotkeyCommand;
            }
        }

        public bool Empowered
        {
            get => empowered;
            set
            {
                empowered = value;
                settingsService.Settings.MacroSettings.Empowered = value;
                RaisePropertyChanged(nameof(Empowered));
            }
        }

        public bool PickGemYourself
        {
            get => pickGem;
            set
            {
                pickGem = value;
                settingsService.Settings.MacroSettings.PickGemYourself = value;
                RaisePropertyChanged(nameof(Empowered));
            }
        }

        public IDictionary<ActionName, Hotkey> Hotkeys { get; }

        public IList<ItemType> ItemTypes { get; }

        public ItemType SelectedGambleItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                settingsService.Settings.MacroSettings.SelectedGambleItem = value;
                RaisePropertyChanged(nameof(SelectedGambleItem));
            }
        }

        public uint SpareColumnIndex
        {
            get => spareColumns;
            set
            {
                spareColumns = value;
                settingsService.Settings.MacroSettings.SpareColumns = spareColumns;
                RaisePropertyChanged(nameof(SpareColumnIndex));
            }
        }

        private void ShowChangeHotkeyDialog(ActionName actionAllocationToChange)
        {
            var oldHotkey = Hotkeys[actionAllocationToChange];

            var dialogViewModel = unityContainer.Resolve<ChangeHotkeyDialogViewModel>("changeHotkey");
            dialogViewModel.SetHotkey(oldHotkey);
            dialogViewModel.EditingAction = actionAllocationToChange;

            if (!settingsService.Settings.IsPaused)
            {
                settingsService.NegateIsPaused();
            }

            bool? success = dialogService.ShowDialog(this, dialogViewModel);
            if (success == true && oldHotkey != dialogViewModel.Hotkey)
            {
                var hotkey = dialogViewModel.Hotkey;

                var listCopy = Hotkeys.Select(item => item).ToList();
                foreach (var item in listCopy)
                {
                    if (item.Value == hotkey)
                    {
                        settingsService.SetHotkeyValue(item.Key, null);
                        Hotkeys[item.Key] = null;
                        break; // There can only ever be 
                    }
                }

                settingsService.SetHotkeyValue(actionAllocationToChange, hotkey);
                Hotkeys[actionAllocationToChange] = hotkey;

                RaisePropertyChanged("Hotkeys");
                NotifyHotkeysChanged(settingsService.Settings.Hotkeys.Values.ToList());
            }

            settingsService.NegateIsPaused();
        }

        private void NotifyHotkeysChanged(IList<Hotkey> newHotkeys)
        {
            ((HotkeyListenerService)hotkeyListener).RefreshHotkeys(newHotkeys);
        }
    }
}