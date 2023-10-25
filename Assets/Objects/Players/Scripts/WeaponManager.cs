using System.Collections.Generic;
using System.Linq;
using Data.ToggleableEntries;
using DefaultNamespace.BaseClasses;
using DefaultNamespace.Data;
using Interfaces;
using Objects.Abilities;
using Objects.Items;
using Objects.Players.Scripts;
using Objects.Stage;
using UI.Labels.InGame.LevelUpScreen;
using UnityEngine;
using UnityEngine.Events;
using Weapons;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponContainer weapons;
    [SerializeField] private ItemContainer items;
    [SerializeField] private Transform weaponContainer;
    private List<WeaponToggleableEntry> availableWeapons;
    private List<ItemToggleableEntry> availableItems;
    private List<WeaponBase> _unlockedWeapons;
    private List<ItemBase> _unlockedItems;
    private PlayerStatsComponent _playerStatsComponent;
    public int maxWeaponCount = 6;
    public int maxItemCount = 6;
    private int _weaponsUpgraded;
    private int _itemsUpgraded;
    [SerializeField] private UnityEvent<WeaponBase, int, int> onWeaponAdded;
    [SerializeField] private UnityEvent<WeaponBase, int, int> onWeaponUpgraded;
    [SerializeField] private UnityEvent<ItemBase, int, int> onItemAdded;
    [SerializeField] private UnityEvent<ItemBase, int, int> onItemUpgraded;
    private SaveFile _saveFile;

    private void Awake()
    {
        _saveFile = FindObjectOfType<SaveFile>();
        _playerStatsComponent = FindObjectOfType<PlayerStatsComponent>();
        _unlockedWeapons = new List<WeaponBase>();
        _unlockedItems = new List<ItemBase>();

        availableWeapons = weapons.GetWeapons();
        availableItems = items.GetItems();
        
        var characterStartingWeapon = GameData.GetPlayerCharacterStartingWeapon() ?? availableWeapons.FirstOrDefault()?.weaponBase;
        AddWeapon(characterStartingWeapon, 1);
    }

    public void AddWeapon(WeaponBase weapon, int rarity)
    {
        var weaponGameObject = Instantiate(weapon, weaponContainer);
        weaponGameObject.ApplyRarity(rarity);
        availableWeapons.RemoveAll(x => x.weaponBase == weapon);
        _unlockedWeapons.Add(weaponGameObject);
        onWeaponAdded?.Invoke(weapon, _unlockedWeapons.Count, rarity);
    }

    public void AddItem(ItemBase item, int rarity)
    {
        var itemGameObject = Instantiate(item, weaponContainer);
        availableItems.RemoveAll(x => x.itemBase == item);
        itemGameObject.ApplyRarity(rarity);
        _playerStatsComponent.Apply(itemGameObject.ItemStats, 1);
        _unlockedItems.Add(itemGameObject);
        onItemAdded?.Invoke(item, _unlockedItems.Count, rarity);
    }

    public void UpgradeWeapon(WeaponBase weapon, UpgradeData upgradeData, int rarity)
    {
        weapon.Upgrade(upgradeData, rarity);
        onWeaponUpgraded?.Invoke(weapon, ++_weaponsUpgraded, rarity);
    }

    public void UpgradeItem(ItemBase itemBase, ItemUpgrade itemUpgrade, int rarity)
    {
        itemBase.RemoveUpgrade(itemUpgrade);
        itemBase.ApplyUpgrade(itemUpgrade, rarity);
        _playerStatsComponent.Apply(itemUpgrade.ItemStats, rarity);
        onItemUpgraded?.Invoke(itemBase, ++_itemsUpgraded, rarity);
    }
    
    public List<IPlayerItem> GetUnlockedWeaponsAsInterface()
    {
        return _unlockedWeapons.Cast<IPlayerItem>().ToList();
    }
    
    public List<IPlayerItem> GetUnlockedItemsAsInterface()
    {
        return _unlockedItems.Cast<IPlayerItem>().ToList();
    }

    public IEnumerable<UpgradeEntry> GetUpgrades()
    {
        var upgrades = new List<UpgradeEntry>();
        upgrades.AddRange(GetWeaponUnlocks());
        upgrades.AddRange(GetItemUnlocks());
        upgrades.AddRange(GetWeaponUpgrades());
        upgrades.AddRange(GetItemUpgrades());
        return upgrades;
    }
    
    public IEnumerable<UpgradeEntry> GetWeaponUpgrades()
    {
        return _unlockedWeapons.Select(unlockedWeapon => new UpgradeEntry()
        {
            Weapon = unlockedWeapon,
            Upgrade = unlockedWeapon.GetAvailableUpgrades().FirstOrDefault()
        }).Where(x => x.Upgrade != null);
    }
    
    public IEnumerable<UpgradeEntry> GetItemUpgrades()
    {
        return _unlockedItems.Select(unlockedItem =>
        {
            var nextUpgrade = unlockedItem.GetAvailableUpgrades().FirstOrDefault();
            if (nextUpgrade == null)
                return null;

            return new UpgradeEntry()
            {
                ItemUpgrade = nextUpgrade,
                Item = unlockedItem
            };
        }).Where(x => x != null);
    }
    
    public IEnumerable<UpgradeEntry> GetWeaponUnlocks()
    {
        if (_unlockedWeapons.Count >= maxWeaponCount)
            return new List<UpgradeEntry>();
        
        return availableWeapons.Where(x => x.isEnabled && x.weaponBase.IsUnlocked(_saveFile)).Select(availableWeapon => new UpgradeEntry()
        {
            Weapon = availableWeapon.weaponBase
        });
    }
    
    private IEnumerable<UpgradeEntry> GetItemUnlocks()
    {
        if (_unlockedItems.Count >= maxItemCount)
            return new List<UpgradeEntry>();
        
        return availableItems.Where(x => x.isEnabled && x.itemBase.IsUnlocked(_saveFile)).Select(availableItem => new UpgradeEntry()
        {
            Item = availableItem.itemBase
        });
    }
    
    public void ReduceWeaponCooldowns(float reductionPercentage)
    {
        foreach (var weapon in _unlockedWeapons)
        {
            weapon.ReduceCooldown(reductionPercentage);
        }
    }
}
