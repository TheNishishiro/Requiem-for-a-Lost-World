using UnityEngine;

namespace DefaultNamespace.Animations
{
    public class AnimateOnEnable : MonoBehaviour
    {
        public Animator animator;
        public string animationState;
        

        void OnEnable()
        {
            animator.Play(animationState);
        }
    }
}