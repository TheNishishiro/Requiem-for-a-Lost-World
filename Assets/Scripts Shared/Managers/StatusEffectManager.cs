using DefaultNamespace.Data.Statuses;
using UI.Labels.InGame.Status_Icon_Bar;
using UnityEngine;

namespace Managers
{
    public class StatusEffectManager : MonoBehaviour
    {
        [SerializeField] private StatusIconContainer statusIconContainer;
        public static StatusEffectManager instance;
        
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
    }
}