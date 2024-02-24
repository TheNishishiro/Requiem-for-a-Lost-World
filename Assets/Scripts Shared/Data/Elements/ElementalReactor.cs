using System.Collections.Generic;
using System.Linq;
using Objects.Characters;
using Objects.Stage;

namespace Data.Elements
{
	public class ElementalReactionComparer : IEqualityComparer<(Element, Element)>
	{
		public bool Equals((Element, Element) x, (Element, Element) y)
		{
			return x.Item1 == y.Item1 && x.Item2 == y.Item2 || x.Item1 == y.Item2 && x.Item2 == y.Item1;
		}

		public int GetHashCode((Element, Element) obj)
		{
			return obj.Item1.GetHashCode() ^ obj.Item2.GetHashCode();
		}
	}
	
	public static class ElementalReactor
	{
		private static readonly Dictionary<(Element, Element), ElementalReaction> Reactions = new (new ElementalReactionComparer())
			{
				{ (Element.Fire, Element.Ice), ElementalReaction.Melt },
				{ (Element.Fire, Element.Lightning), ElementalReaction.Explosion },
				{ (Element.Fire, Element.Wind), ElementalReaction.Swirl },
				{ (Element.Lightning, Element.Ice), ElementalReaction.Shock },
				{ (Element.Lightning, Element.Wind), ElementalReaction.Swirl },
				{ (Element.Wind, Element.Earth), ElementalReaction.Erode },
				{ (Element.Light, Element.Cosmic), ElementalReaction.Collapse },
				{ (Element.Fire, Element.Cosmic), ElementalReaction.Annihilation },
			};
		
		public static (ElementalReaction reaction, Element removedA, Element removedB) GetReaction(List<Element> activeElements)
		{
			if (activeElements == null) return (ElementalReaction.None, Element.None, Element.None);

			foreach (var a in activeElements)
			{
				foreach (var b in activeElements.Where(b => a != b))
				{
					if (!Reactions.TryGetValue((a, b), out var reaction)) continue;
					if (reaction == ElementalReaction.Annihilation && !GameData.IsCharacter(CharactersEnum.Nishi_HoF)) continue;
					
					activeElements.Remove(a);
					activeElements.Remove(b);
					return (reaction, a, b);
				}
			}

			return (ElementalReaction.None, Element.None, Element.None);
		}
	}
}