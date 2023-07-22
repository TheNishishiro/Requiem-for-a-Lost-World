using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events.Handlers;
using Events.Scripts;
using Objects.Characters;
using Objects.Characters.Amelia.Skill;
using Objects.Players.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class AmeliaGlassShield : CharacterSkillBase, IDamageTakenHandler
{
    [SerializeField] private int maxShardsCount;
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float offsetMin;
    [SerializeField] private float offsetMax;
    private List<AmeliaGlassShard> activeShards;
    
    private void Awake()
    {
        activeShards = new List<AmeliaGlassShard>(maxShardsCount);
        for (var i = 0; i < maxShardsCount; i++)
        {
            var go = Instantiate(shardPrefab, transform);
            go.transform.localPosition += new Vector3(Random.Range(offsetMin, offsetMax),Random.Range(offsetMin, offsetMax),Random.Range(offsetMin, offsetMax));
            var component = go.GetComponent<AmeliaGlassShard>();
            component.Initialize(transform);
            activeShards.Add(component);
        }
    }

    private void OnEnable()
    {
        DamageTakenStaticEvent.Register(this);
    }
    
    private void OnDisable()
    {
        DamageTakenStaticEvent.Unregister(this);
    }

    public void OnDamageTaken(float amount)
    {
        if (activeShards.Count == 0)
            return;
        
        var shard = activeShards.First();
        Destroy(shard.gameObject);
        activeShards.Remove(shard);
        
        FindObjectOfType<HealthComponent>().Damage(-amount);
    }
}
