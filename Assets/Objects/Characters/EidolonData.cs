using System;
using System.Text;
using DefaultNamespace.Data.Locale;
using NaughtyAttributes;
using UnityEngine;

namespace Objects.Characters
{
	[Serializable]
	public class EidolonData
	{
		public Sprite EidolonTexture;
		[SerializeField] 
		public string EidolonName;
		[ResizableTextArea] [SerializeField]
		public string EidolonDescription;
		[ResizableTextArea] [SerializeField]
		public string WeaponUpgradeDescription;
		[ResizableTextArea] [SerializeField]
		public string SkillUpgradeDescription;
		[ResizableTextArea] [SerializeField]
		public string PassiveUpgradeDescription;
		[ResizableTextArea] [SerializeField]
		public string EidolonQuote;
		
		public string GetDescription()
		{
			var sb = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(EidolonDescription))
				sb.AppendLine(GetEidolonDescription());
			if (!string.IsNullOrWhiteSpace(WeaponUpgradeDescription))
				sb.AppendLine(GetWeaponUpgradeDescription());
			if (!string.IsNullOrWhiteSpace(SkillUpgradeDescription))
				sb.AppendLine(GetSkillUpgradeDescription());
			if (!string.IsNullOrWhiteSpace(PassiveUpgradeDescription))
				sb.AppendLine(GetPassiveUpgradeDescription());

			return sb.ToString();
		}
		
		public string GetEidolonName()
		{
			return EidolonName.Translate();
		}
		
		public string GetEidolonDescription()
		{
			return EidolonDescription.Translate();
		}
		
		public string GetWeaponUpgradeDescription()
		{
			return WeaponUpgradeDescription.Translate();
		}
		
		public string GetSkillUpgradeDescription()
		{
			return SkillUpgradeDescription.Translate();
		}
		
		public string GetPassiveUpgradeDescription()
		{
			return PassiveUpgradeDescription.Translate();
		}
		
		public string GetEidolonQuote()
		{
			return EidolonQuote.Translate();
		}
	}
}