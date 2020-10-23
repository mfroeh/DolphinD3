using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Dolphin.Ui.ViewModel
{
    public class SettingsTabViewModel : ViewModelBase
    {
        private readonly ISettingsService settingsService;
        private ICollection<Keys> skillKeybindings;
        private Keys teleportToTownKey;
        private Keys openMapKey;
        private Keys openInventoryKey;

        public SettingsTabViewModel(ISettingsService settingsService)
        {
            this.settingsService = settingsService;

            PossibleKeys = System.Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            SkillKeybindings = new ObservableCollection<Keys>(settingsService.Settings.SkillKeybindings);

            // TODO: Generic approach, if one more keybinding is added
            TeleportToTownKey = settingsService.Settings.TeleportToTownKeybinding;
            OpenMapKey = settingsService.Settings.OpenMapKeybinding;
            OpenInventoryKey = settingsService.Settings.OpenInventoryKeybinding;
        }

        public ICollection<Keys> PossibleKeys { get; }

        public ICollection<Keys> SkillKeybindings
        {
            get => skillKeybindings;
            set
            {
                skillKeybindings = value;
                RaisePropertyChanged(nameof(SkillKeybindings));
            }
        }

        public Keys TeleportToTownKey
        {
            get => teleportToTownKey;
            set
            {
                teleportToTownKey = value;
                settingsService.Settings.TeleportToTownKeybinding = value;
                RaisePropertyChanged(nameof(TeleportToTownKey));
            }
        }

        public Keys OpenMapKey
        {
            get => openMapKey;
            set
            {
                openMapKey = value;
                settingsService.Settings.OpenMapKeybinding = value;
                RaisePropertyChanged(nameof(OpenMapKey));
            }
        }

        public Keys OpenInventoryKey
        {
            get => openInventoryKey;
            set
            {
                openInventoryKey = value;
                settingsService.Settings.OpenInventoryKeybinding = value;
                RaisePropertyChanged(nameof(OpenInventoryKey));
            }
        }
    }
}