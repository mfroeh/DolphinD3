using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Dolphin.NewEventBus
{
    public class EventBus : IEventBus
    {
        private readonly IList<Subscription<IEvent>> subscribers = new List<Subscription<IEvent>>();

        public void Publish<T>(T @event) where T : IEvent
        {
            foreach (var sub in subscribers)
            {
                if (sub.EventType == typeof(T))
                    sub.ReactAsync(@event);
            }
        }

        public void Subscribe<T>(Subscription<T> subscriber) where T : IEvent
        {
            lock (subscribers)
            {
                subscribers.Add((Subscription<IEvent>)(object)subscriber);
            }
        }

        public void Unsubscribe<T>(Subscription<T> subscriber) where T : IEvent
        {
            lock (subscribers)
            {
                subscribers.Remove((Subscription<IEvent>)(object)subscriber);
            }
        }
    }
}
