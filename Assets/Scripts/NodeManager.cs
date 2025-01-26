using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeManager : Singleton<NodeManager> {
	[SerializeField] private GameObject prefab;
	[SerializeField] private GameObject nodeParent;
	[SerializeField] private Information initialNode;
	
	private List<MindMapNode> _nodes = new();

	public List<Information> DELETE_ME;

	private void Awake() {
		AddNode(initialNode, new Vector2(500, 500));
	}
	
	public MindMapNode AddNode(Information info, Vector2? position = null) {
		if (HasNode(info)) return null;
		var node = Instantiate(prefab, nodeParent.transform, true).GetComponent<MindMapNode>();
		position ??= CanvasManager.Instance.GetMousePositionInRect(nodeParent.transform as RectTransform);
		node.transform.position = (Vector2)position;
		node.transform.localScale = Vector3.one;
		node.Initialize(info);
		_nodes.Add(node);
		return node;
	}

	public void RemoveNode(MindMapNode node) {
		Destroy(node);
		_nodes.Remove(node);
	}

	public bool HasNode(Information info) => _nodes.Any(node => node.Information.Equals(info));
	
	public MindMapNode GetNodeByInfo(Information info) => _nodes.FirstOrDefault(node => node.Information.Equals(info));

	public void UpdateConclusions() {
		foreach (var node in _nodes) {
			foreach (var conclusion in node.Information.GetConclusions()) {
				if (HasNode(conclusion)) continue;
				if (UpdateConclusion(conclusion)) return;
			}
		}
	}
	
	private bool UpdateConclusion(Information conclusion) {
		var connectedPrereqs = new HashSet<(Information, Information)>();
    
		var prereqNodes = conclusion.GetPrereqs().Where(HasNode).Select(GetNodeByInfo).ToList();
    
		if (prereqNodes.Count != conclusion.GetPrereqs().Count()) return false;

		foreach (var node1 in prereqNodes) {
			foreach (var node2 in prereqNodes) {
				if (node1 == node2) continue;
            
				if (node1.Links.Any(link => link.mmn == node2)) 
					connectedPrereqs.Add((node1.Information, node2.Information));
			}
		}

		var requiredConnections = conclusion.GetPrereqs()
			.SelectMany(info1 => conclusion.GetPrereqs()
				.Where(info2 => info1 != info2)
				.Select(info2 => (info1, info2)))
			.Where(pair => pair.info1.GetConnectedInfo().Contains(pair.info2))
			.ToHashSet();

		if (requiredConnections.Except(connectedPrereqs).Any()) return false;
    
		var avgPosition = prereqNodes.Aggregate(Vector2.zero, (sum, node) => sum + (Vector2)node.transform.position) / prereqNodes.Count;
		var conclusionNode = AddNode(conclusion, avgPosition + Vector2.up * 100);
    
		foreach (var prereqNode in prereqNodes) {
			var line = conclusionNode.linkPrefab;
            
			var lineRenderer = Instantiate(line, nodeParent.transform)
				.Initialize(conclusionNode.RectTransform, prereqNode.RectTransform, conclusion: true);
			lineRenderer.transform.SetAsFirstSibling();
        
			conclusionNode.Links.Add((lineRenderer, prereqNode));
			prereqNode.Links.Add((lineRenderer, conclusionNode));
        
			conclusionNode.UpdateNodeState();
			prereqNode.UpdateNodeState();
		}
    
		return true;
	}
}
