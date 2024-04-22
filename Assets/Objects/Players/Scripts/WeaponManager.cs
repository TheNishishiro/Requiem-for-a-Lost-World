using System;
using System.Collections.Generic;
using System.Linq;
using Data.ToggleableEntries;
using DefaultNamespace.BaseClasses;
using DefaultNamespace.Data;
using Interfaces;
using Managers;
using Objects;
using Objects.Abilities;
using Objects.Items;
using Objects.Players.Containers;
using Objects.Players.Scripts;
using Objects.Stage;
using UI.In_Game.GUI.Scripts.Managers;
using UI.Labels.InGame.LevelUpScreen;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using Weapons;

public class WeaponManager : NetworkBehaviour
{
    public static WeaponManager instance;
    [SerializeField] private WeaponContainer weapons;
    [SerializeField] private ItemContainer items;
    [SerializeField] private PlayerStatsComponent _playerStatsComponent;
    private Transform _weaponContainer;
    public Dictionary<WeaponEnum, GameObject> weaponProjectilePrefabs;
    private List<WeaponToggleableEntry> availableWeapons;
    private List<ItemToggleableEntry> availableItems;
    private List<WeaponBase> _unlockedWeapons;
    private List<ItemBase> _unlockedItems;
    public int maxWeaponCount = 6;
    public int maxItemCount = 6;
    private int _weaponsUpgraded;
    private int _itemsUpgraded;
    private SaveFile _saveFile;
    private bool _isInitialized;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        Init();
    }

    public void Init()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        _saveFile = FindAnyObjectByType<SaveFile>();
        _unlockedWeapons = new List<WeaponBase>();
        _unlockedItems = new List<ItemBase>();
        weaponProjectilePrefabs = new Dictionary<WeaponEnum, GameObject>();

        availableWeapons = weapons.GetWeapons();
        foreach (var availableWeapon in availableWeapons)
        {
            weaponProjectilePrefabs.TryAdd(availableWeapon.weaponBase.WeaponId, availableWeapon.weaponBase.spawnPrefab);
        }
        
        availableItems = items.GetItems();
    }
    
    public void AddStartingWeapon(Transform weaponContainer)
    {
        _weaponContainer = weaponContainer;
        var characterStartingWeapon = GameData.GetPlayerCharacterStartingWeapon() ?? availableWeapons.FirstOrDefault()?.weaponBase;
        AddWeapon(characterStartingWeapon, 1);
    }

    public void AddWeapon(WeaponBase weapon, int rarity)
    {
        var weaponGameObject = Instantiate(weapon, _weaponContainer);
        weaponGameObject.ApplyRarity(rarity);
        availableWeapons.RemoveAll(x => x.weaponBase == weapon);
        _unlockedWeapons.Add(weaponGameObject);
        weaponGameObject.ActivateWeapon();
        AchievementManager.instance.OnWeaponUnlocked(weapon, _unlockedWeapons.Count, rarity);
        GuiManager.instance.UpdateItems();
    }

    public void AddItem(ItemBase item, int rarity)
    {
        var itemGameObject = Instantiate(item, _weaponContainer);
        availableItems.RemoveAll(x => x.itemBase == item);
        itemGameObject.ApplyRarity(rarity);
        _playerStatsComponent.Apply(itemGameObject.ItemStats, 1);
        _unlockedItems.Add(itemGameObject);
        AchievementManager.instance.OnItemUnlocked(item, _unlockedItems.Count, rarity);
        GuiManager.instance.UpdateItems();
    }

    public void UpgradeWeapon(WeaponBase weapon, UpgradeData upgradeData, int rarity)
    {
        weapon.Upgrade(upgradeData, rarity);
        GuiManager.instance.UpdateItems();
    }

    public void UpgradeItem(ItemBase itemBase, ItemUpgrade itemUpgrade, int rarity)
    {
        itemBase.RemoveUpgrade(itemUpgrade);
        itemBase.ApplyUpgrade(itemUpgrade, rarity);
        _playerStatsComponent.Apply(itemUpgrade.ItemStats, rarity);
        GuiManager.instance.UpdateItems();
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
            Upgrade = unlockedWeapon.GetAvailableUpgrades().FirstOrDefault(),
            ChanceOfAppearance = unlockedWeapon.chanceToAppear
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
                Item = unlockedItem,
                ChanceOfAppearance = unlockedItem.chanceToAppear
            };
        }).Where(x => x != null);
    }
    
    public IEnumerable<UpgradeEntry> GetWeaponUnlocks()
    {
        if (_unlockedWeapons.Count >= maxWeaponCount)
            return new List<UpgradeEntry>();
        
        return availableWeapons.Where(x => x.isEnabled && x.weaponBase.IsUnlocked(_saveFile)).Select(availableWeapon => new UpgradeEntry()
        {
            Weapon = availableWeapon.weaponBase,
            ChanceOfAppearance = availableWeapon.weaponBase.chanceToAppear
        });
    }
    
    private IEnumerable<UpgradeEntry> GetItemUnlocks()
    {
        if (_unlockedItems.Count >= maxItemCount)
            return new List<UpgradeEntry>();
        
        return availableItems.Where(x => x.isEnabled && x.itemBase.IsUnlocked(_saveFile)).Select(availableItem => new UpgradeEntry()
        {
            Item = availableItem.itemBase,
            ChanceOfAppearance = availableItem.itemBase.chanceToAppear
        });
    }
    
    public void ReduceWeaponCooldowns(float reductionPercentage)
    {
        foreach (var weapon in _unlockedWeapons)
        {
            weapon.ReduceCooldown(reductionPercentage);
        }
    }

    public WeaponBase GetUnlockedWeapon(WeaponEnum weaponId)
    {
        return _unlockedWeapons.FirstOrDefault(x => x.WeaponId == weaponId);
    }
}
