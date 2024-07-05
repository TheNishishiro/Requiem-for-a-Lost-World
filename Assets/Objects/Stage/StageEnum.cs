using System;
using DefaultNamespace.Attributes;

namespace Objects.Stage
{
    public enum StageEnum
    {
        [StringValue("Scenes/Main Level")]
        CapitalOutskirts = 1,
        [StringValue("Scenes/Capital Undergrounds Level")]
        CapitalUndergrounds = 2
    }
}