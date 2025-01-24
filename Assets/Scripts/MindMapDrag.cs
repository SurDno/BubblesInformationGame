using UnityEngine;
using UnityEngine.EventSystems;

// TODO: parent abstract class for draggables
public class MindMapDrag : MonoBehaviour, IBeginDragHandler, IDragHandler {
    private RectTransform rect;
    private Vector2 offset;
    
    private void Awake() {
        rect = GetComponent<RectTransform>();
    }
    
    public void OnBeginDrag(PointerEventData data) {
        if (!IsOverNode(data))
            offset = rect.anchoredPosition - data.position;
    }
    
    public void OnDrag(PointerEventData data) {
        if (!IsOverNode(data))
            rect.anchoredPosition = data.position + offset;
    }
    
    private bool IsOverNode(PointerEventData data) {
        return data.pointerEnter != null && data.pointerEnter.GetComponent<DraggableNode>() != null;
    }
}