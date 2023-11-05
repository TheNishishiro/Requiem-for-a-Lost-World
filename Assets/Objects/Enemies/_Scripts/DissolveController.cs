using System;
using System.Collections;
using UnityEngine;

namespace Objects.Enemies
{
    public class DissolveController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void Clear()
        {
            spriteRenderer.material.SetFloat("_DissolveAmount", 0);
        }

        public void Dissolve(float dissolveTime)
        {
            StartCoroutine(DissolveCoroutine(dissolveTime));
        }

        private IEnumerator DissolveCoroutine(float dissolveTime)
        {
            var currentDissolveAmount = 0.0f;
            var step = 1 / (dissolveTime / 0.02f);
            var material = spriteRenderer.material;
            while (currentDissolveAmount < 1)
            {
                currentDissolveAmount += step;
                material.SetFloat("_DissolveAmount", currentDissolveAmount);
                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}