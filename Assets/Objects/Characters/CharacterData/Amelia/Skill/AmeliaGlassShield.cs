using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events.Handlers;
using Events.Scripts;
using Managers;
using Objects.Characters;
using Objects.Characters.Amelia.Skill;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;
using Random = UnityEngine.Random;

public class AmeliaGlassShield : CharacterSkillBase, IDamageTakenHandler, ISpecialBarFilledHandler, IExpPickedUpHandler
{
    [SerializeField] private GameObject shardPrefab;
    private const float SpawnOffset = 0.2f;
    private List<AmeliaGlassShard> _activeShards = new ();
    private SpecialBarManager _specialBarManager;
    private static int MaxShardsCount => GameData.GetPlayerCharacterRank() == CharacterRank.E5 ? 16 : 12;

    public SpecialBarManager SpecialBarManager
    {
        get
        {
            if (_specialBarManager == null)
                _specialBarManager = FindObjectOfType<SpecialBarManager>();
            return _specialBarManager;
        }
    }
    
    private void Awake()
    {
        _activeShards = new List<AmeliaGlassShard>();
    }

    private void OnEnable()
    {
        DamageTakenEvent.Register(this);
        SpecialBarFilledEvent.Register(this);
        ExpPickedUpEvent.Register(this);
    }
    
    private void OnDisable()
    {
        DamageTakenEvent.Unregister(this);
        SpecialBarFilledEvent.Unregister(this);
        ExpPickedUpEvent.Unregister(this);
    }

    public void OnDamageTaken(float amount)
    {
        if (_activeShards.Count == 0)
            return;
        
        var shard = _activeShards.First();
        shard.Shatter();
        _activeShards.Remove(shard);
        
        FindObjectOfType<HealthComponent>().Damage(-amount);
    }

    public void OnSpecialBarFilled()
    {
        if (_activeShards.Count >= MaxShardsCount)
            return;
        
        SpawnShard();
        SpecialBarManager.ResetBar();
    }
    
    private void SpawnShard()
    {
        var go = Instantiate(shardPrefab, transform);
        go.transform.localPosition += new Vector3(Random.Range(-SpawnOffset, SpawnOffset),Random.Range(-SpawnOffset, SpawnOffset),Random.Range(-SpawnOffset, SpawnOffset));
        var component = go.GetComponent<AmeliaGlassShard>();
        if (component == null)
            Destroy(go);
        component.Initialize(transform);
        _activeShards.Add(component);
    }

    public void OnExpPickedUp(float amount)
    {
        SpecialBarManager.Increment();
    }

    public void SpawnShards(int i)
    {
        for (var j = 0; j < i; j++)
        {
            if (_activeShards.Count >= MaxShardsCount)
                return;
            
            SpawnShard();
        }
    }
}
