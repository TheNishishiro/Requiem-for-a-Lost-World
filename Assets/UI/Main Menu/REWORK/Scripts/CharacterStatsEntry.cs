using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStatsEntry : MonoBehaviour
{
    public TextMeshProUGUI labelValue;
    public Animator animator;

    public void Set(string text)
    {
        labelValue.text = text;
    }

    public void ToggleDescription()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            animator.SetTrigger("Expand");
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Expand"))
            animator.SetTrigger("Collapse");
    }
}
