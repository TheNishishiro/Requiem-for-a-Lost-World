using DefaultNamespace.Data.VFX_Stages;
using Interfaces;
using NaughtyAttributes;
using Objects.Abilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Weapons
{
    public class StagableProjectile : DamageSource
    {
	    public ProjectileState State { get; private set; }
	    
        private Vector3 baseScale;
        protected float TimeToLive;
        protected float TimeAlive;
        protected Transform transformCache;
        [SerializeField] public bool UseParticles;
        [ShowIf("UseParticles")]
        [SerializeField] public ParticleSystem ParticleSystem;
        [SerializeField] private Collider hitbox;
        [SerializeField] private VfxStage spawningStage;
        [SerializeField] private VfxStage flyingStage;
        [SerializeField] private VfxStage dissipationStage;
        [SerializeField] public bool isLimitedHitBox;
        [ShowIf("isLimitedHitBox")]
        [SerializeField] public float hitBoxDuration;
        [ShowIf("isLimitedHitBox")]
        [SerializeField] public float hitBoxStart;
        
        protected virtual void Awake()
        {
	        transformCache = transform;
            var localScale = transformCache.localScale;
            baseScale = new Vector3(localScale.x,localScale.y,localScale.z);
        }
        
        public virtual void SetParentWeapon(WeaponBase parentWeapon, bool initStats = true)
        {
            ParentWeapon = parentWeapon;
            if (initStats)
				SetStats(parentWeapon.WeaponStatsStrategy);
        }
		
        public virtual void SetStats(IWeaponStatsStrategy weaponStatsStrategy)
        {
	        WeaponStatsStrategy = weaponStatsStrategy;
            transform.localScale = baseScale * WeaponStatsStrategy.GetScale();
            TimeToLive = GetTimeToLive();
            damageCooldown = WeaponStatsStrategy.GetDamageCooldown();
            currentPassedEnemies = WeaponStatsStrategy.GetPassThroughCount();
            StopAllCoroutines();
            IsDead = false;
            TimeAlive = 0;
            isDamageCooldownExpired = false;
            ProjectileDamageIncreasePercentage = 0;
            transformCache = transform;
            if (UseParticles)
            {
                ParticleSystem.Simulate( 0.0f, true, true );
                ParticleSystem.Play();
            }

            spawningStage.Stop();
            flyingStage.Stop();
            dissipationStage.Stop();
            
            SetState(ProjectileState.Spawning);
        }

        private void SpawnUpdate()
        {
	        spawningStage.Update();
	        if (!spawningStage.IsFinished()) return;
		    
	        spawningStage.Stop();
	        SetState(ProjectileState.Flying);
        }
	
        private void FlyUpdate()
        {
	        TickLifeTime();
	        TickDamageCooldown();
	        CustomUpdate();
        }
	
        private void DissipateUpdate()
        {
	        dissipationStage.Update();
	        if (!dissipationStage.IsFinished()) return;
	        
	        dissipationStage.Stop();
	        Destroy();
        }
        
        protected virtual float GetTimeToLive()
        {
            return WeaponStatsStrategy.GetTotalTimeToLive();
        }
        
        private void Update()
    	{
    	    switch (State)
    	    {
    	        case ProjectileState.Spawning:
    	            SpawnUpdate();
    	            break;
	
    	        case ProjectileState.Flying:
    	            FlyUpdate();
    	            break;
	
    	        case ProjectileState.Dissipating:
    	            DissipateUpdate();
    	            break;
    	    }
    	}

	    protected virtual void CustomUpdate()
	    {
	    }

	    public void SetState(ProjectileState state)
	    {
		    ToggleHitBox(state);
		    ToggleStage(state);
		    
    	    this.State = state;
    	}

	    private void ToggleHitBox(ProjectileState state)
	    {
		    switch (state)
		    {
			    case ProjectileState.Spawning:
			    case ProjectileState.Dissipating:
				    hitbox.enabled = false;
				    break;
	
			    case ProjectileState.Flying:
				    hitbox.enabled = !isLimitedHitBox;
				    break;
		    }
	    }

	    private void ToggleStage(ProjectileState state)
	    {
		    if (state == this.State) return;
		    
		    flyingStage.Stop();
		    switch (state)
		    {
			    case ProjectileState.Spawning:
				    spawningStage.Play();
				    break;
			    case ProjectileState.Flying:
				    flyingStage.Play();
				    break;
			    case ProjectileState.Dissipating:
				    dissipationStage.Play();
				    break;
		    }
	    }

	    private void TickLifeTime()
	    {
		    TimeToLive -= Time.deltaTime;
		    TimeAlive += Time.deltaTime;
		    if (isLimitedHitBox)
				UpdateCollider();

		    if (TimeToLive <= 0)
		    {
			    SetState(ProjectileState.Dissipating);
		    }
	    }
	    
	    protected void ReturnToPool<T>(ObjectPool<T> pool, T entity) where T : MonoBehaviour
	    {
		    pool.Release(entity);
	    }
	    
	    private void UpdateCollider()
	    {
		    if (TimeAlive > hitBoxDuration + hitBoxStart)
			    OnColliderEnd();
		    else if (TimeAlive >= hitBoxStart)
			    OnColliderStart();
	    }

	    private void OnColliderStart()
	    {
		    if (!hitbox.enabled)
			    hitbox.enabled = true;
	    }

	    private void OnColliderEnd()
	    {
		    if (hitbox.enabled)
			    hitbox.enabled = false;
	    }
    }
}