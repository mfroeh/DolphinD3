using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Dolphin.UI
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IEventChannel eventChannel;
        private readonly IModelService modelService;

        public ICollection<TabViewmodelBase> Tabs { get; } = new ObservableCollection<TabViewmodelBase>();
        private int selectedTab = 0;

        public MainWindowViewModel(IEventChannel eventChannel, IModelService modelService, [Dependency("data")] TabViewmodelBase dataTab,
                                    [Dependency("log")] TabViewmodelBase logTab, ISettingsService settingsService) : base(settingsService)
        {
            this.eventChannel = eventChannel;
            this.modelService = modelService;
            Tabs.Add(dataTab);
            Tabs.Add(logTab);
        }

        public int Selected
        {
            get { return selectedTab; }
            set
            {
                selectedTab = value;
                RaisePropertyChanged("Selected");
            }
        }
    }
}