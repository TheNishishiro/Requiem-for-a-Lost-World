using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.Data.VFX_Stages
{
    [Serializable]
    public class VfxStage
    {
        public GameObject stageObject;
        public float stageDuration;
        public List<ParticleSystem> particleSystems;
        private float _currentStageDuration;

        public void Update()
        {
            if (_currentStageDuration > 0)
                _currentStageDuration -= Time.deltaTime;
        }

        public void Play()
        {
            _currentStageDuration = stageDuration;

            if (stageObject != null)
                stageObject.SetActive(true);
            
            particleSystems?.ForEach(x =>
            {
                x.Simulate( 0.0f, true, true);
                x.Play(true);
            });
        }

        public void Stop()
        {
            if (stageObject != null)
                stageObject.SetActive(false);

            particleSystems?.ForEach(x =>
            {
                x.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            });
        }

        public bool IsFinished()
        {
            return _currentStageDuration <= 0 || stageDuration == 0;
        }
    }
}