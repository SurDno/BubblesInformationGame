using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Abstract {
    public abstract class DraggableElement : MonoBehaviour, IBeginDragHandler, IDragHandler {
        protected RectTransform _rect;
        private Vector2 _offset;
    
        protected virtual void Awake() {
            _rect = GetComponent<RectTransform>();
            Assert.IsNotNull(_rect);
        }
    
        public virtual void OnBeginDrag(PointerEventData data) {
            if (!CanDrag(data)) return; 
            _offset = _rect.anchoredPosition - data.position;
        }
    
        public virtual void OnDrag(PointerEventData data) {
            if (!CanDrag(data)) return;
            _rect.anchoredPosition = data.position + _offset;
        }
    
        protected abstract bool CanDrag(PointerEventData data);
    }
}