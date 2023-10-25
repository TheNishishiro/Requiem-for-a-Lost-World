using UnityEngine;

namespace DefaultNamespace.BaseClasses
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
    
        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                
                var newInstance = new GameObject(typeof(T).Name);
                instance = newInstance.AddComponent<T>();
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
        
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}