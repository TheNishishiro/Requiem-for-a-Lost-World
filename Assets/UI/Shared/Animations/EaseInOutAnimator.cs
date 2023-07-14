using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EaseInOutAnimator : MonoBehaviour
{
    public GameObject targetObject;
    public float animationDuration = 0.3f;
    public AnimationCurve easeInOutCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float preferredDelay = 0;
    public bool playOnAwake = false;
    public bool applyCurveAtStart = false;

    private Vector3 initialScale;

    private void Awake()
    {
        if (targetObject)
            initialScale = targetObject.transform.localScale;
       
        if (applyCurveAtStart)
            targetObject.transform.localScale = easeInOutCurve.Evaluate(0) * initialScale;
        if (playOnAwake)
            ShowPanel(preferredDelay);
    }

    public void ShowPanel(float delay)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateToObjectState(initialScale, delay));
    }

    public void HidePanel(float delay)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateToObjectState(Vector3.zero, delay));
    }


    private IEnumerator AnimateToObjectState(Vector3 targetScale, float delay)
    {
        var progress = 0f;
        var animationDelay = delay == 0 ? preferredDelay : delay;
        
        yield return new WaitForSeconds(animationDelay);
        while(progress < 1)
        {
            progress += Time.deltaTime / animationDuration;
            var evaluatedLerp = easeInOutCurve.Evaluate(progress);

            targetObject.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, evaluatedLerp);

            yield return null;
        }

        targetObject.transform.localScale = targetScale;
    }
}
