using UnityEngine;
using UnityEngine.EventSystems;

// TODO: parent abstract class for draggables
public class DraggableNode : MonoBehaviour, IBeginDragHandler, IDragHandler {
    private RectTransform rect;
    private Vector2 offset;
    
    private void Awake() {
        rect = GetComponent<RectTransform>();
    }
    
    public void OnBeginDrag(PointerEventData data) {
        offset = rect.anchoredPosition - data.position;
    }
    
    public void OnDrag(PointerEventData data) {
        rect.anchoredPosition = data.position + offset;
    }
}