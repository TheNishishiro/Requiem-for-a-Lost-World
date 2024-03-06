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
	    private ProjectileState _projectileState;
	    public ProjectileState State
	    {
		    get => _projectileState;
		    set
		    {
			    _projectileState = value;
			    networkProjectile?.SetNewState(_projectileState);
		    }
	    }
	    
	    protected Vector3 baseScale { get; private set; }
        protected Vector3 calculatedScale { get; private set; }
        protected float CurrentTimeToLive;
        protected float TimeAlive;
        [SerializeField] private NetworkProjectile networkProjectile;
        [SerializeField] protected Transform transformCache;
        [SerializeField] public bool UseParticles;
        [ShowIf("UseParticles")]
        [SerializeField] public ParticleSystem ParticleSystem;
        [SerializeField] private bool disableHitbox;
        [HideIf("disableHitbox")]
        [SerializeField] private Collider hitbox;
        [SerializeField] private VfxStage spawningStage;
        [SerializeField] private VfxStage flyingStage;
        [SerializeField] private VfxStage dissipationStage;
        [SerializeField] public bool isLimitedHitBox;
        [ShowIf("isLimitedHitBox")]
        [SerializeField] public float hitBoxDuration;
        [ShowIf("isLimitedHitBox")]
        [SerializeField] public float hitBoxStart;
        [SerializeField] public bool isChangeSizeOverLife;
        [ShowIf("isChangeSizeOverLife")]
        [SerializeField] public float increaseSizeUntilTime;
        [ShowIf("isChangeSizeOverLife")]
        [SerializeField] public float reduceSizeBeforeTime;
        [ShowIf("isChangeSizeOverLife")]
        [SerializeField] public Vector3 targetMinSize;
        [ShowIf("isChangeSizeOverLife")]
        [SerializeField] public bool multiplyTargetByScale;
        [SerializeField] private bool dontTickLifeTime;
        private Vector3 calculatedAnimationTargetSize;
        private bool _isInitialized;
        
        protected virtual void Awake()
        {
	        Init();
        }

        public void Init()
        {
	        if (_isInitialized) return;
	        _isInitialized = true;
	        if (transformCache == null)
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
	        calculatedScale = baseScale * WeaponStatsStrategy.GetScale();
	        calculatedAnimationTargetSize = !multiplyTargetByScale
		        ? targetMinSize
		        : new Vector3(targetMinSize.x * calculatedScale.x, targetMinSize.y * calculatedScale.y,
			        targetMinSize.z * calculatedScale.z);
	        
	        if (transformCache == null)
				transformCache = transform;
	        transformCache.localScale = calculatedScale;
            CurrentTimeToLive = GetTimeToLive();
            damageCooldown = WeaponStatsStrategy.GetDamageCooldown();
            currentPassedEnemies = WeaponStatsStrategy.GetPassThroughCount();
            StopAllCoroutines();
            IsDead = false;
            TimeAlive = 0;
            isDamageCooldownExpired = false;
            ProjectileDamageIncreasePercentage = 0;
            if (UseParticles)
            {
                ParticleSystem.Simulate( 0.0f, true, true );
                ParticleSystem.Play();
            }

            StopAllStages();

            State = ProjectileState.Unspecified;
            SetState(ProjectileState.Spawning);
        }

        public void StopAllStages()
        {
	        spawningStage.Stop();
	        flyingStage.Stop();
	        dissipationStage.Stop();
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
	        ResizeUpdate();
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

	    protected virtual void ResizeUpdate()
	    {
		    if (!isChangeSizeOverLife) return;
		    
		    if (TimeAlive < increaseSizeUntilTime)
		    {
			    transformCache.localScale = SetNewSize(calculatedAnimationTargetSize, true, increaseSizeUntilTime);
		    }
		    else if (CurrentTimeToLive < reduceSizeBeforeTime)
		    {
			    transformCache.localScale = SetNewSize(calculatedAnimationTargetSize, false, reduceSizeBeforeTime);
		    }
	    }

	    public void SetState(ProjectileState state)
	    {
		    ToggleHitBox(state);
		    if (State != state)
		    {
			    ToggleStage(state);
			    OnStateChanged(state);
		    }

		    State = state;
    	}

	    protected virtual void OnStateChanged(ProjectileState state)
	    {
		    
	    }

	    private void ToggleHitBox(ProjectileState state)
	    {
		    if (disableHitbox) return;
		    
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

	    public void ToggleStage(ProjectileState state)
	    {
		    if (state == this.State) return;
		    
		    flyingStage.Stop();
		    switch (state)
		    {
			    case ProjectileState.Spawning:
				    spawningStage.Play();
				    break;
			    case ProjectileState.Flying:
				    ResizeUpdate();
				    flyingStage.Play();
				    break;
			    case ProjectileState.Dissipating:
				    dissipationStage.Play();
				    break;
		    }
	    }

	    private void TickLifeTime()
	    {
		    if (dontTickLifeTime) return;
		    
		    CurrentTimeToLive -= Time.deltaTime;
			TimeAlive += Time.deltaTime;
		    if (isLimitedHitBox)
				UpdateCollider();

		    if (CurrentTimeToLive <= 0)
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
        
	    private Vector3 SetNewSize(Vector3 minTarget, bool isAnticipation, float expandTime)
	    {
		    return isAnticipation ? Vector3.Lerp(minTarget, calculatedScale, TimeAlive / expandTime) : Vector3.Lerp(minTarget, calculatedScale, CurrentTimeToLive / expandTime);
	    }
    }
}