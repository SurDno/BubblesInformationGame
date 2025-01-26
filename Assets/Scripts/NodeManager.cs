using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeManager : Singleton<NodeManager> {
	[SerializeField] private GameObject prefab;
	[SerializeField] private GameObject nodeParent;
	[SerializeField] private Information initialNode;
	
	private List<MindMapNode> _nodes = new();

	private void Awake() {
		AddNode(initialNode, new Vector2(500, 500));
	}
	
	public void AddNode(Information info, Vector2? position = null) {
		if (HasNode(info)) return;
		var node = Instantiate(prefab, nodeParent.transform, true).GetComponent<MindMapNode>();
		position ??= CanvasManager.Instance.GetMousePositionInRect(nodeParent.transform as RectTransform);
		node.transform.position = (Vector2)position;
		node.Initialize(info);
		_nodes.Add(node);
	}

	public bool HasNode(Information info) => _nodes.Any(node => node.Information.Equals(info));

}
