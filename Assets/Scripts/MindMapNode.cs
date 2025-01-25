using System;
using Abstract;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MindMapNode : DraggableElement, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private Information DELETEME;
    public Information Information { get; private set; }
    private bool _initialized;

    [Header("References")] 
    private Image _image;
    public RectTransform RectTransform { get; private set; }

    protected override void Awake() {
        base.Awake();
        _image = GetComponent<Image>();
        Assert.IsNotNull(_image);
        RectTransform = GetComponent<RectTransform>();
        Initialize(DELETEME);
    }
    
    public void Initialize(Information information) {
        if(_initialized) throw new Exception("MindMapNode already initialized");
        Information = information;
        _image.sprite = Information.Sprite;
    } 
    
    protected override bool CanDrag(PointerEventData data) => true;
    
    public void OnPointerEnter(PointerEventData eventData) {
        if (dragging) return;
        TooltipManager.Instance.SelectNode(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        TooltipManager.Instance.UnselectNode(this);
    }

    public override void OnBeginDrag(PointerEventData eventData) {
        base.OnBeginDrag(eventData);
        TooltipManager.Instance.UnselectNode(this);
    }
}