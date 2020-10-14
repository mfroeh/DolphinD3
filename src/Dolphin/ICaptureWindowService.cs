using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface ICaptureWindowService
    {
        Task<Bitmap> CaptureWindow(string processName);

        Task<Bitmap> CaptureWindow(IntPtr hwnd);
    }
}