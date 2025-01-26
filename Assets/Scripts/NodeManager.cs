using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeManager : Singleton<NodeManager> {
	[SerializeField] private GameObject prefab;
	[SerializeField] private GameObject nodeParent;
	
	private List<MindMapNode> _nodes = new();

	public void AddNode(Information info) {
		if (HasNode(info)) return;
		var node = Instantiate(prefab, nodeParent.transform, true).GetComponent<MindMapNode>();
		node.transform.position = CanvasManager.Instance.GetMousePositionInRect(nodeParent.transform as RectTransform);
		node.Initialize(info);
		_nodes.Add(node);
	}

	public bool HasNode(Information info) => _nodes.Any(node => node.Information.Equals(info));

}
