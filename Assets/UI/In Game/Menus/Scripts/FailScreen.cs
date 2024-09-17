using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.In_Game.Menus.Scripts
{
    public class FailScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tipText;
        [SerializeField] private List<string> tips;

        private void OnEnable()
        {
            tipText.text = tips.OrderBy(_ => Random.value).First();
        }
    }
}