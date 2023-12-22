using System;
using UnityEngine;

namespace DefaultNamespace.Data.VFX_Stages
{
    [Serializable]
    public class VfxStage
    {
        public GameObject stageObject;
        public float stageDuration;
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
        }

        public void Stop()
        {
            if (stageObject != null)
                stageObject.SetActive(false);
        }

        public bool IsFinished()
        {
            return _currentStageDuration <= 0 || stageDuration == 0;
        }
    }
}