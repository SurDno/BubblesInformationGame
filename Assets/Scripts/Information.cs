using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Information", menuName = "Information")]
public class Information : ScriptableObject {
	[SerializeField] private string text;
	[SerializeField] private Sprite sprite;
	[SerializeField] private bool isTrue;
	[SerializeField] private bool isConclusion;
	[SerializeField] private List<Information> connectedInfo, conclusions, prereqs;
	
	public string Text => text;
	public Sprite Sprite => sprite;
	public bool IsTrue => isTrue;
	public bool IsConclusion => isConclusion;
	public List<Information> GetConnectedInfo() => connectedInfo;
	public List<Information> GetConclusions() => conclusions;
	public List<Information> GetPrereqs() => prereqs;
}