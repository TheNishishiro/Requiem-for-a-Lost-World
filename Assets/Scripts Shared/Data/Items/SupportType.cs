using DefaultNamespace.Attributes;

namespace DefaultNamespace.Data
{
    public enum SupportType
    {
        [StringValue("Unknown")]
        None = 0,
        [StringValue("Damage Buff")]
        DamageIncrease = 1,
        [StringValue("Utility")]
        Utility = 2,
        [StringValue("Survivability")]
        Survivability = 3,
    }
}