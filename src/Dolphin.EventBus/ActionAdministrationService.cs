using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dolphin.EventBus
{
    public class ActionAdministrationService : IActionAdministrationService, IEventSubscriber
    {
        private EventHandler<SkillInformationEventArgs> SkillCanBeCastedHandler;
        private EventHandler<WorldInformationEventArgs> LocationChangedHandler;
        private AsyncEventHandler<HotkeyInformationEventArgs> HotkeyPressedHandler;
        private EventHandler<PausedEventArgs> PausedHandler; // This subscribes to the event in the viewmodel

        private readonly IEventChannel eventChannel;
        private readonly IActionExecutionService actionService;
        private readonly Settings settings;
        private readonly ILogService logService;

        public ActionAdministrationService(IEventChannel eventChannel, Settings settings, IActionExecutionService actionExecutionService, ILogService logService)
        {
            this.eventChannel = eventChannel;
            this.actionService = actionExecutionService;
            this.settings = settings;
            this.logService = logService;

            SkillCanBeCastedHandler = SkillCanBeCasted;
            LocationChangedHandler = LocationChanged;

            eventChannel.Subscribe(SkillCanBeCastedHandler);
            eventChannel.Subscribe(LocationChangedHandler);
        }

        // TODO:
        private void LocationChanged(object sender, WorldInformationEventArgs e)
        {
            if (e.Location != WorldLocation.Rift && e.Location != WorldLocation.Grift)
            {
                eventChannel.Unsubscribe(SkillCanBeCastedHandler);
            }
        }

        private void SkillCanBeCasted(object sender, SkillInformationEventArgs e)
        {
            if (!settings.EnabledSkills.Contains(e.SkillName)) return;
            if (e.Index == -1) logService.AddEntry(this, "Recived SkillInformationEventArgs with Index - 1", LogLevel.Warning); // return;

            IntPtr handle = WindowHelper.GetHWND();
            Action action;
            if (e.Index <= 3)
                action = _Action.GetSkillCastAction(handle, settings.SkillKeybindings[e.Index]);
            else if (e.Index == 4)
                action = _Action.GetSkillCastAction(handle, MouseButtons.Left);
            else
                action = _Action.GetSkillCastAction(handle, MouseButtons.Right);

            var condition = e.SkillName.GetCondition();
            if (condition == null)
            {
                logService.AddEntry(this, $"{e.SkillName} has no Condition defined, defaulting to just return true.", LogLevel.Info);
                condition = (p, w) => true;
            }
            actionService.Execute(action, condition);
        }
    }
}
