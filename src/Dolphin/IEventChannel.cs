using System;

namespace Dolphin
{
    public interface IEventChannel
    {
        event EventHandler<SkillInformationEventArgs> SkillCanBeCasted;

        event EventHandler<BuffInformationEventArgs> BuffInformationChanged;

        event EventHandler<WorldInformationEventArgs> WorldInformationChanged;

        event EventHandler<HotkeyInformationEventArgs> HotkeyPressed;

        void InvokeSkillCanBeCasted(object sender, SkillInformationEventArgs e);

        void InvokeBuffInformationChanged(object sender, SkillInformationEventArgs e);

        void InvokeWorldInformationChanged(object sender, SkillInformationEventArgs e);

        void InvokeHotkeyPressed(object sender, HotkeyInformationEventArgs e);
    }
}