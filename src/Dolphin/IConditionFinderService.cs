using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface IConditionFinderService
    {
        Func<bool> FindCondition(SkillName skillName);
    }
}
