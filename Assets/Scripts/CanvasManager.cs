
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.UI;

    public class CanvasManager : Singleton<CanvasManager> {
        [SerializeField] private CanvasScaler _canvasScaler;

        public float GetScalingFactor() => _canvasScaler.referenceResolution.y / Screen.height;
    }
