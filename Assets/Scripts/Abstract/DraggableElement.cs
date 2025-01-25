using Interfaces;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Abstract {
    public abstract class DraggableElement : MonoBehaviour, IBeginDragHandler, IDragHandler {
        [SerializeField] private MonoBehaviour scalerObject;
        protected RectTransform _rect;
        private Vector2 _offset;
    
        protected virtual void Awake() {
            _rect = GetComponent<RectTransform>();
            Assert.IsNotNull(_rect);
        }
    
        public virtual void OnBeginDrag(PointerEventData data) {
            if (!CanDrag(data)) return; 
            var scale = scalerObject is IScaler scaler ? scaler.Scale : 1f;
            //Debug.Log($"{scalerObject is IScaler s} {scale}");
            _offset = _rect.anchoredPosition - (data.position / scale);
        }
    
        public virtual void OnDrag(PointerEventData data) {
            if (!CanDrag(data)) return;
            var scale = scalerObject is IScaler scaler ? scaler.Scale : 1f;
            _rect.anchoredPosition = (data.position / scale) + _offset;
        }
    
        protected abstract bool CanDrag(PointerEventData data);
    }
}