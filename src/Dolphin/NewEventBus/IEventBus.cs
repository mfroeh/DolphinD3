namespace Dolphin
{
    public interface IEventBus
    {
        void Publish<T>(T @event) where T : IEvent;

        void Subscribe<T>(Subscription<T> subscriber) where T : IEvent;

        void Unsubscribe<T>(Subscription<T> subscriber) where T : IEvent;
    }
}
