namespace Dolphin.UI
{
    public class TabViewmodelBase : ViewModelBase
    {
        public string Title { get; set; }

        public TabViewmodelBase(ISettingsService settingsService) : base(settingsService)
        {
        }
    }
}
