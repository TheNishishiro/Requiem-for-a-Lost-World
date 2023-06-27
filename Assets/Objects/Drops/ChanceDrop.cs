using System;
using UnityEngine;

namespace Objects.Drops
{
	[Serializable]
	public class ChanceDrop
	{
		[SerializeField] public GameObject gameObject;
		[SerializeField] [Range(0f, 1f)] public float chance;
		[SerializeField] public int amount;
	}
}