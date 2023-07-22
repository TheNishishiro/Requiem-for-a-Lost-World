using System.Collections;
using UnityEngine;

namespace Objects.Characters.Amelia.Skill
{
    public class AmeliaGlassShard : MonoBehaviour
    {
        private float rotationSpeed;
        private float radius;
        private Vector3 desiredPosition;
        private Vector3 rotationAxis;   
        private Transform parentTransform;  

        public void Initialize(Transform parent)
        {
            parentTransform = parent;
            rotationSpeed = Random.Range(60, 130);
            radius = Random.Range(0.1f, 0.3f);
            rotationAxis = Random.onUnitSphere;
            transform.position = (transform.position - parentTransform.position).normalized * radius + parentTransform.position;
        }
	
        void Update () {
            transform.RotateAround (parentTransform.position, rotationAxis, rotationSpeed * Time.deltaTime);
            desiredPosition = (transform.position - parentTransform.position).normalized * radius + parentTransform.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * rotationSpeed);
        }
    }
}
