using Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class LineRendererUi : MonoBehaviour {
    [SerializeField] private MonoBehaviour scalerObject;
    private RectTransform m_myTransform;
    private Image m_image;
    [SerializeField] private float linkWidth = 3f;
    [SerializeField] private RectTransform node1, node2;
    public void Start() {
        m_myTransform = GetComponent<RectTransform>();
        m_image = GetComponent<Image>();
    }

    private void Update() {
        var point1 = new Vector2(node2.position.x, node2.position.y);
        var point2 = new Vector2(node1.position.x, node1.position.y);
        var midpoint = (point1 + point2) / 2f;

        m_myTransform.position = midpoint;

        var customScale = scalerObject is IScaler scaler ? scaler.Scale : 1f;
        var canvasScale = CanvasManager.Instance.GetScalingFactor();
        
        var dir = point1 - point2;
        m_myTransform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        m_myTransform.sizeDelta = new Vector2(Vector2.Distance(node1.position, node2.position) / customScale * canvasScale, linkWidth);
    }

    public void Initialize(RectTransform node1, RectTransform node2) {
        this.node1 = node1;
        this.node2 = node2;
    }
}