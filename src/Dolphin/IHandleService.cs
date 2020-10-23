using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface IHandleService
    {
        IntPtr GetHandle(string processName = "Dialo III64");

        void UpdateHandle(string processName = "Diablo III64");
    }
}
