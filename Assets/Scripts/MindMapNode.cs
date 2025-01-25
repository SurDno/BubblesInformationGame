using System;
using System.Collections;
using System.Collections.Generic;
using Abstract;
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

    private static readonly int InversionAmount = Shader.PropertyToID("_InversionAmount");
    private Material _material;
    private Coroutine _inversionCoroutine;
    [SerializeField] private float transitionDuration = 0.3f;
    
    protected override void Awake() {
        base.Awake();
        _image = GetComponent<Image>();
        Assert.IsNotNull(_image);
        RectTransform = GetComponent<RectTransform>();
        Initialize(DELETEME);
        _material = new Material(_image.material);
        _image.material = _material;
    }
    
    public void UpdateNodeState() {
        bool isLinked = Links.Count > 0;
        float targetInversion = isLinked ? 0f : 1f;

        if (_inversionCoroutine != null)
            StopCoroutine(_inversionCoroutine);
        _inversionCoroutine = StartCoroutine(TransitionInversionAmount(targetInversion));
    }
    
    private IEnumerator TransitionInversionAmount(float targetValue) {
        float startValue = _material.GetFloat(InversionAmount);
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            float currentValue = Mathf.Lerp(startValue, targetValue, t);
            _material.SetFloat(InversionAmount, currentValue);
            yield return null;
        }

        _material.SetFloat(InversionAmount, targetValue);
        _inversionCoroutine = null;
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
                if (linkingNode == this) return;
                
                foreach (var (_, mmn) in Links) {
                    if (mmn != linkingNode) continue;
                    DisableLinkingNode();
                    return;
                }
                var lineRendererUi = Instantiate(linkPrefab, transform.parent)
                    .Initialize(linkingNode.RectTransform, RectTransform);
                lineRendererUi.transform.SetAsFirstSibling();
                Links.Add((lineRendererUi, linkingNode));
                linkingNode.Links.Add((lineRendererUi, this));
                UpdateNodeState();
                linkingNode.UpdateNodeState();
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

    public void RemoveLink(LineRendererUi lineRendererUi) {
        for (var index = 0; index < Links.Count; index++) {
            var (lr, mmn) = Links[index];
            if (lr == lineRendererUi) {
                Links.RemoveAt(index);
                UpdateNodeState();
                mmn.UpdateNodeState();
                break;
            }
        }
    }

    private void OnDestroy() {
        if (_material != null)
            Destroy(_material);
    }

    public static void DisableLinkingNode() {
        linkingNode = null;
        if(tempRenderer != null && tempRenderer.gameObject != null)
             Destroy(tempRenderer.gameObject);
    }
}