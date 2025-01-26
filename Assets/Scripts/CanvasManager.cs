using UnityEngine;
    using UnityEngine.UI;

    public class CanvasManager : Singleton<CanvasManager> {
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private Canvas _canvas;

        public float GetScalingFactor() => _canvasScaler.referenceResolution.y / Screen.height;
        
        public Vector2 GetMousePositionInRect(RectTransform obj) {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                obj, 
                Input.mousePosition,
                null, // No camera needed for Overlay
                out var mousePos
            );
            return mousePos;
        }

        public RectTransform GetCanvasTransform() => _canvas.transform as RectTransform;
    }
