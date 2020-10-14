using System.Threading.Tasks;

namespace Dolphin.DevTools
{
    public interface ISaveSkillImages
    {
        Task SaveAllCurrentSkills(string path, bool byName);
    }
}
