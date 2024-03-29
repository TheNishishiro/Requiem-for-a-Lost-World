using UnityEngine;

namespace Objects.Stage
{
    [CreateAssetMenu]
    public class StageDefinition : ScriptableObject
    {
        public StageEnum id;
        public Sprite background;
        public string title;
    }
}