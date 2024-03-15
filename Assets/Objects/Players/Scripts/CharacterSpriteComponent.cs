using System;
using Objects.Stage;
using Unity.Netcode;
using UnityEngine;

namespace Objects.Players.Scripts
{
	public class CharacterSpriteComponent : MonoBehaviour
	{
		private void Awake()
		{
			//if (!IsOwner) return;
//
			//var characterSprite = GameData.GetPlayerSprite();
			//if (characterSprite != null)
			//	GetComponent<SpriteRenderer>().sprite = characterSprite;
		}
	}
}