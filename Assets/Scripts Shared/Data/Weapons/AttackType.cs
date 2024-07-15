using DefaultNamespace.Attributes;

namespace DefaultNamespace.Data.Weapons
{
    public enum AttackType
    {
        [StringValue("Unknown")]
        None = 0,
        [StringValue("Instance Damage")]
        Simple = 1,
        [StringValue("Pierce")]
        Pierce = 2,
        [StringValue("Follow up")]
        FollowUp = 3,
        [StringValue("AoE")]
        AreaOfEffect = 4,
        [StringValue("DoT")]
        DoT = 5,
        [StringValue("Support")]
        Support = 6,
    }
}