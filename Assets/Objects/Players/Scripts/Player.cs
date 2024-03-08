using System;
using System.Collections;
using System.Collections.Generic;
using Objects.Players;
using Objects.Players.Scripts;
using Objects.Stage;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;


public class Player : MonoBehaviour
{
	[HideInInspector] public LevelComponent levelComponent;
	[HideInInspector] public HealthComponent healthComponent; 
	[HideInInspector] public PlayerStatsComponent playerStatsComponent;
	[HideInInspector] public PlayerVfxComponent playerVfxComponent;
	[HideInInspector] public Transform playerTransform;
	[SerializeField] public GameResultData gameResultData;
	[SerializeField] public GameObject reviveCardPrefab;
	private List<ulong> clientCards = new ();
	
	public PlayerCharacterState CharacterState { get; private set; }
	
	private void Start()
	{
		levelComponent = GetComponent<LevelComponent>();
		healthComponent = GetComponent<HealthComponent>();
		playerStatsComponent = GetComponent<PlayerStatsComponent>();
		playerVfxComponent = GetComponent<PlayerVfxComponent>();
		playerTransform = transform;
	}

	public int GetLevel()
	{
		return levelComponent.GetLevel();
	}
	
	public void AddExperience(float amount)
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

	public void SetCharacterState(PlayerCharacterState characterState)
	{
		CharacterState = characterState;
	}

	public void AddPlayerCard(ulong clientId)
	{
		if (!clientCards.Contains(clientId))
			clientCards.Add(clientId);
	}
}
