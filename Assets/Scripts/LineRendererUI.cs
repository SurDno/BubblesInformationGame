using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineRendererUi : MonoBehaviour, IPointerClickHandler {
    private MindMapContents _scalerObject;
    private RectTransform _transform;
    private Image _image;
    [SerializeField] private float linkWidth = 3f;
    [SerializeField] private RectTransform node1, node2;
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

    public LineRendererUi Initialize(RectTransform node1, RectTransform node2) {
        this.node1 = node1;
        this.node2 = node2;
        return this;
    }

    public void OnPointerClick(PointerEventData eventData) { 
        if (eventData.button != PointerEventData.InputButton.Right) return;
        node1.GetComponent<MindMapNode>().RemoveLink(this);
        node2.GetComponent<MindMapNode>().RemoveLink(this);
        Destroy(this.gameObject);
    }
}