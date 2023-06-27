using System;
using Objects.Stage;
using UnityEngine;

namespace Objects.Players.Scripts
{
	public class CharacterSpriteComponent : MonoBehaviour
	{
		private void Awake()
		{
			var characterSprite = GameData.GetPlayerSprite();
			if (characterSprite != null)
				GetComponent<SpriteRenderer>().sprite = characterSprite;
		}
	}
}