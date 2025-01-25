using UnityEngine;

[CreateAssetMenu(fileName = "New Information", menuName = "Information")]
public class Information : ScriptableObject {
	[SerializeField] private string text;
	[SerializeField] private Sprite sprite;
	[SerializeField] private bool isTrue;
	
	public string Text => text;
	public Sprite Sprite => sprite;
	public bool IsTrue => isTrue;
}