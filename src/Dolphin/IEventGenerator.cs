using System;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface IEventGenerator
    {
        Task InvokeEventChannel(EventArgs e);
    }
}