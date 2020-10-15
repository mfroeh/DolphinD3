using Dolphin.Enum;
using System.Collections.Generic;

namespace Dolphin
{
    public interface IModelAdministrationService
    {
        Player Player { get; }
        World World { get; }

        Skill GetSkill(SkillName name);

        Skill GetSkill(int index);

        void SetSkill(int index, Skill skill);

        IEnumerable<SkillName> GetPossibleSkills(); // TODO: Make dependent on player class

        IEnumerable<PlayerHealth> GetPossibleHealthPercentage();

        IEnumerable<PlayerResource> GetPossiblePrimaryResource();

        IEnumerable<PlayerResource> GetPossibleSecondaryResource();

        void SetPlayerHealth(PlayerHealth health);

        void SetPlayerPrimaryResource(PlayerResource primary);

        void SetPlayerSecondaryResource(PlayerResource secondary);
    }
}