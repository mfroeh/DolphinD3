using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin.NewEventBus
{
    public class SkillExecutionService : IEventSubscriber
    {
        private readonly Subscription<SkillCanBeCastedEvent> skillSubscription;

        private readonly IEventBus eventBus;

        public SkillExecutionService(IEventBus eventBus)
        {
            this.eventBus = eventBus;

            skillSubscription = new Subscription<SkillCanBeCastedEvent>(SkillCanBeCastedHandler);
            Subscribe(skillSubscription);
        }

        public void Subscribe<T>(Subscription<T> subscription) where T : IEvent
        {
            eventBus.Subscribe(subscription);
        }

        public void Unsubscribe<T>(Subscription<T> subscription) where T : IEvent
        {
            eventBus.Unsubscribe(subscription);
        }

        private void SkillCanBeCastedHandler(object o, SkillCanBeCastedEvent e)
        {
            // Cast the skill;
        }
    }
}
