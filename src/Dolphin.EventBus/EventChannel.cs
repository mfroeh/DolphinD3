using System;

namespace Dolphin.EventBus
{
    public class EventChannel : IEventChannel
    {
        public event EventHandler<SkillInformationEventArgs> SkillInformationChanged;

        public event EventHandler<BuffInformationEventArgs> BuffInformationChanged;

        public event EventHandler<WorldInformationEventArgs> WorldInformationChanged;

        public void InvokeBuffInformationChanged(object sender, SkillInformationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void InvokeSkillInformationChanged(object sender, SkillInformationEventArgs e)
        {
            SkillInformationChanged?.Invoke(sender, e);
        }

        public void InvokeWorldInformationChanged(object sender, SkillInformationEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}