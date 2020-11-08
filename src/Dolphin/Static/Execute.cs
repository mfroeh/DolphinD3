using System;
using System.Threading.Tasks;
using System.Windows;

namespace Dolphin
{
    public static class Execute
    {
        public static void Action(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch { /* Swallow */ };
        }

        public static Task AndForgetAsync(Action action)
        {
            return Task.Run(() => Execute.Action(action));
        }

        public static void OnUIThread(Action action)
        {
            try
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
            catch { /* Swallow */ };
        }

        public static Task OnUIThreadAsync(Action action)
        {
            return Execute.AndForgetAsync(() => OnUIThread(action));
        }
    }
}