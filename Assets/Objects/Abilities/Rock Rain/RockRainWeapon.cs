using DefaultNamespace;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Rock_Rain
{
    public class RockRainWeapon : PoolableWeapon<RockRainProjectile>
    {
        public bool IsHailstorm { get; set; }
        
        public override void SetupProjectile(NetworkProjectile networkProjectile)
        {
            var position = transform.position;
            var spawnPosition = new Vector3(position.x, position.y + 5.0f, position.z);
            var targetPosition = Utilities.GetPointOnColliderSurface(Utilities.GetRandomInArea(position, IsHailstorm ? 3f : 2f), transform);
            networkProjectile.Initialize(this, spawnPosition);
            networkProjectile.GetProjectile<RockRainProjectile>().SetDirection(targetPosition.x, targetPosition.y, targetPosition.z);
        }

        protected override void OnLevelUp()
        {
            if (LevelField == 7)
                IsHailstorm = true;
            
            base.OnLevelUp();
        }
    }
}