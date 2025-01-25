
    using UnityEngine;

    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        public static T Instance { get; private set; }
        protected void OnEnable() {
            Instance = this as T;
        }
    }
