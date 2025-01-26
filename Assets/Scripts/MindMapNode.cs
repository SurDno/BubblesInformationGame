using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abstract;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MindMapNode : DraggableElement, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public Information Information { get; private set; }
    private bool _initialized;

    [Header("References")] 
    private Image _image;   
    [SerializeField] public LineRendererUi linkPrefab;
    [SerializeField] private Text _missingLinkNumber;
    
    public List<(LineRendererUi lr, MindMapNode mmn)> Links = new();
    private static MindMapNode linkingNode;
    private static LineRendererUi tempRenderer;
    public RectTransform RectTransform { get; private set; }

    private static readonly int InversionAmount = Shader.PropertyToID("_InversionAmount");
    private static readonly int PopupProgress = Shader.PropertyToID("_PopupProgress");
    private static readonly int ShapeMorph = Shader.PropertyToID("_ShapeMorph");
    private Material _material;
    private Coroutine _inversionCoroutine;
    [SerializeField] private float transitionDuration = 0.3f;

    private bool _animated;

    public float AudioVolume = 1f;

    public AudioClip AudioClipError;
    public AudioMixerGroup AudioGroupError;

    protected override void Awake() {
        base.Awake();
        _image = GetComponent<Image>();
        Assert.IsNotNull(_image);
        RectTransform = GetComponent<RectTransform>();
        _material = new Material(_image.material);
        _image.material = _material;
        _material.SetFloat(PopupProgress, 0f);
        scalerObject = transform.parent.GetComponent<MindMapContents>();
    }

    private void OnEnable() {
        StartCoroutine(AnimatePopup());
    }

    private IEnumerator AnimatePopup() {
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            _material.SetFloat(PopupProgress, progress);
            yield return null;
        }

        _material.SetFloat(PopupProgress, 1f);
        _animated = true;
        _missingLinkNumber.transform.parent.gameObject.SetActive(true);
    }
    
    public void UpdateNodeState() {
        var isLinked = Links.Count > 0;
        var targetInversion = isLinked ? 0f : 1f;

        if (_inversionCoroutine != null)
            StopCoroutine(_inversionCoroutine);
        _inversionCoroutine = StartCoroutine(TransitionInversionAmount(targetInversion));
        UpdateMissingLinkNumber();
    }
    
    private IEnumerator TransitionInversionAmount(float targetValue) {
        var startValue = _material.GetFloat(InversionAmount);
        var elapsedTime = 0f;

        while (elapsedTime < transitionDuration) {
            elapsedTime += Time.deltaTime;
            var t = elapsedTime / transitionDuration;
            var currentValue = Mathf.Lerp(startValue, targetValue, t);
            _material.SetFloat(InversionAmount, currentValue);
            yield return null;
        }

        _material.SetFloat(InversionAmount, targetValue);
        _inversionCoroutine = null;
    }

    public void Initialize(Information information) {
        if(_initialized) throw new Exception("MindMapNode already initialized");
        Information = information;
        _image.sprite = Information.Sprite;
        if(information.IsConclusion)
            _material.SetFloat(ShapeMorph, 1);
        UpdateMissingLinkNumber();
    } 
    
    protected override bool CanDrag(PointerEventData data) => true;
    
    public void OnPointerEnter(PointerEventData eventData) {
        if (dragging || linkingNode != null || !_animated) return;
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
                    //SFXManager.PlaySound(AudioClipError, AudioGroupError, AudioVolume);
                    return;
                }

                if (!Information.GetConnectedInfo().Contains(linkingNode.Information)) {
                    HintManager.Instance.ShowUnrelatedMessage();
                    DisableLinkingNode();
                    //SFXManager.PlaySound(AudioClipError, AudioGroupError, AudioVolume);
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
                NodeManager.Instance.UpdateConclusions();
                break;
            case PointerEventData.InputButton.Right:
                DisableLinkingNode();
                linkingNode = this;
                tempRenderer = Instantiate(linkPrefab, transform.parent).Initialize(RectTransform, null);
                tempRenderer.transform.SetAsFirstSibling();
                tempRenderer.GetComponent<Image>().raycastTarget = false;
                break;
        }
    }

    private void UpdateMissingLinkNumber() {
        var potentialConnections = Information.GetConnectedInfo().Count;
        var existingConnections = Links.Count(link => Information.GetConnectedInfo().Contains(link.mmn.Information));
        
        _missingLinkNumber.text = (potentialConnections - existingConnections).ToString();
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
        if (tempRenderer != null && tempRenderer.gameObject != null)
             Destroy(tempRenderer.gameObject);
    }
}