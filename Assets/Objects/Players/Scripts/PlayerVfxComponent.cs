using System.Collections;
using UnityEngine;

namespace Objects.Players.Scripts
{
    public class PlayerVfxComponent : MonoBehaviour
    {
        [SerializeField] private GameObject nishiVoidTransformVfx;
        [SerializeField] private GameObject nishiFlameTransformVfx;

        public void PlayVoidTransform()
        {
            StartCoroutine(PlayVfx(nishiVoidTransformVfx, 1f));
        }

        public void PlayFlameTransform()
        {
            StartCoroutine(PlayVfx(nishiFlameTransformVfx, 1f));
        }

        private IEnumerator PlayVfx(GameObject go, float time)
        {
            go.SetActive(true);
            yield return new WaitForSeconds(time);
            go.SetActive(false);
        }
    }
}