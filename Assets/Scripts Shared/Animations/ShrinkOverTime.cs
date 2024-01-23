using System.Collections;
using UnityEngine;

public class ShrinkOverTime : MonoBehaviour
{
    [SerializeField] private float shrinkTimeInSeconds = 1f;
    [SerializeField] private float delayInSeconds = 0f;
    
    private void OnEnable()
    {
        transform.localScale = Vector3.one/0.75f;
        StartCoroutine(ShrinkWithDelay(shrinkTimeInSeconds, delayInSeconds));
    }

    private IEnumerator ShrinkWithDelay(float shrinkTimeInSeconds, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        var initialScale = transform.localScale;
        var targetScale = Vector3.zero;
        var elapsedTime = 0f;

        while (elapsedTime < shrinkTimeInSeconds)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / shrinkTimeInSeconds);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}