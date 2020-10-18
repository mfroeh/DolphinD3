using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dolphin.NewEventBus
{
    public static class Execute
    {
        public static void AndForgetAsync(Action action)
        {
            Task.Run(action);
        }

        public static void AndForgetAsync(Subscription )
    }
}