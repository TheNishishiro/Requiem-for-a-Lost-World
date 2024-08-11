using System;
using Data.Elements;
using Unity.Netcode;

namespace Objects.Enemies
{
    [Serializable]
    public struct ElementStatsSimple : INetworkSerializeByMemcpy, IEquatable<ElementStatsSimple>
    {
        public Element element;
        public float damageReduction;

        public bool Equals(ElementStatsSimple other)
        {
            return element == other.element && damageReduction.Equals(other.damageReduction);
        }

        public override bool Equals(object obj)
        {
            return obj is ElementStatsSimple other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)element, damageReduction);
        }
    }
}