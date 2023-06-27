using Objects.Abilities;
using Objects.Items;
using UnityEngine;
using Weapons;

namespace UI.Labels.InGame.LevelUpScreen
{
	public class UpgradeEntry
	{
		public WeaponBase Weapon { get; set; }
		public UpgradeData Upgrade { get; set; }
		public ItemBase Item { get; set; }
		public ItemUpgrade ItemUpgrade { get; set; }

		public bool IsWeaponUpgrade => Weapon != null && Upgrade != null;
		public bool IsWeaponUnlock => Weapon != null && Upgrade == null;
		public bool IsItemUnlock => Item != null && ItemUpgrade == null;
		public bool IsItemUpgrade => Item != null && ItemUpgrade != null;

		public void LevelUp(WeaponManager weaponManager)
		{
			if (IsWeaponUpgrade)
			{
				weaponManager.UpgradeWeapon(Weapon, Upgrade);
				return;
			}

			if (IsWeaponUnlock)
			{
				weaponManager.AddWeapon(Weapon);
				return;
			}

			if (IsItemUnlock)
			{
				weaponManager.AddItem(Item);
				return;
			}
			
			weaponManager.UpgradeItem(Item, ItemUpgrade);
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
				return Upgrade.Description;
			if (IsWeaponUnlock)
				return Weapon.Description;
			if (IsItemUnlock)
				return Item.Description;
			if (IsItemUpgrade)
				return ItemUpgrade.Description;
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
	}
}