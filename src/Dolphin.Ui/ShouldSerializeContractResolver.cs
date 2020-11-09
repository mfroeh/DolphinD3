using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Dolphin.Ui
{
    public class ShouldSerializeContractResolver : DefaultContractResolver
    {
        public static ShouldSerializeContractResolver Instance { get; } = new ShouldSerializeContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (typeof(Settings).IsAssignableFrom(member.DeclaringType) && member.Name == nameof(Settings.IsPaused))
            {
                property.Ignored = true;
            }
            else if (typeof(SkillCastSettings).IsAssignableFrom(member.DeclaringType) && member.Name == nameof(SkillCastSettings.SelectedSkillCastConfiguration))
            {
                property.Ignored = true;
            }

            return property;
        }
    }
}