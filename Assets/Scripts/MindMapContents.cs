using Abstract;
using UnityEngine;
using UnityEngine.EventSystems;

public class MindMapContents : DraggableElement {
    private float _scale = 1f;
    
    [Header("Settings")]
    [SerializeField] private float zoomSpeed = 0.1f, minZoom = 0.5f, maxZoom = 2f;
    
    private void Update() {
        if (Input.mouseScrollDelta.y == 0) return;
        _scale = Mathf.Clamp(_scale + Input.mouseScrollDelta.y * zoomSpeed, minZoom, maxZoom);
        _rect.localScale = Vector3.one * _scale;
    }
    
    protected override bool CanDrag(PointerEventData data) {
        return data.pointerEnter == null || data.pointerEnter.GetComponent<MindMapNode>() == null;
    }
}