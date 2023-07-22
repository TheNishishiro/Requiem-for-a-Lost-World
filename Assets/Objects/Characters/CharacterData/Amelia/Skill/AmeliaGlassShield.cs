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
    [SerializeField] private int maxShardsCount;
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float offsetMin;
    [SerializeField] private float offsetMax;
    private List<AmeliaGlassShard> _activeShards = new ();
    private SpecialBarManager _specialBarManager;

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
        _activeShards = new List<AmeliaGlassShard>(maxShardsCount);
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
        Destroy(shard.gameObject);
        _activeShards.Remove(shard);
        
        FindObjectOfType<HealthComponent>().Damage(-amount);
    }

    public void OnSpecialBarFilled()
    {
        if (_activeShards.Count >= maxShardsCount)
            return;
        
        SpawnShard();
        SpecialBarManager.ResetBar();
    }
    
    private void SpawnShard()
    {
        var go = Instantiate(shardPrefab, transform);
        go.transform.localPosition += new Vector3(Random.Range(offsetMin, offsetMax),Random.Range(offsetMin, offsetMax),Random.Range(offsetMin, offsetMax));
        var component = go.GetComponent<AmeliaGlassShard>();
        component.Initialize(transform);
        _activeShards.Add(component);
    }

    public void OnExpPickedUp(float amount)
    {
        SpecialBarManager.Increment();
    }
}
