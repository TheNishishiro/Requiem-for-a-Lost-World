using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Data.Achievements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI achievementDescription;
    [SerializeField] private GameObject checkmark;
    public AchievementEnum Achievement { get; private set; }
    
    public void SetAchievement(AchievementEnum achievementId, string description, bool isCompleted)
    {
        Achievement = achievementId;
        achievementDescription.text = description;
        checkmark.SetActive(isCompleted);
    }

    public void MarkUnlocked()
    {
        checkmark.SetActive(true);
    }
}
