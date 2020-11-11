namespace Dolphin
{
    public interface IStartProcessService
    {
        void Start(string path);

        void StartIfNotRunning(string path, string processName);

        void Restart(string path, string processName);
    }
}
