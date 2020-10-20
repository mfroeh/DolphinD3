using Dolphin.Enum;
using System.Collections.Generic;

namespace Dolphin
{
    public static class StaticResource
    {
        public static IDictionary<PlayerClass, IList<Enum.PlayerResource>> ResourceByClass = new Dictionary<PlayerClass, IList<Enum.PlayerResource>>
        {
            { PlayerClass.DemonHunterFemale, new List<Enum.PlayerResource>
            {
                Enum.PlayerResource.PrimaryHatred_0, Enum.PlayerResource.PrimaryHatred_20, Enum.PlayerResource.PrimaryHatred_40, Enum.PlayerResource.PrimaryHatred_60, Enum.PlayerResource.PrimaryHatred_80, Enum.PlayerResource.PrimaryHatred_100,
                Enum.PlayerResource.SecondaryDiscipline_0, Enum.PlayerResource.SecondaryDiscipline_20, Enum.PlayerResource.SecondaryDiscipline_40, Enum.PlayerResource.SecondaryDiscipline_60, Enum.PlayerResource.SecondaryDiscipline_80, Enum.PlayerResource.SecondaryDiscipline_100
            } },
            { PlayerClass.DemonHunterMale, ResourceByClass[PlayerClass.DemonHunterFemale] }
        };

        public static IDictionary<PlayerClass, IList<SkillName>> SkillsByClass = new Dictionary<PlayerClass, IList<SkillName>>
        {
            { PlayerClass.DemonHunterFemale, new List<SkillName> {
                SkillName.Companion, SkillName.FanOfKnives, SkillName.Preperation, SkillName.ShadowPower, SkillName.Strafe, SkillName.Vengeance
            } },
            { PlayerClass.DemonHunterMale, SkillsByClass[PlayerClass.DemonHunterFemale] }
        };
    }
}