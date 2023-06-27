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
    private bool _isMovementDisabled;
    
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

    public void SetTemporaryTarget(GameObject target, float? tempSpeed = null)
    {
        tempTarget = target;
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

        var isTempTarget = tempTarget != null;
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

        if (!isTempTarget && Vector3.Distance(transform.position, destination) > 12f)
        {
            transform.position =
                Utilities.GetPointOnColliderSurface(destination - Utilities.GenerateRandomPositionOnEdge(new Vector2(8, 8)), transform, GetComponentInChildren<BoxCollider>().size.y/2);
        }

        transform.LookAt(destination);
        transform.position = Vector3.MoveTowards(transform.position, destination, (isTempTarget ? tempSpeed : movementSpeed) * Time.deltaTime);
    }

    public void SetImmobile(float time)
    {
        if (_immobileTimer < time)
            _immobileTimer = time;
    }

    public void SetMovementState(bool isDisabled)
    {
        _isMovementDisabled = isDisabled;
    }
}
