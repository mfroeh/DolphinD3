namespace Dolphin.NewEventBus
{
    public interface IEventPublisher<T> where T : IEvent
    {
        void Publish(T @event);
    }
}
