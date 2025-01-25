using System;
using Abstract;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MindMapNode : DraggableElement {
    public Information Information { get; private set; }
    private bool _initialized;

    [Header("References")] 
    private Image _image;

    protected override void Awake() {
        base.Awake();
        _image = GetComponent<Image>();
        Assert.IsNotNull(_image);
    }
    
    public void Initialize(Information information) {
        if(_initialized) throw new Exception("MindMapNode already initialized");
        Information = information;
        _image.sprite = Information.Sprite;
    } 
    
    protected override bool CanDrag(PointerEventData data) => true;
}