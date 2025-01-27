using Interfaces;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Abstract {
    public abstract class DraggableElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        [SerializeField] protected MonoBehaviour scalerObject;
        protected RectTransform _rect;
        private Vector2 _offset;
        protected static bool dragging;
    
        protected virtual void Awake() {
            _rect = GetComponent<RectTransform>();
            Assert.IsNotNull(_rect);
        }
    
        public virtual void OnBeginDrag(PointerEventData data) {
            if (data.button == PointerEventData.InputButton.Right) return;
            if (!CanDrag(data)) return; 
            var customScale = scalerObject is IScaler scaler ? scaler.Scale : 1f;
            var canvasScale = CanvasManager.Instance.GetScalingFactor();
            _offset = _rect.anchoredPosition - (data.position / customScale * canvasScale);
            dragging = true;
        }
    
        public virtual void OnDrag(PointerEventData data) {
            if (!CanDrag(data) || !dragging) return;
            var customScale = scalerObject is IScaler scaler ? scaler.Scale : 1f;
            var canvasScale = CanvasManager.Instance.GetScalingFactor();
            _rect.anchoredPosition = (data.position / customScale * canvasScale) + _offset;
        }
        
        public virtual void OnEndDrag(PointerEventData eventData) {
            dragging = false;
        }
    
        protected abstract bool CanDrag(PointerEventData data);
    }
}