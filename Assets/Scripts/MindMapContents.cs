using Abstract;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

public class MindMapContents : DraggableElement, IScaler, IPointerClickHandler {
    public static MindMapContents Instance { get; private set; }
    protected void OnEnable() => Instance = this;
    
    public float Scale { get; set; } = 1f;
    
    [Header("Settings")]
    [SerializeField] private float zoomSpeed = 0.1f, minZoom = 0.5f, maxZoom = 2f;
    
    private void Update() {
        if (Input.mouseScrollDelta.y == 0) return;
        Scale = Mathf.Clamp(Scale + Input.mouseScrollDelta.y * zoomSpeed, minZoom, maxZoom);
        _rect.localScale = Vector3.one * Scale;
    }
    
    protected override bool CanDrag(PointerEventData data) {
        return data.pointerEnter == null || data.pointerEnter.GetComponent<MindMapNode>() == null;
    }

    public override void OnBeginDrag(PointerEventData data) {
        base.OnBeginDrag(data);
        MindMapNode.DisableLinkingNode();
    }
    
    public void OnPointerClick(PointerEventData eventData) {
        MindMapNode.DisableLinkingNode();
    }
}