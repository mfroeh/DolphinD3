using Dolphin.NewEventBus;
using System;
using System.Threading;

namespace Dolphin
{
    public class Subscription<T> where T : IEvent
    {
        public Subscription(CancellableEventHandler<T> cancelableHandler)
        {
            CancellableHandler = cancelableHandler;
        }

        public Subscription(EventHandler<T> handler)
        {
            Handler = handler;
        }

        public CancellableEventHandler<T> CancellableHandler { get; }

        public EventHandler<T> Handler { get; }

        public Type EventType { get; } = typeof(T);

        public CancellationTokenSource TokenSource { get; set; }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public static class SubscriptionExtensionMethods
    {
        public static void ReactAsync<T>(this Subscription<T> sub, T @event) where T : IEvent
        {
            if (sub.CancellableHandler != null)
            {
                sub.TokenSource?.Dispose();
                sub.TokenSource = new CancellationTokenSource();
                Execute.AndForgetAsync(() => sub.CancellableHandler.Invoke(@event, sub.TokenSource.Token));
            }

            if (sub.Handler != null)
                Execute.AndForgetAsync(() => sub.Handler.Invoke(null, @event));
        }

        public static void CancelReaction<T>(this Subscription<T> sub) where T : IEvent
        {
            sub.TokenSource?.Cancel();
            sub.TokenSource?.Dispose();
        }
    }

    public delegate void CancellableEventHandler<T>(T @event, CancellationToken token);
}