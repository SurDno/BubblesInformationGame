using System;
using System.Collections.Generic;
using Abstract;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MindMapNode : DraggableElement, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    [SerializeField] private Information DELETEME;
    public Information Information { get; private set; }
    private bool _initialized;

    [Header("References")] 
    private Image _image;   
    [SerializeField] private LineRendererUi linkPrefab;
    
    public List<(LineRendererUi lr, MindMapNode mmn)> Links = new();
    private static MindMapNode linkingNode;
    private static LineRendererUi tempRenderer;
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
        //_image.sprite = Information.Sprite;
    } 
    
    protected override bool CanDrag(PointerEventData data) => true;
    
    public void OnPointerEnter(PointerEventData eventData) {
        if (dragging || linkingNode != null) return;
        TooltipManager.Instance.SelectNode(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        TooltipManager.Instance.UnselectNode(this);
    }

    public override void OnBeginDrag(PointerEventData eventData) {
        base.OnBeginDrag(eventData);
        TooltipManager.Instance.UnselectNode(this);
    }

    public void OnPointerClick(PointerEventData eventData) {
        switch (eventData.button) {
            case PointerEventData.InputButton.Left:
                if (!linkingNode) return;

                foreach (var (lr, mmn) in Links) {
                    if (mmn != linkingNode) continue;
                    DisableLinkingNode();
                    return;
                }
                var lineRendererUi = Instantiate(linkPrefab, transform.parent)
                    .Initialize(linkingNode.RectTransform, RectTransform);
                lineRendererUi.transform.SetAsFirstSibling();
                Debug.Log(lineRendererUi, linkingNode);
                Links.Add((lineRendererUi, linkingNode));
                linkingNode.Links.Add((lineRendererUi, this));
                DisableLinkingNode();
                break;
            case PointerEventData.InputButton.Right:
                DisableLinkingNode();
                linkingNode = this;
                tempRenderer = Instantiate(linkPrefab, transform.parent).Initialize(RectTransform, null);
                tempRenderer.GetComponent<Image>().raycastTarget = false;
                break;
        }
    }

    public static void DisableLinkingNode() {
        linkingNode = null;
        if(tempRenderer != null && tempRenderer.gameObject != null)
             Destroy(tempRenderer.gameObject);
    }

    public void RemoveLink(LineRendererUi lineRendererUi) {
        for (var index = 0; index < Links.Count; index++) {
            var (lr, mmn) = Links[index];
            if (lr == lineRendererUi) 
                Links.RemoveAt(index);
            break;
        }
    }
}