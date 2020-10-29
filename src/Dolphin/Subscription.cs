using System;
using System.Threading;

namespace Dolphin
{
    public delegate void CancellableEventHandler<T>(T @event, CancellationToken token);

    public interface ISubscription
    {
        public Type EventType { get; }
    }

    public class Subscription<T> : ISubscription where T : IEvent
    {
        public Subscription(EventHandler<T> handler)
        {
            Handler = handler;
        }

        public Type EventType { get; } = typeof(T);

        public EventHandler<T> Handler { get; }
    }
}