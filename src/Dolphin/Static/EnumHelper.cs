using System.Collections.Generic;
using System.Linq;

namespace Dolphin
{
    public static class EnumHelper
    {
        public static IEnumerable<TEnum> GetValues<TEnum>(bool excludeDefault = true) where TEnum : System.Enum
        {
            var values = System.Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            return !excludeDefault ? values : values.Where(x => !x.Equals(default(TEnum)));
        }
    }
}
