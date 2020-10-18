using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Dolphin.Service
{
    public class EventChannel : IEventChannel
    {
        public event EventHandler<SkillInformationEventArgs> SkillCanBeCasted;

        public event EventHandler<BuffInformationEventArgs> BuffInformationChanged;

        public event EventHandler<WorldInformationEventArgs> LocationChanged;

        public event AsyncEventHandler<HotkeyInformationEventArgs> HotkeyPressed;

        public event EventHandler<PlayerInformationEventArgs> PlayerInformationChanged;

        private readonly IList<int> subscribers = new List<int>(); // Items are Hashcodes of the subscribed delegates

        public void InvokeBuffInformationChanged(object sender, SkillInformationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void InvokeSkillCanBeCasted(object sender, SkillInformationEventArgs e)
        {
            SkillCanBeCasted?.Invoke(sender, e);
        }

        public void InvokeWorldInformationChanged(object sender, SkillInformationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public async Task InvokeHotkeyPressed(object sender, HotkeyInformationEventArgs e)
        {
            await InvokeAllAsynchronus(HotkeyPressed, sender, e);
        }

        public void InvokePlayerInformationChanged(object sender, PlayerInformationEventArgs e)
        {
            PlayerInformationChanged?.Invoke(sender, e);
        }

        public void Subscribe<T>(EventHandler<T> subscriber) where T : EventArgs
        {
            if (subscribers.Contains(subscriber.GetHashCode())) return;

            if (subscriber is EventHandler<SkillInformationEventArgs> skillHandler)
                SkillCanBeCasted += skillHandler;
            else if (subscriber is EventHandler<BuffInformationEventArgs> buffHandler)
                BuffInformationChanged += buffHandler;
            else if (subscriber is EventHandler<WorldInformationEventArgs> worldHandler)
                LocationChanged += worldHandler;
            else if (subscriber is EventHandler<PlayerInformationEventArgs> playerHandler)
                PlayerInformationChanged += playerHandler;

            subscribers.Add(subscriber.GetHashCode());
        }

        public void Subscribe<T>(AsyncEventHandler<T> subscriber) where T : EventArgs
        {
            if (subscribers.Contains(subscriber.GetHashCode())) return;

            if (subscriber is AsyncEventHandler<HotkeyInformationEventArgs> hotkeyHandler)
                HotkeyPressed += hotkeyHandler;

            subscribers.Add(subscriber.GetHashCode());
        }

        public void Unsubscribe<T>(EventHandler<T> subscriber) where T : EventArgs
        {
            if (!subscribers.Contains(subscriber.GetHashCode())) return;

            if (subscriber is EventHandler<SkillInformationEventArgs> skillHandler)
                SkillCanBeCasted += skillHandler;
            else if (subscriber is EventHandler<BuffInformationEventArgs> buffHandler)
                BuffInformationChanged -= buffHandler;
            else if (subscriber is EventHandler<WorldInformationEventArgs> worldHandler)
                LocationChanged -= worldHandler;
            else if (subscriber is EventHandler<PlayerInformationEventArgs> playerHandler)
                PlayerInformationChanged -= playerHandler;

            subscribers.Remove(subscriber.GetHashCode());
        }

        public void Unsubscribe<T>(AsyncEventHandler<T> subscriber) where T : EventArgs
        {
            if (!subscribers.Contains(subscriber.GetHashCode())) return;

            if (subscriber is AsyncEventHandler<HotkeyInformationEventArgs> hotkeyHandler)
                HotkeyPressed -= hotkeyHandler;

            subscribers.Remove(subscriber.GetHashCode());
        }

        // TODO: Untested. If this doesnt work, just go back to invoking the event as a whole
        private async Task InvokeAllAsynchronus<T>(AsyncEventHandler<T> toInvoke, object sender, T eventArgs) where T : EventArgs
        {
            if (toInvoke != null)
            {
                await Task.WhenAll(Array.ConvertAll(
                    toInvoke.GetInvocationList(),
                    x => ((AsyncEventHandler<T>)x).Invoke(sender, eventArgs)));
            }
        }
    }
}