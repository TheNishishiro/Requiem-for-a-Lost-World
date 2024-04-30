using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UISquareTransform : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector2 targetSize = new Vector2(300, 300);
    public float transitionSpeed = 2f; 
    private Vector2 originalSize;
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private int initialOrder;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalSize = rectTransform.sizeDelta;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        canvas.sortingOrder = 1;
        
        StopAllCoroutines(); 
        StartCoroutine(LerpSize(targetSize));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canvas.sortingOrder = 0;
        StopAllCoroutines(); 
        StartCoroutine(LerpSize(originalSize));
    }
    
    IEnumerator LerpSize(Vector2 target)
    {
        while (Vector2.Distance(rectTransform.sizeDelta, target) > 0.01f)
        {
            rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, target, transitionSpeed * Time.deltaTime);
            yield return null;
        }
        rectTransform.sizeDelta = target; // Ensures precise setting of target size
    }
}