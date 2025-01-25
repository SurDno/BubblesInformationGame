using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : Singleton<TooltipManager> {
    private MindMapNode selectedNode;
    [SerializeField] private RectTransform tooltipObject;
    [SerializeField] private Text tooltipText;

    public void SelectNode(MindMapNode node) {
        selectedNode = node;
        tooltipObject.gameObject.SetActive(true);
        tooltipText.text = node.Information.Text;
        var left = Input.mousePosition.x < Screen.width / 2f;
        var up = Input.mousePosition.y < Screen.height / 2f;

        var newPos = node.RectTransform.position;
        newPos.x += (left ? node.RectTransform.rect.width*3 : -node.RectTransform.rect.width*3);
        newPos.y += (up ? node.RectTransform.rect.height : -node.RectTransform.rect.height);

        tooltipObject.position = newPos;
    }

    public void UnselectNode(MindMapNode node) {
        if (selectedNode == node)
            selectedNode = null;
        tooltipObject.gameObject.SetActive(false);
    }
}