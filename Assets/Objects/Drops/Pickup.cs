using DefaultNamespace;
using Interfaces;
using Objects.Drops;
using Objects.Drops.ExpDrop;
using Objects.Players.Scripts;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Pickup : PickupBase
{
	private void Start()
	{
		Init();
	}

	private void Update()
	{
		FollowPlayerWhenClose();
	}

	private void OnTriggerEnter(Collider col)
	{
		OnCollision(col);
	}

	public void SetIsFollowingPlayer(bool isFollowingPlayer)
	{
		IsFollowingPlayer = isFollowingPlayer;
	}
}