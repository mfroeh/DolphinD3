using System;
using System.Threading.Tasks;
using System.Windows;

namespace Dolphin
{
    public static class Execute
    {
        public static void Action(Action action)
        {
            action.Invoke();
        }

        public static Task AndForgetAsync(Action action)
        {
            return Task.Run(action);
        }

        public static void OnUIThread(Action action)
        {
            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                action.Invoke();
            }
            else
            {
                dispatcher.Invoke(action);
            }
        }

        public static Task OnUIThreadAsync(Action action)
        {
            return Execute.AndForgetAsync(() => OnUIThread(action));
        }
    }
}