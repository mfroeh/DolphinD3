namespace Dolphin
{
    public class EventSubscriberBase : IEventSubscriber
    {
        protected readonly IEventBus eventBus;

        public EventSubscriberBase(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public void Subscribe<T>(Subscription<T> subscription) where T : IEvent
        {
            eventBus.Subscribe(subscription);
        }

        public void Unsubscribe<T>(Subscription<T> subscription) where T : IEvent
        {
            eventBus.Unsubscribe(subscription);
        }
    }
}