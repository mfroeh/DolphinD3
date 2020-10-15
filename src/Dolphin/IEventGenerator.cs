using System;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface IEventGenerator
    {
        Task GenerateEvent(EventArgs e);
    }
}