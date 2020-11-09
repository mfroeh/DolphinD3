namespace Dolphin
{
    public class PausedEvent : IEvent
    {
        public bool IsFromChanging { get; set; }

        public bool IsPaused { get; set; }
    }
}