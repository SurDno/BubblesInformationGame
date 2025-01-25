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
        tooltipObject.position = node.RectTransform.position;
    }

    public void UnselectNode(MindMapNode node) {
        if (selectedNode == node)
            selectedNode = null;
        tooltipObject.gameObject.SetActive(false);
    }
}
