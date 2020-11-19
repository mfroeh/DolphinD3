using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Dolphin.Service
{
    public class StartProcessService : IStartProcessService
    {
        public void Restart(string path, string processName)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            foreach (var process in Process.GetProcessesByName(fileName))
            {
                process.Kill();
            }

            Start(path);
        }

        public void Start(string path)
        {
            try
            {
                Process.Start(path);
            }
            catch { }
        }

        public void StartIfNotRunning(string path, string processName)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);

            if (!Process.GetProcessesByName(fileName).Any())
            {
                Start(path);
            }
        }
    }
}