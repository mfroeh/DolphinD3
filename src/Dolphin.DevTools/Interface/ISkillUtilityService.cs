using System.Threading.Tasks;

namespace Dolphin.DevTools
{
    public interface ISkillUtilityService
    {
        Task SaveCurrentSkills(string path);

        Task TestSkillRecognition();
    }
}