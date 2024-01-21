using System;
using System.Collections;
using Objects.Players;
using Objects.Players.Scripts;
using Objects.Stage;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class Player : MonoBehaviour
{
	[HideInInspector] public LevelComponent levelComponent;
	[HideInInspector] public HealthComponent healthComponent; 
	[HideInInspector] public PlayerStatsComponent playerStatsComponent;
	[HideInInspector] public Transform playerTransform;
	[SerializeField] public GameResultData gameResultData;
	
	private void Start()
	{
		tag = "Player";
		levelComponent = GetComponent<LevelComponent>();
		healthComponent = GetComponent<HealthComponent>();
		playerStatsComponent = GetComponent<PlayerStatsComponent>();
		playerTransform = transform;
	}

	public int GetLevel()
	{
		return levelComponent.GetLevel();
	}
	
	public void AddExperience(int amount)
	{
		levelComponent.AddExperience(amount);
	}

	public void TakeDamage(float amount, bool isIgnoreArmor = false, bool isPreventDeath = false)
	{
		if (amount > 0 && PlayerStatsScaler.GetScaler().GetDodgeChance() > Random.value)
			return;
        
		healthComponent.Damage(amount, isIgnoreArmor, isPreventDeath);
	}

	public void AddGold(int goldAmount)
	{
		var goldEarned = (int)Math.Ceiling(goldAmount * PlayerStatsScaler.GetScaler().GetItemRewardIncrease());
		gameResultData.AddGold(goldEarned);
	}

	public void AddGems(int gemAmount)
	{
		var gemsEarned = (int)Math.Ceiling(gemAmount * PlayerStatsScaler.GetScaler().GetItemRewardIncrease());
		gameResultData.AddGems(gemsEarned);
	}
}
