using Managers;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class SocialsManager : MonoBehaviour
    {
        public void OpenTwitter()
        {
            Application.OpenURL("https://twitter.com/for_requie65776");
            SaveManager.instance.GetSaveFile().MarkOpened(OpenedLinks.Twitter);
        }

        public void OpenItch()
        {
            Application.OpenURL("https://thenishishiro.itch.io/requiem-for-a-lost-world");
            SaveManager.instance.GetSaveFile().MarkOpened(OpenedLinks.ItchIo);
        }

        public void OpenDiscord()
        {
            Application.OpenURL("https://discord.gg/RgcwZn9JTQ");
            SaveManager.instance.GetSaveFile().MarkOpened(OpenedLinks.Discord);
        }

        public void OpenSteam()
        {
            Application.OpenURL("https://store.steampowered.com/app/3255660/Requiem_For_a_Lost_World/");
            SaveManager.instance.GetSaveFile().MarkOpened(OpenedLinks.Steam);
        }
    }
}