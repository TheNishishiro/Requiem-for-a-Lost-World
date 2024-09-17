using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Attributes;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Twitch;
using Interfaces;
using Managers.StageEvents;
using Objects.Characters;
using Objects.Enemies;
using Objects.Players.PermUpgrades;
using Objects.Stage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class TwitchEventsManager : MonoBehaviour
    {
        private float _nextEventTimer;

        private void Start()
        {
            RefreshTimer();
        }

        public void Update()
        {
            if (!TwitchIntegrationManager.instance.IsEnabledAndConnected())
                return;

            if (TwitchPollManager.instance.IsAnyPoolRunning())
                return;
            
            if (_nextEventTimer > 0)
            {
                _nextEventTimer -= Time.deltaTime;
                return;
            }

            RunEvent();
        }

        public void RunEvent()
        {
            var availableEvents = new List<TwitchEvent>();
            if (SaveFile.Instance.ConfigurationFile.TwitchStageRules)
                availableEvents.Add(TwitchEvent.StageRules);
            if (SaveFile.Instance.ConfigurationFile.TwitchSpawnEnemies)
                availableEvents.Add(TwitchEvent.SpawnEnemies);
            if (SaveFile.Instance.ConfigurationFile.TwitchRemoveItems)
                availableEvents.Add(TwitchEvent.RemoveItem);
            if (SaveFile.Instance.ConfigurationFile.TwitchControlBuffs)
            {
                availableEvents.Add(TwitchEvent.ControlBuffsIncrease);
                availableEvents.Add(TwitchEvent.ControlBuffsReduce);
            }
            if (SaveFile.Instance.ConfigurationFile.TwitchBanItems)
                availableEvents.Add(TwitchEvent.BanItems);
            
            var randomEvent = availableEvents.OrderBy(_ => Random.value).FirstOrDefault();
            switch (randomEvent)
            {
                case TwitchEvent.RemoveItem:
                    break;
                case TwitchEvent.ControlBuffsReduce:
                case TwitchEvent.ControlBuffsIncrease:
                    var statToUpgrade = Enum.GetValues(typeof(StatEnum)).Cast<StatEnum>().OrderBy(_ => Random.value).Take(4).ToList();
                    if (randomEvent == TwitchEvent.ControlBuffsIncrease)
                        TwitchPollManager.instance.StartPoll("Increase for 30 sec", statToUpgrade.Select(x => x.GetLongName()).ToArray(), RandomStatIncrease, statToUpgrade);
                    if (randomEvent == TwitchEvent.ControlBuffsReduce)
                        TwitchPollManager.instance.StartPoll("Reduce for 30 sec", statToUpgrade.Select(x => x.GetLongName()).ToArray(), RandomStatReduction, statToUpgrade);
                    break;
                case TwitchEvent.StageRules:
                    var randomRules = Utilities.RandomEnumValues<DefaultNamespace.Data.Stages.StageEvent>(4);
                    TwitchPollManager.instance.StartPoll("Change rules", randomRules.Select(x => x.GetStringValue()).ToArray(), RandomRuleChange, randomRules);
                    return;
                case TwitchEvent.SpawnEnemies:
                    var enemies = EnemyManager.instance.GetPossibleEnemies().OrderBy(_ => Random.value).Take(4).ToList();
                    TwitchPollManager.instance.StartPoll("Spawn 10-50 enemies", enemies.Select(x => x.enemyName).ToArray(), RandomEnemySpawn, enemies);
                    return;
                case TwitchEvent.BanItems:
                    var items = WeaponManager.instance.GetAvailableItemsAsInterface();
                    items.AddRange(WeaponManager.instance.GetAvailableWeaponsAsInterface());
                    var itemCount = items.Count > 4 ? 4 : items.Count;
                    if (itemCount == 0) break;
                    
                    var pollItems = items.OrderBy(_ => Random.value).Take(itemCount).ToList();
                    TwitchPollManager.instance.StartPoll("Ban item/weapon", pollItems.Select(x => x.NameField).ToArray(), BanItem, pollItems);
                    return;
            }
            RefreshTimer();
        }

        private void RandomStatIncrease(int result, List<StatEnum> stats)
        {
            var stat = stats[result];
            var statIncrease = stat.IsPercent() || stat == StatEnum.MovementSpeed  ? Random.value : Random.Range(1, 10);

            GameManager.instance.playerStatsComponent.TemporaryStatBoost(stat, statIncrease, 30f);
        }

        private void RandomStatReduction(int result, List<StatEnum> stats)
        {
            var stat = stats[result];
            var statIncrease = stat.IsPercent() || stat == StatEnum.MovementSpeed  ? Random.value : Random.Range(1, 10);
            GameManager.instance.playerStatsComponent.TemporaryStatBoost(stat, -statIncrease, 30f);
        }

        private void BanItem(int result, List<IPlayerItem> items)
        {
            WeaponManager.instance.BanItem(items[result]);
        }

        public void RandomEnemySpawn(int result, List<EnemyData> enemyData)
        {
            EnemyManager.instance.BurstSpawn(new List<EnemySpawnData>()
            {
                new()
                {
                    enemy = enemyData[result],
                    probability = 1f
                }
            }, Random.Range(10, 50));
            RefreshTimer();
        }

        public void RandomRuleChange(int result, List<DefaultNamespace.Data.Stages.StageEvent> stageEvents)
        {
            switch (stageEvents[result])
            {
                case DefaultNamespace.Data.Stages.StageEvent.SpawnRate:
                    EnemyManager.instance.ChangeSpawnRate(Random.Range(0.1f, 2f));
                    break;
                case DefaultNamespace.Data.Stages.StageEvent.Health:
                    EnemyManager.instance.ChangeHealthMultiplier(Random.Range(0.2f, 5f));
                    break;
                case DefaultNamespace.Data.Stages.StageEvent.Damage:
                    EnemyManager.instance.ChangeEnemyDamageMultiplier(Random.Range(0f, 10f));
                    break;
                case DefaultNamespace.Data.Stages.StageEvent.EraseEnemies:
                    EnemyManager.instance.EraseAllEnemies();
                    break;
                case DefaultNamespace.Data.Stages.StageEvent.EnemyMinCount:
                    EnemyManager.instance.ChangeMinimumEnemyCount(Random.Range(0, 300));
                    break;
            }
            RefreshTimer();
        }

        private void RefreshTimer()
        {
            _nextEventTimer = Random.Range(30, 60);
        }
    }
}