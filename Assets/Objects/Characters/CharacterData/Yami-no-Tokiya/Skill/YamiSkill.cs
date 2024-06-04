using UnityEngine;

namespace Objects.Characters.Yami_no_Tokiya.Skill
{
    public class YamiSkill : CharacterSkillBase
    {
        [SerializeField] private Material skyboxMaterial;
        [SerializeField] private LayerMask cameraCullingMask;
        private int _cullingMask;
        private Material _originalSkybox;

        protected override void Awake()
        {
            var camera = Camera.main;
            _originalSkybox = RenderSettings.skybox;
            _cullingMask = camera.cullingMask;
            camera.cullingMask = cameraCullingMask;
            RenderSettings.skybox = skyboxMaterial;
        }
        
        private void Update()
        {
            TickLifeTime();
        }

        protected override void OnDestroy()
        {
            Camera.main.cullingMask = _cullingMask;
            RenderSettings.skybox = _originalSkybox;
            base.OnDestroy();
        }
    }
}