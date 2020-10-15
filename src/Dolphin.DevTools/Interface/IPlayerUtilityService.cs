using System.Threading.Tasks;

namespace Dolphin.DevTools
{
    public interface IPlayerUtilityService
    {
        Task SavePlayerClass(string path);

        Task TestPlayerClassRecognition();

        Task SaveHealth(string path);

        Task TestHealthRecognition();

        Task SavePrimaryResource(string path);

        Task TestPrimaryResource();

        Task SaveSecondaryResource(string path);

        Task TestSecondaryResource();
    }
}