using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Data.Statuses;
using UI.Labels.InGame.Status_Icon_Bar;
using UnityEngine;

namespace Managers
{
    public class StatusEffectManager : MonoBehaviour
    {
        [SerializeField] private StatusIconContainer statusIconContainer;
        public static StatusEffectManager instance;
        private Dictionary<StatusEffectType, int> _activeTempEffects;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void AddEffect(StatusEffectType statusEffect, int stackCount)
        {
            statusIconContainer.DisplayStatus(statusEffect, stackCount);
        }

        public void RemoveEffect(StatusEffectType statusEffect)
        {
            statusIconContainer.RemoveStatus(statusEffect);
        }

        public void AddOrRemoveEffect(StatusEffectType statusEffect, int stackCount)
        {
            if (stackCount > 0)
                AddEffect(statusEffect, stackCount);
            else
                RemoveEffect(statusEffect);
        }

        public void AddUniqueTemporaryEffect(StatusEffectType statusEffect, float duration)
        {
            _activeTempEffects ??= new Dictionary<StatusEffectType, int>();
            if (_activeTempEffects.ContainsKey(statusEffect)) return;

            _activeTempEffects.Add(statusEffect, 1);
            StartCoroutine(TempEffectProcess(statusEffect, duration));
        }

        public void AddTemporaryEffect(StatusEffectType statusEffect, float duration)
        {
            _activeTempEffects ??= new Dictionary<StatusEffectType, int>();
            if (!_activeTempEffects.TryAdd(statusEffect, 1))
            {
                _activeTempEffects[statusEffect]++;
            }
            StartCoroutine(TempEffectProcess(statusEffect, duration));
        }
        
        private IEnumerator TempEffectProcess(StatusEffectType statEnum, float duration)
        {
            AddEffect(statEnum, _activeTempEffects[statEnum]);
            yield return new WaitForSeconds(duration);
            _activeTempEffects[statEnum] -= 1;
            if (_activeTempEffects[statEnum] <= 0)
            {
                _activeTempEffects.Remove(statEnum);
                RemoveEffect(statEnum);
            }
        }
    }
}