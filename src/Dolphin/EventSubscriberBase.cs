namespace Dolphin
{
    public class EventSubscriberBase : IEventSubscriber
    {
        protected readonly IEventBus eventBus;

        public EventSubscriberBase(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        protected void SubscribeBus<T>(Subscription<T> subscription) where T : IEvent
        {
            eventBus.Subscribe(subscription);
        }

        protected void UnsubscribeBus<T>(Subscription<T> subscription) where T : IEvent
        {
            eventBus.Unsubscribe(subscription);
        }
    }
}