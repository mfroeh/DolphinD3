using Dolphin.Enum;
using System.Collections.Generic;

namespace Dolphin
{
    public interface IModelService
    {
        Player Player { get; }

        World World { get; }

        IEnumerable<PlayerHealth> PossibleHealthEnum();

        IEnumerable<Enum.PlayerResource> PossiblePrimaryResourceEnum();

        IEnumerable<Enum.PlayerResource> PossibleSecondaryResourceEnum();

        IEnumerable<SkillName> PossibleSkills(bool isMouse);

        Skill GetSkill(SkillName name);

        Skill GetSkill(int index);

        void SetPlayerHealth(PlayerHealth health);

        void SetPlayerPrimaryResource(Enum.PlayerResource primary);

        void SetPlayerSecondaryResource(Enum.PlayerResource secondary);

        void SetSkill(int index, Skill skill);
    }
}