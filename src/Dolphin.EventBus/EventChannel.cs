using System;

namespace Dolphin.EventBus
{
    public class EventChannel : IEventChannel
    {
        public event EventHandler<SkillInformationEventArgs> SkillCanBeCasted;

        public event EventHandler<BuffInformationEventArgs> BuffInformationChanged;

        public event EventHandler<WorldInformationEventArgs> WorldInformationChanged;

        public event EventHandler<HotkeyInformationEventArgs> HotkeyPressed;

        public event EventHandler<PlayerInformationEventArgs> PlayerInformationChanged;

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

        public void InvokeHotkeyPressed(object sender, HotkeyInformationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void InvokePlayerInformationChanged(object sender, PlayerInformationEventArgs e)
        {
            PlayerInformationChanged?.Invoke(this, e);
        }
    }
}