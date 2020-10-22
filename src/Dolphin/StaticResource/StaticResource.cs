using Dolphin.Enum;
using System.Collections.Generic;
using System.Linq;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin
{
    public static class StaticResour // Makethis go await
    {
        public static IList<ItemType> ItemTypeList
        {
            get
            {
                var list = new List<ItemType>();
                foreach (var @enum in System.Enum.GetValues(typeof(ItemType)).Cast<ItemType>())
                {
                    if (@enum != ItemType.None)
                    {
                        list.Add(@enum);
                    }
                }

                return list;
            }
        }

        public static IDictionary<PlayerClass, IList<Enum.PlayerResource>> ResourceDictionary = new Dictionary<PlayerClass, IList<Enum.PlayerResource>>
        {
            { PlayerClass.DemonHunterFemale, new List<Enum.PlayerResource>
            {
                Enum.PlayerResource.PrimaryHatred_0, Enum.PlayerResource.PrimaryHatred_20, Enum.PlayerResource.PrimaryHatred_40, Enum.PlayerResource.PrimaryHatred_60, Enum.PlayerResource.PrimaryHatred_80, Enum.PlayerResource.PrimaryHatred_100,
                Enum.PlayerResource.SecondaryDiscipline_0, Enum.PlayerResource.SecondaryDiscipline_20, Enum.PlayerResource.SecondaryDiscipline_40, Enum.PlayerResource.SecondaryDiscipline_60, Enum.PlayerResource.SecondaryDiscipline_80, Enum.PlayerResource.SecondaryDiscipline_100
            } },
        };

        public static IDictionary<PlayerClass, IList<SkillName>> SkillDictionary = new Dictionary<PlayerClass, IList<SkillName>>
        {
            { PlayerClass.DemonHunterFemale, new List<SkillName> {
                SkillName.Companion, SkillName.FanOfKnives, SkillName.Preperation, SkillName.ShadowPower, SkillName.Vengeance
            } },
        };
    }
}