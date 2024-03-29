using UnityEngine;
using UnityEngine.EventSystems;

public class UIParallax : MonoBehaviour
{
    [SerializeField] private float _parallaxStrength = 100f;
    [SerializeField] private Vector2 _parallaxClamp;

    private Vector2 _startPosition;
    private Camera _cam;

    private void Start()
    {
        _startPosition = transform.position;
        _cam = Camera.main;
    }

    private void Update()
    {
        Vector2 mousePos = _cam.ScreenToViewportPoint(Input.mousePosition);
        var posX = Mathf.Lerp(transform.position.x,
            _startPosition.x + (mousePos.x * _parallaxStrength),
            5f * Time.deltaTime);
        var posY = Mathf.Lerp(transform.position.y,
            _startPosition.y + (mousePos.y * _parallaxStrength),
            5f * Time.deltaTime);
        posX = Mathf.Clamp(posX,
            _startPosition.x - _parallaxClamp.x,
            _startPosition.x + _parallaxClamp.x);
        posY = Mathf.Clamp(posY,
            _startPosition.y - _parallaxClamp.y,
            _startPosition.y + _parallaxClamp.y);

        transform.position = new Vector3(posX, posY, transform.position.z);
    }
}