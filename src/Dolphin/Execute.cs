using System;
using System.Threading.Tasks;

namespace Dolphin
{
    public static class Execute
    {
        public static Task AndForgetAsync(Action action)
        {
            return Task.Run(action);
        }
    }
}