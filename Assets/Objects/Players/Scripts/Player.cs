﻿using System;
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

	public void TakeDamage(float amount)
	{
		if (amount > 0 && playerStatsComponent.GetDodgeChance() > Random.value)
			return;
        
		healthComponent.Damage(amount);
	}

	public void AddGold(int goldAmount)
	{
		var goldEarned = (int)Math.Ceiling(goldAmount * playerStatsComponent.GetItemRewardIncrease());
		gameResultData.AddGold(goldEarned);
	}

	public void AddGems(int gemAmount)
	{
		var gemsEarned = (int)Math.Ceiling(gemAmount * playerStatsComponent.GetItemRewardIncrease());
		gameResultData.AddGems(gemsEarned);
	}
}
