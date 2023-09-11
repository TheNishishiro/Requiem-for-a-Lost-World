using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep : MonoBehaviour
{
    public GameObject nextStep;
    public bool IsTheEnd;

    public void Next()
    {
        gameObject.SetActive(false);
        nextStep.SetActive(!IsTheEnd);
    }
}
