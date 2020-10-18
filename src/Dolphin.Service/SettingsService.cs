using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin.Service
{
    public class SettingsService : ISettingsService
    {
        public Settings Settings { get; }
        public UISettings UISettings { get; }

        public SettingsService(Settings settings)
        {
            Settings = settings;
            UISettings = settings.UISettings;
        }
    }
}
