using Dolphin.Enum;

namespace Dolphin
{
    public interface ISettingsService
    {
        Settings Settings { get; }

        bool SkillIsEnabled(SkillName skill);
    }
}