using System;

namespace Dolphin
{
    public interface IEventChannel
    {
        event EventHandler<SkillInformationEventArgs> SkillInformationChanged;

        event EventHandler<BuffInformationEventArgs> BuffInformationChanged;

        event EventHandler<WorldInformationEventArgs> WorldInformationChanged;

        void InvokeSkillInformationChanged(object sender, SkillInformationEventArgs e);

        void InvokeBuffInformationChanged(object sender, SkillInformationEventArgs e);

        void InvokeWorldInformationChanged(object sender, SkillInformationEventArgs e);
    }
}