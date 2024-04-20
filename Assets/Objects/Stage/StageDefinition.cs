using System;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.Stage
{
    [Serializable]
    public class StageDefinition
    {
        public StageEnum id;
        public Sprite background;
        public Sprite backgroundBlur;
        public string title;
    }
}