using UnityEngine;

namespace UI.Shared.Animations
{
    public class RotateAroundTarget : MonoBehaviour
    {
        public Transform target;
        public float speed = 20f;

        void Update()
        {
            transform.RotateAround(target.transform.position, target.transform.up, speed * Time.deltaTime);
        }
    }
}