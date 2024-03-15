using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Interfaces;
using Objects.Abilities.Back_Hole;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class ChaseComponent : NetworkBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private NetworkTransform networkTransport;
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
    private bool _isPlayerControlled;
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
        _isPlayerControlled = false;
    }

    public void SetTarget(GameObject target)
    {
        if (target == null) return;
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
        if (!IsHost)
            return;

        if (_isMovementDisabled)
            return;
        
        if (targetDestination == null)
            targetDestination = FindObjectsByType<MultiplayerPlayer>(FindObjectsSortMode.None).FirstOrDefault(x => !x.isPlayerDead.Value)?.transform;

        var currentPosition = transformCache.position;

        var isTempTarget = false;
        if (_tempTargetTimer > 0)
        {
            isTempTarget = tempTarget != null && tempTarget.gameObject.activeInHierarchy;
            _tempTargetTimer-=Time.deltaTime;
        }

        if (_isPlayerControlled && targetDestination?.gameObject?.activeInHierarchy != true)
        {
            targetDestination = EnemyManager.instance.GetUncontrolledClosestEnemy(transform.position).transform;
            if (targetDestination == null)
                return;
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

        if (!isTempTarget && !_isPlayerControlled && Vector3.Distance(currentPosition, destination) > 12f)
        {
            currentPosition = Utilities.GetPointOnColliderSurface(destination - Utilities.GenerateRandomPositionOnEdge(new Vector2(8, 8)), transformCache, GetComponent<CapsuleCollider>().height);
            networkTransport.Teleport(currentPosition, Quaternion.identity, transformCache.localScale);
            return;
        }

        var speed = (isTempTarget ? tempSpeed : movementSpeed) * (_slowTimer > 0 ? _slowAmount : 1.0f) * (_isPlayerControlled ? 1.3f : 1.0f );
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

    public void MarkAsPlayerControlled(bool isControlled)
    {
        _isPlayerControlled = isControlled;
    }
}
