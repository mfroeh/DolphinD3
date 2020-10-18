using System;
using System.Collections.Generic;

namespace Dolphin
{
    public interface IEventSubscriber
    {
        // IList<Subscription<IEvent>> Subscriptions { get; }

        void Subscribe<T>(Subscription<T> subscription) where T : IEvent;

        void Unsubscribe<T>(Subscription<T> subscription) where T : IEvent;
    }
}