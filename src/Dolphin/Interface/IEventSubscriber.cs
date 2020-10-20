namespace Dolphin
{
    public interface IEventSubscriber
    {
        void Subscribe<T>(Subscription<T> subscription) where T : IEvent;

        void Unsubscribe<T>(Subscription<T> subscription) where T : IEvent;
    }
}