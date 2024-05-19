using System;
using System.Collections;
using Managers;
using Objects.Players.Scripts;
using UnityEngine;

namespace Objects.Characters.Amelia_Alter.Skill
{
    public class AmelisanaSkill : MonoBehaviour
    {
        [SerializeField] private Material skyboxMaterial;
        [SerializeField] private Color atmosphericColor;
        
        private void OnEnable()
        {
            StartCoroutine(ControlEffects());
        }

        private IEnumerator ControlEffects()
        {
            gameObject.SetActive(true);
            GameManager.instance.playerComponent.SetCharacterState(PlayerCharacterState.Amelisana_Ultimate);
            var sceneLight = FindAnyObjectByType<Light>();
            var originalColor = sceneLight.color;
            var originalMaterial = RenderSettings.skybox;
            var originalFogColor = RenderSettings.fogColor;

            RenderSettings.skybox = skyboxMaterial;
            var time = 0f;
            const float step = 0.05f;
            while (time <= 1f)
            {
                time += step;
                var color = Color.Lerp(originalColor, atmosphericColor, time);
                sceneLight.color = color;
                RenderSettings.fogColor = color;
                yield return new WaitForSeconds(step);
            }
            
            yield return new WaitForSeconds(15);
            
            while (time > 0f)
            {
                time -= step;
                var color = Color.Lerp(originalColor, atmosphericColor, time);
                sceneLight.color = color;
                RenderSettings.fogColor = color;
                yield return new WaitForSeconds(step);
            }
            
            sceneLight.color = originalColor;
            RenderSettings.skybox = originalMaterial;
            RenderSettings.fogColor = originalFogColor;
            
            GameManager.instance.playerComponent.SetCharacterState(PlayerCharacterState.None);
            var characterScaler = PlayerStatsScaler.GetScaler();
            GameManager.instance.playerStatsComponent.SetHealth(characterScaler.GetHealth() + characterScaler.GetSpecialValue());
            characterScaler.ResetSpecialBar();
            gameObject.SetActive(false);
        }
    }
}