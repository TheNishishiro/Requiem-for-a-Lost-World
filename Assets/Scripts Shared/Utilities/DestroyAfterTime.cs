using System;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    private float _currentLifeTime;

    private void Update()
    {
        _currentLifeTime += Time.deltaTime;
        if (_currentLifeTime >= lifeTime)
            Destroy(gameObject);
    }
}
