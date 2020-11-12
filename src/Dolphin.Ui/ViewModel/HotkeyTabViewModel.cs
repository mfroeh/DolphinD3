using Dolphin.Enum;
using Dolphin.Service;
using Dolphin.Ui.Dialog;
using MvvmDialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Unity;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Ui.ViewModel
{
    public class HotkeyTabViewModel : ViewModelBase
    {
        #region Private Fields

        private readonly IEventPublisher<HotkeyPressedEvent> hotkeyListener;
        private readonly IMessageBoxService messageBoxService;
        private readonly ISettingsService settingsService;
        private ICommand changeHotkeyCommand;

        private ConvertingSpeed convertingSpeed;
        private bool empowered;
        private bool pickGem;
        private ItemType selectedItem;

        private uint spareColumns;

        #endregion Private Fields

        #region Public Constructors

        public HotkeyTabViewModel(ISettingsService settingsService, [Dependency("hotkeyListener")] IEventPublisher<HotkeyPressedEvent> hotkeyListener, IMessageBoxService messageBoxService)
        {
            this.settingsService = settingsService;
            this.hotkeyListener = hotkeyListener;
            this.messageBoxService = messageBoxService;

            Hotkeys = new Dictionary<ActionName, Hotkey>(settingsService.Settings.Hotkeys);
            ItemTypes = System.Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToList();

            selectedItem = settingsService.Settings.MacroSettings.SelectedGambleItem;
            spareColumns = settingsService.Settings.MacroSettings.SpareColumns;
            empowered = settingsService.Settings.MacroSettings.Empowered;
            pickGem = settingsService.Settings.MacroSettings.PickGemYourself;
            convertingSpeed = settingsService.Settings.MacroSettings.ConvertingSpeed;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICommand ChangeHotkeyCommand
        {
            get
            {
                changeHotkeyCommand = changeHotkeyCommand ?? new RelayCommand((o) => ShowChangeHotkeyDialog((ActionName)o));
                return changeHotkeyCommand;
            }
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

        public IDictionary<ActionName, Hotkey> Hotkeys { get; }

        public IList<ItemType> ItemTypes { get; }

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

        public int SelectedSkillCastProfile
        {
            get => settingsService.SkillCastSettings.SelectedIndex;
            set
            {
                settingsService.SkillCastSettings.SelectedIndex = value;
                RaisePropertyChanged(nameof(SelectedSkillCastProfile));
            }
        }

        public IList<string> SkillCastProfiles
        {
            get => settingsService.SkillCastSettings.SkillCastConfigurations.Select(x => x.Name).ToList();
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

        #endregion Public Properties

        #region Private Methods

        private void NotifyHotkeysChanged(IList<Hotkey> newHotkeys)
        {
            ((HotkeyListenerService)hotkeyListener).RefreshHotkeys(newHotkeys);
        }

        private void ShowChangeHotkeyDialog(ActionName actionAllocationToChange)
        {
            var oldHotkey = Hotkeys[actionAllocationToChange];
            settingsService.SetPaused(true, true);

            var result = messageBoxService.ShowCustomDialog<ChangeHotkeyDialogViewModel>(this, oldHotkey, actionAllocationToChange);
            var viewModel = result.Item2;
            if (result.Item1 == true && oldHotkey != viewModel.Hotkey)
            {
                var hotkey = viewModel.Hotkey;

                var listCopy = Hotkeys.Select(item => item).ToList();
                foreach (var item in listCopy)
                {
                    if (item.Value == hotkey)
                    {
                        settingsService.SetHotkeyValue(item.Key, null);
                        Hotkeys[item.Key] = null;
                        break; // There can only ever be one
                    }
                }

                settingsService.SetHotkeyValue(actionAllocationToChange, hotkey);
                Hotkeys[actionAllocationToChange] = hotkey;

                RaisePropertyChanged(nameof(Hotkeys));
                NotifyHotkeysChanged(settingsService.Settings.Hotkeys.Values.ToList());
            }

            settingsService.SetPaused(false, true);
        }

        #endregion Private Methods
    }
}