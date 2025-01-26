using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : Singleton<TooltipManager> {
    private MindMapNode selectedNode;
    [SerializeField] private RectTransform tooltipObject;
    [SerializeField] private Text tooltipText;
    [SerializeField] private MonoBehaviour scalerObject;

    public void SelectNode(MindMapNode node) {
        selectedNode = node;
        tooltipObject.gameObject.SetActive(true);
        tooltipText.text = node.Information.Text;
        var left = Input.mousePosition.x < Screen.width / 2f;
        var up = Input.mousePosition.y < Screen.height / 2f;

        var scale = CanvasManager.Instance.GetScalingFactor();
        var newPos = node.RectTransform.position;
        newPos.x += (left ? node.RectTransform.rect.width*2/scale: -node.RectTransform.rect.width*2/scale);
        newPos.y += (up ? node.RectTransform.rect.height/scale : -node.RectTransform.rect.height/scale);

        tooltipObject.position = newPos;
    }

    public void UnselectNode(MindMapNode node) {
        if (selectedNode == node)
            selectedNode = null;
        tooltipObject.gameObject.SetActive(false);
    }
}
