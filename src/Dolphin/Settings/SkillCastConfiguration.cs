using System.Collections.Generic;
using System.Linq;

namespace Dolphin
{
    public class SkillCastConfiguration
    {
        public IDictionary<int, int> Delays { get; set; }

        public string Name { get; set; }

        public IList<int> SkillIndices { get; set; }

        // For Ui - I know this violates MVVM but I cba rightnow
        public string SkillsAndDelays
        {
            get
            {
                var @string = "{ ";
                if (Delays?.Count >= 1)
                {
                    foreach (var item in Delays.OrderBy(x => x.Key))
                    {
                        @string += $"{item.Key}.Skill: {item.Value}, ";
                    }
                }
                @string += "}";

                return @string;
            }
        }
    }
}