using UnityEngine;
using UnityEngine.UI;

namespace Objects.Stage
{
    [CreateAssetMenu]
    public class StageDefinition : ScriptableObject
    {
        public StageEnum id;
        public Sprite background;
        public Sprite backgroundBlur;
        public string title;
    }
}