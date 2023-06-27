using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField] private bool fullBillboard;
    
    private void LateUpdate()
    {
        if (Time.frameCount % 5 != 0)
            return;
        
        if (!fullBillboard)
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        else
            transform.LookAt(Camera.main.transform);
    }
}
