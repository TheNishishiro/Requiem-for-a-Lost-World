using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Interfaces;
using Objects.Abilities.Back_Hole;
using UnityEngine;

public class ChaseComponent : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    private Transform targetDestination;
    private GameObject tempTarget;
    private float movementSpeed;
    private float tempSpeed;
    public bool FollowYAxis { get; set; }
    private float _immobileTimer;
    private float _slowTimer;
    private float _slowAmount;
    private bool _isMovementDisabled;
    private float _tempTargetTimer;
    private Transform transformCache;
    
    private void Awake()
    {
        transformCache = transform;
    }

    public void Clear()
    {
        tempTarget = null;
        _immobileTimer = 0;
        _slowTimer = 0;
        _slowAmount = 0;
        _isMovementDisabled = false;
        _tempTargetTimer = 0;
    }

    public void SetTarget(GameObject target)
    {
        targetDestination = target.transform;
    }

    public void SetTemporaryTarget(GameObject target, float? tempSpeed = null, float tempTargetCooldown = 0.2f)
    {
        tempTarget = target;
        _tempTargetTimer = tempTargetCooldown;
        this.tempSpeed = tempSpeed ?? movementSpeed;
    }

    public void SetSpeed(float speed)
    {
        movementSpeed = speed;
    }

    void Update()
    {
        if (targetDestination == null)
            return;
        
        if (_isMovementDisabled)
            return;

        var currentPosition = transformCache.position;

        var isTempTarget = false;
        if (_tempTargetTimer > 0)
        {
            isTempTarget = tempTarget != null && tempTarget.gameObject.activeInHierarchy;
            _tempTargetTimer-=Time.deltaTime;
        }

        var destination = isTempTarget ? tempTarget.transform.position : targetDestination.position;
        if (!FollowYAxis)
            destination.y = currentPosition.y;
        
        if (_immobileTimer > 0)
        {
            _immobileTimer -= Time.deltaTime;
            return;
        }        
        if (_slowTimer > 0)
        {
            _slowTimer -= Time.deltaTime;
        }

        if (!isTempTarget && Vector3.Distance(currentPosition, destination) > 12f)
        {
            currentPosition = Utilities.GetPointOnColliderSurface(destination - Utilities.GenerateRandomPositionOnEdge(new Vector2(8, 8)), transformCache, GetComponent<CapsuleCollider>().height);
        }

        var speed = (isTempTarget ? tempSpeed : movementSpeed) * (_slowTimer > 0 ? _slowAmount : 1.0f);
        transformCache.position = Vector3.MoveTowards(currentPosition, destination, speed * Time.deltaTime);
    }

    public void SetImmobile(float time)
    {
        if (_immobileTimer < time)
            _immobileTimer = time;
    }

    public void SetSlow(float time, float amount)
    {
        if (_slowTimer < time)
        {
            _slowTimer = time;
            _slowAmount = 1 - amount;
        }
    }

    public void SetMovementState(bool isDisabled)
    {
        _isMovementDisabled = isDisabled;
    }
}
