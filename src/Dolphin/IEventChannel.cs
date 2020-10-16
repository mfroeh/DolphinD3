using System;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface IEventChannel
    {
        event AsyncEventHandler<SkillInformationEventArgs> SkillCanBeCasted;

        event EventHandler<BuffInformationEventArgs> BuffInformationChanged;

        event EventHandler<WorldInformationEventArgs> LocationChanged;

        event AsyncEventHandler<HotkeyInformationEventArgs> HotkeyPressed;

        event EventHandler<PlayerInformationEventArgs> PlayerInformationChanged;

        Task InvokeSkillCanBeCasted(object sender, SkillInformationEventArgs e);

        void InvokeBuffInformationChanged(object sender, SkillInformationEventArgs e);

        void InvokeWorldInformationChanged(object sender, SkillInformationEventArgs e);

        Task InvokeHotkeyPressed(object sender, HotkeyInformationEventArgs e);

        void InvokePlayerInformationChanged(object sender, PlayerInformationEventArgs e);

        void Subscribe<T>(EventHandler<T> subscriber) where T : EventArgs;
        void Subscribe<T>(AsyncEventHandler<T> subscriber) where T : EventArgs;
        void Unsubscribe<T>(EventHandler<T> subscriber) where T : EventArgs;
        void Unsubscribe<T>(AsyncEventHandler<T> subscriber) where T : EventArgs;
    }
}