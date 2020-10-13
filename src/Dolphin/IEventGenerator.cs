using System;

namespace Dolphin
{
    public interface IEventGenerator
    {
        void InvokeChannelEvent(EventArgs e);
    }
}