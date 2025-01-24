using Abstract;
using UnityEngine.EventSystems;

public class MindMapNode : DraggableElement {
    protected override bool CanDrag(PointerEventData data) => true;
}