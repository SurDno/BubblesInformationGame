using Abstract;
using UnityEngine.EventSystems;

public class MindMapContents : DraggableElement {
    protected override bool CanDrag(PointerEventData data) {
        return data.pointerEnter == null || data.pointerEnter.GetComponent<MindMapNode>() == null;
    }
}