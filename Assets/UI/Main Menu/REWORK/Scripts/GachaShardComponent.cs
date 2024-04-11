using System;
using DefaultNamespace.Data.Gacha;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class GachaShardComponent : MonoBehaviour
    {
        [SerializeField] private ParticleSystem shardParticles;
        [SerializeField] private ParticleSystem subParticles;

        public void SetColor(Color color)
        {
            var main = shardParticles.main;
            var sub = subParticles.main;
            sub.startColor = main.startColor = color; 
        }
    }
}