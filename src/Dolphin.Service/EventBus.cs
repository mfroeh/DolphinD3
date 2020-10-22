using System.Collections.Generic;

namespace Dolphin.Service
{
    public class EventBus : IEventBus
    {
        private readonly IList<ISubscription> subscribers = new List<ISubscription>();

        public void Publish<T>(T @event) where T : IEvent
        {
            foreach (var sub in subscribers)
            {
                if (sub.EventType == typeof(T))
                {
                    ((Subscription<T>)sub).Handler.Invoke(this, @event);
                }
            }
        }

        public void Subscribe<T>(Subscription<T> subscriber) where T : IEvent
        {
            lock (subscribers)
            {
                subscribers.Add(subscriber);
            }
        }

        public void Unsubscribe<T>(Subscription<T> subscriber) where T : IEvent
        {
            lock (subscribers)
            {
                subscribers.Remove(subscriber);
            }
        }
    }
}