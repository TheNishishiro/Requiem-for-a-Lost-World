using UnityEngine;

public class VirtualJoystick : MonoBehaviour
{
    public RectTransform handle;
    private Vector2 joystickPosition = Vector2.zero;
    private int pointerId = -1;
    private Camera cam;

    void Start()
    {
        cam = Camera.main; 
        joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, handle.position);
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            // Convert the touch position to a local position with respect to the joystick
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                handle.parent as RectTransform, touch.position, cam, out localPoint);

            if (touch.phase == TouchPhase.Began && handle.rect.Contains(localPoint))
            {
                pointerId = touch.fingerId;
            }

            if (touch.fingerId == pointerId)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 direction = localPoint;
                    handle.anchoredPosition = direction / joystickPosition.magnitude;
                }
 
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    pointerId = -1; 
                    handle.anchoredPosition = Vector2.zero;
                }
            }
        }
    }
}