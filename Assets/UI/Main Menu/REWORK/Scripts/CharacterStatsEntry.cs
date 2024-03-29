using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStatsEntry : MonoBehaviour
{
    public TextMeshProUGUI labelValue;

    public void Set(string text)
    {
        labelValue.text = text;
    }
}
