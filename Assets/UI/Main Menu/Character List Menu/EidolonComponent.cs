using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Objects.Characters;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UI.Main_Menu.Character_List_Menu;
using UnityEngine.EventSystems;

public class EidolonComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{   
    public Image TargetImage;
    public Image GlassImage;
    private static readonly int Mask1 = Shader.PropertyToID("_Mask");
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    private static readonly int MainColor = Shader.PropertyToID("_MainColor");
    [Tooltip("Maximum tilt angle towards the cursor in degrees")]
    public float maxTiltAngle = 15f;
    private RectTransform rectTransform;
    private bool isMouseOver = false;
    private Canvas canvas;
    private EidolonData eidolonData;
    public float idleMoveMagnitude = 10f;    // The magnitude of the idle movement
    public float idleMoveSpeed = 1f;         // The speed of the idle movement
    public float idleRotateSpeed = 30f;      // The speed of the idle rotation
    private Vector3 idlePositionOffset;      // The offset for the idle position
    private Quaternion idleRotationOffset; 
    private Vector3 originalPosition;
    private bool _isUnlocked;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
        
        idleMoveSpeed += GetInstanceID() % 10 / 10.0f;
        idleRotateSpeed += GetInstanceID() % 20 / 10.0f;
    }
    
    private void Update()
    {
        if (isMouseOver)
        {
            Vector2 localMousePosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, canvas.worldCamera, out localMousePosition))
            {
                // The mouse position (localMousePosition) is now in local space. Let's get its relative position in terms of the RectTransform's dimension (from -0.5 to 0.5).
                var relativePosition = new Vector2(
                    localMousePosition.x / rectTransform.rect.width,
                    localMousePosition.y / rectTransform.rect.height);

                // Normalize the vector. Here the magnitude may exceed 1 (since it's not a unit square) which can result in a tilt greater than max tilt angle.
                var directionToMouse = relativePosition.normalized;

                // Calculate the tilt angle
                var tiltX = directionToMouse.y * maxTiltAngle * Mathf.Abs(relativePosition.y);
                var tiltY = -directionToMouse.x * maxTiltAngle * Mathf.Abs(relativePosition.x);

                // Apply the tilt rotation.
                rectTransform.localRotation = Quaternion.Euler(tiltX, tiltY, 0f);
            }
        }
        else
        {
            rectTransform.localRotation = Quaternion.identity * idleRotationOffset;

            // Apply the idle position offset relative to the original position
            rectTransform.anchoredPosition3D = originalPosition + idlePositionOffset;
        }

        // Update the idle position/rotation offsets  
        idlePositionOffset = new Vector3
        (
            Mathf.PingPong(Time.time * idleMoveSpeed, idleMoveMagnitude) - idleMoveMagnitude / 2,
            Mathf.PingPong(Time.time * idleMoveSpeed, idleMoveMagnitude) - idleMoveMagnitude / 2,
            0f
        );

        idleRotationOffset = Quaternion.Euler(0f, 0f, Mathf.PingPong(Time.time * idleRotateSpeed, maxTiltAngle) - maxTiltAngle / 2);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        FindObjectOfType<EidolonDescriptionPanel>().Open(eidolonData, _isUnlocked);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }
    
    public void Setup(EidolonData eidolon, Color borderColor, bool isUnlocked)
    {
        _isUnlocked = isUnlocked;
        eidolonData = eidolon;
        TargetImage.sprite = eidolon.EidolonTexture;
        var clonedGlassMaterial = new Material(GlassImage.material);
        ApplyMaskToMaterial(clonedGlassMaterial, eidolon.EidolonTexture.texture);
        GlassImage.material = clonedGlassMaterial;
        
        var clonedMaterial = new Material(TargetImage.material);
        SetSpriteColor(clonedMaterial, isUnlocked);
        SetBorderColor(clonedMaterial, borderColor);
        TargetImage.material = clonedMaterial;
    }
    
    void ApplyMaskToMaterial(Material material, Texture mask)
    {
        material.SetTexture(Mask1, mask);
    }
    
    void SetBorderColor(Material material, Color color)
    {
        var factor = Mathf.Pow(2,5);
        Color hdrColor = new (color.r*factor,color.g*factor,color.b*factor);
        
        material.SetColor(Color1, hdrColor);
    }
    
    void SetSpriteColor(Material material, bool isUnlocked)
    {
        material.SetColor(MainColor, isUnlocked ? Color.white : Color.black);
    }
}