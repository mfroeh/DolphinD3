using Dolphin.Enum;

namespace Dolphin
{
    public interface IModelAdministrationService
    {
        Player Player { get; }
        World World { get; }

        Skill GetSkill(SkillName name);

        Skill GetSkill(int index);

        void SetSkill(int index, Skill skill);
    }
}