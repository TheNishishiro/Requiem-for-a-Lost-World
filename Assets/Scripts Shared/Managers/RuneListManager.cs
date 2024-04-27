using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Runes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class RuneListManager : MonoBehaviour
    {
        public static RuneListManager instance;
        [SerializeField] private List<RuneData> runes;

        public void Start()
        {
            if (instance == null)
                instance = this;
        }

        public List<RuneData> GetRunes()
        {
            return runes;
        }

        public RuneData GetRandomRune()
        {
            return runes.OrderBy(x => Random.value).FirstOrDefault();
        }
    }
}