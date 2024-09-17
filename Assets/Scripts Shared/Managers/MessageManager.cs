using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Data;
using TMPro;
using UI.Labels.InGame;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

namespace Managers
{
	public class MessageManager : NetworkBehaviour
	{
		public static MessageManager instance;
		[SerializeField] private GameObject damageMessage;
		private List<TextMeshPro> _messagePool;
		private readonly int _objectCount = 1000;
		private int _count;
		
		private void Start()
		{
			_messagePool = new List<TextMeshPro>();
			for (var i = 0; i < _objectCount; i++)
				Populate();
		}

		private void Awake()
		{
			instance = this;
		}

		public void Populate()
		{
			var go = Instantiate(damageMessage, transform);
			_messagePool.Add(go.GetComponent<TextMeshPro>());
			go.SetActive(false);
		}

		public void PostMessage(string text, Vector3 worldPosition, Quaternion rotation, Color color)
		{
			_messagePool[_count].gameObject.SetActive(SaveFile.Instance.ConfigurationFile.DamageNumbers != 0);
			_messagePool[_count].fontSize = SaveFile.Instance.ConfigurationFile.DamageNumbers switch
			{
				1 => 0.5f,
				2 => 1f,
				3 => 2f,
				_ => _messagePool[_count].fontSize
			};
			_messagePool[_count].color = color;
			_messagePool[_count].transform.position = Utilities.GetRandomInArea(worldPosition, 0.2f);
			_messagePool[_count].transform.localRotation = rotation;
			_messagePool[_count].text = text;
			_count += 1;
			if (_count >= _objectCount)
				_count = 0;
		}

		[Rpc(SendTo.Everyone)]
		public void PostMessageRpc(string text, Vector3 worldPosition, Quaternion rotation, Color color)
		{
			PostMessage(text, worldPosition, rotation, color);
		}
	}
}