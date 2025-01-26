using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineRendererUi : MonoBehaviour, IPointerClickHandler {
    private MindMapContents _scalerObject;
    private RectTransform _transform;
    private Image _image;
    [SerializeField] private float linkWidth = 3f;
    [SerializeField] private RectTransform node1, node2;
    private bool _isConclusionLink;
    
    public void Start() {
        _transform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _scalerObject = MindMapContents.Instance;
    }

    private void Update() {
        var point1 = new Vector2(node1.position.x, node1.position.y);
        if (node2) {
            var point2 = new Vector2(node2.position.x, node2.position.y);
            UpdateLink(point1, point2);
        } else {
            UpdateLink(point1, CanvasManager.Instance.GetMousePositionInRect(_transform));
        }
    }
    
    private void UpdateLink(Vector2 point1, Vector2 point2) { 
        var midpoint = (point1 + point2) / 2f;
        _transform.position = midpoint;
        
        var customScale = _scalerObject.Scale;
        var canvasScale = CanvasManager.Instance.GetScalingFactor();
        
        var dir = point1 - point2;
        _transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        _transform.sizeDelta = new Vector2(Vector2.Distance(point1, point2) / customScale * canvasScale, linkWidth);
    }

    public LineRendererUi Initialize(RectTransform node1, RectTransform node2, bool conclusion = false) {
        this.node1 = node1;
        this.node2 = node2;
        
        var image = GetComponent<Image>();
        var material = new Material(image.material);
        image.material = material;
        var color = conclusion ? Color.white : Color.gray;
        material.SetColor("_Color", color);
        _isConclusionLink = conclusion;

        return this;
    }

    private void RecursivelyRemoveConclusions(Information conclusionInfo, HashSet<Information> processedConclusions) {
        if (processedConclusions.Contains(conclusionInfo)) return;
        processedConclusions.Add(conclusionInfo);
        
        var conclusionNode = NodeManager.Instance.GetNodeByInfo(conclusionInfo);
        if (conclusionNode == null) return;

        var dependentConclusions = new HashSet<Information>();
        foreach (var (_, connectedNode) in conclusionNode.Links) {
            foreach (var furtherConclusion in connectedNode.Information.GetConclusions()) {
                if (furtherConclusion.GetPrereqs().Contains(conclusionInfo)) {
                    dependentConclusions.Add(furtherConclusion);
                }
            }
        }

        foreach (var dependent in dependentConclusions) {
            RecursivelyRemoveConclusions(dependent, processedConclusions);
        }

        var linksToDestroy = conclusionNode.Links.Select(l => l.lr).ToList();
        foreach (var lr in linksToDestroy) {
            if (lr != null && lr.gameObject != null) {
                Destroy(lr.gameObject);
            }
        }
        NodeManager.Instance.RemoveNode(conclusionNode);
        Destroy(conclusionNode.gameObject);
    }

    public void OnPointerClick(PointerEventData eventData) { 
        if (eventData.button != PointerEventData.InputButton.Right || _isConclusionLink) return;
        
        var fromNode = node1.GetComponent<MindMapNode>();
        var toNode = node2.GetComponent<MindMapNode>();
        var affectedConclusions = new HashSet<Information>();
        var processedConclusions = new HashSet<Information>();

        foreach (var conclusion in fromNode.Information.GetConclusions()) {
            if (conclusion.GetPrereqs().Contains(fromNode.Information) && 
                conclusion.GetPrereqs().Contains(toNode.Information)) {
                affectedConclusions.Add(conclusion);
            }
        }
        
        foreach (var conclusion in affectedConclusions) {
            RecursivelyRemoveConclusions(conclusion, processedConclusions);
        }

        fromNode.RemoveLink(this);
        toNode.RemoveLink(this);
        Destroy(gameObject);
    }
}