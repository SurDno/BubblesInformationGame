using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Information", menuName = "Information")]
public class Information : ScriptableObject {
	[SerializeField] private string text;
	[SerializeField] private Sprite sprite;
	[SerializeField] private bool isTrue;
	[SerializeField] private List<Information> connectedInfo;
	
	public string Text => text;
	public Sprite Sprite => sprite;
	public bool IsTrue => isTrue;
}