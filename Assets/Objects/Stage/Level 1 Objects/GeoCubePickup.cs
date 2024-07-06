using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using UnityEngine;

namespace Objects.Stage.Level_1_Objects
{
    public class GeoCubePickup : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(!SaveFile.Instance.IsAchievementUnlocked(AchievementEnum.VisitStonehenge));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SaveFile.Instance.UnlockAchievement(AchievementEnum.VisitStonehenge);
                gameObject.SetActive(false);
            }
        }
    }
}