using System;
using Data.Elements;
using Objects.Abilities;
using Objects.Items;
using Objects.Stage;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace UI.Labels.InGame.LevelUpScreen
{
	public class UpgradeEntry
	{
		public WeaponBase Weapon { get; set; }
		public UpgradeData Upgrade { get; set; }
		public ItemBase Item { get; set; }
		public ItemUpgrade ItemUpgrade { get; set; }
		public float ChanceOfAppearance { get; set; }
		public int Rarity { get; private set; } = (Random.value + GameData.GetPlayerCharacterData().Stats.Luck) switch
		{
			<= 0.75f =>  1,
			<= 0.85f => 2,
			<= 0.9f => 3,
			<= 0.97f => 4,
			_ => 5
		};

		public bool IsWeaponUpgrade => Weapon != null && Upgrade != null;
		public bool IsWeaponUnlock => Weapon != null && Upgrade == null;
		public bool IsItemUnlock => Item != null && ItemUpgrade == null;
		public bool IsItemUpgrade => Item != null && ItemUpgrade != null;

		public void LevelUp(WeaponManager weaponManager)
		{
			if (IsWeaponUpgrade)
			{
				weaponManager.UpgradeWeapon(Weapon, Upgrade, Rarity);
				return;
			}

			if (IsWeaponUnlock)
			{
				weaponManager.AddWeapon(Weapon, Rarity);
				return;
			}

			if (IsItemUnlock)
			{
				weaponManager.AddItem(Item, Rarity);
				return;
			}
			
			weaponManager.UpgradeItem(Item, ItemUpgrade, Rarity);
		}

		public string GetUnlockName()
		{
			if (IsWeaponUpgrade)
				return Upgrade.Name;
			if (IsWeaponUnlock)
				return Weapon.Name;
			if (IsItemUnlock)
				return Item.Name;
			if (IsItemUpgrade)
				return ItemUpgrade.Name;
			return null;
		}

		public string GetUnlockDescription()
		{
			if (IsWeaponUpgrade)
				return Upgrade.GetDescription(Rarity);
			if (IsWeaponUnlock)
				return Weapon.GetDescription(Rarity);
			if (IsItemUnlock)
				return Item.GetDescription(Rarity);
			if (IsItemUpgrade)
				return ItemUpgrade.GetDescription(Rarity);
			return null;
		}

		public Sprite GetUnlockIcon()
		{
			if (IsWeaponUpgrade || IsWeaponUnlock)
				return Weapon.Icon;
			if (IsItemUnlock || IsItemUpgrade)
				return Item.Icon;
			return null;
		}
		
		public Element GetElement()
		{
			if (IsWeaponUpgrade || IsWeaponUnlock)
				return Weapon.element;
			return Element.Disabled;
		}

		public Color GetUpgradeColor()
		{
			var color = Rarity switch
			{
				1 => Color.white,
				2 => Color.green,
				3 => Color.cyan,
				4 => Color.yellow,
				5 => Color.red,
				_ => Color.gray
			};
			
			return new Color(color.r, color.g, color.b,0.2f);
		}
	}
}