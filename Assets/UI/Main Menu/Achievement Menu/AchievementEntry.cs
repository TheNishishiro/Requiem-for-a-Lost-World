using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Data.Achievements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI achievementTitle;
    [SerializeField] private TextMeshProUGUI achievementDescription;
    [SerializeField] private GameObject checkmark;
    [SerializeField] private Image unlockIcon;
    public AchievementEnum Achievement { get; private set; }
    
    public void SetAchievement(AchievementEnum achievementId, string title, string description, bool isCompleted, Sprite icon)
    {
        Achievement = achievementId;
        achievementTitle.text = title;
        achievementDescription.text = description;
        unlockIcon.color = isCompleted ? Color.gray : Color.white;
        unlockIcon.sprite = icon;
        unlockIcon.gameObject.SetActive(icon != null);
        checkmark.SetActive(isCompleted);
    }

    public void MarkUnlocked()
    {
        checkmark.SetActive(true);
    }
}
