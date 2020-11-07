using System.Collections;
using System.Collections.Generic;

namespace Dolphin
{
    public class SkillCastConfiguration
    {
        public IList<int> SkillIndices { get; set; }

        public IDictionary<int, int> Delays { get; set; }
    }
}