using Dolphin.Service;
using Newtonsoft.Json;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Unity.Lifetime;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Test
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var container = new UnityContainer();

            container.RegisterInstance(new Settings());

            container.RegisterType<EventBus, EventBus>(new ContainerControlledLifetimeManager());

            var eventBus = container.Resolve<IEventBus>();
            var subscription = new Subscription<PausedEvent>((object o, IEvent x) => Console.WriteLine());

            var cast = (ISubscription<IEvent>)subscription;
            eventBus.Subscribe(subscription);
        }

    }
}