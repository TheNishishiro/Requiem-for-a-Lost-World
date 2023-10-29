using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Interfaces;
using Objects.Abilities.Back_Hole;
using UnityEngine;

public class ChaseComponent : MonoBehaviour
{
    private Rigidbody _rigidbody;
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
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody is null)
            Debug.LogWarning($"Chasing object does not contain RigidBody '{gameObject.name}'");
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

        var isTempTarget = false;
        if (_tempTargetTimer > 0)
        {
            isTempTarget = tempTarget != null && tempTarget.gameObject.activeInHierarchy;
            _tempTargetTimer-=Time.deltaTime;
        }

        var destination = isTempTarget ? tempTarget.transform.position : targetDestination.position;
        if (!FollowYAxis)
            destination.y = transform.position.y;

        if (_isMovementDisabled)
            return;
        
        if (_immobileTimer > 0)
        {
            _immobileTimer -= Time.deltaTime;
            return;
        }        
        if (_slowTimer > 0)
        {
            _slowTimer -= Time.deltaTime;
        }

        if (!isTempTarget && Vector3.Distance(transform.position, destination) > 12f)
        {
            transform.position =
                Utilities.GetPointOnColliderSurface(destination - Utilities.GenerateRandomPositionOnEdge(new Vector2(8, 8)), transform, GetComponentInChildren<BoxCollider>().size.y/2);
        }
        
        transform.LookAt(destination);
        var speed = (isTempTarget ? tempSpeed : movementSpeed) * (_slowTimer > 0 ? _slowAmount : 1.0f);
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
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
