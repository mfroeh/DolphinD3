namespace Dolphin.Service
{
    public class SkillExecutionService : IEventSubscriber
    {
        private readonly IEventBus eventBus;
        private readonly Subscription<SkillCanBeCastedEvent> skillSubscription;

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