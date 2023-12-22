using System;
using System.Text;
using NaughtyAttributes;
using UnityEngine;

namespace Objects.Characters
{
	[Serializable]
	public class EidolonData
	{
		public Sprite EidolonTexture;
		public string EidolonName;
		[ResizableTextArea]
		public string EidolonDescription;
		[ResizableTextArea]
		public string WeaponUpgradeDescription;
		[ResizableTextArea]
		public string SkillUpgradeDescription;
		[ResizableTextArea]
		public string PassiveUpgradeDescription;
		[ResizableTextArea]
		public string EidolonQuote;

		public string GetDescription()
		{
			var sb = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(EidolonDescription))
				sb.AppendLine(EidolonDescription);
			if (!string.IsNullOrWhiteSpace(WeaponUpgradeDescription))
				sb.AppendLine(WeaponUpgradeDescription);
			if (!string.IsNullOrWhiteSpace(SkillUpgradeDescription))
				sb.AppendLine(SkillUpgradeDescription);
			if (!string.IsNullOrWhiteSpace(PassiveUpgradeDescription))
				sb.AppendLine(PassiveUpgradeDescription);

			return sb.ToString();
		}
	}
}