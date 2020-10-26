namespace Dolphin.Service
{
    public class WorldInformationChangedEvent : IEvent
    {
        public bool IsLocationChanged { get; set; }
        
        public bool IsWindowChanged { get; set; }
    }
}