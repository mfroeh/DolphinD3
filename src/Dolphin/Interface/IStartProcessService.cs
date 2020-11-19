namespace Dolphin
{
    public interface IStartProcessService
    {
        void Restart(string path, string processName);

        void Start(string path);

        void StartIfNotRunning(string path, string processName);
    }
}