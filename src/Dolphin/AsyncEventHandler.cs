using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin
{
    public delegate Task AsyncEventHandler<TEvent>(TEvent e) where TEventArgs : IEvent;
}
