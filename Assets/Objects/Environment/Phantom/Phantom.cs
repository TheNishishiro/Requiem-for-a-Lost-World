using System;
using System.Collections.Generic;
using DefaultNamespace.Data.Achievements;
using Managers;
using UnityEngine;

namespace Objects.Environment.Phantom
{
    public class Phantom : MonoBehaviour
    {
        [SerializeField] private List<Transform> pointsToVisit;
        [SerializeField] private float speed = 5f;  // Speed of the phantom

        private bool _isFollowingPath = false;
        private Rigidbody _rigidbody;
        private int _currentPointIndex = 0;
        private List<Transform> _visitedPoints = new ();

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isFollowingPath = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isFollowingPath = false;
            }
        }

        private void Update()
        {
            if (_isFollowingPath && pointsToVisit.Count > 0)
            {
                MoveTowardsPoint(pointsToVisit[_currentPointIndex]);
            }
        }

        private void MoveTowardsPoint(Transform point)
        {
            var direction = (point.position - transform.position).normalized;
            _rigidbody.MovePosition(transform.position + direction * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, point.position) < 0.1f)
            {
                _visitedPoints.Add(point);
                _currentPointIndex = (_currentPointIndex + 1) % pointsToVisit.Count;

                if (_currentPointIndex == 0 && _visitedPoints.Count == pointsToVisit.Count)
                {
                    AchievementManager.instance.UnlockAchievement(AchievementEnum.FollowThePhantom);
                }
            }
        }
    }
}