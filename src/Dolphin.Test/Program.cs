using Dolphin.NewEventBus;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Dolphin.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Action<CancellationToken> action = (t) =>
            {
                for (int i = 0; i < 99; i++)
                {
                    if (t.IsCancellationRequested)
                    {
                        Trace.WriteLine("Cancellation requested!");
                        return;
                    }
                    Trace.WriteLine(i);
                    Thread.Sleep(200);
                }
            };

            var src = new CancellationTokenSource();

            Execute.AndForgetAsync(() => action.Invoke(src.Token));

            Thread.Sleep(2000);

            src.Cancel();
            Console.ReadLine();
        }
    }
}