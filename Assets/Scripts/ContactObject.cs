using UnityEngine;

[CreateAssetMenu(fileName = "Contact", menuName = "Contact")]
public class ContactObject : ScriptableObject
{
    [SerializeField] private string _contactName;
    [SerializeField] private TextMessageObject[] _contactHistory;

    public string ContactName { get { return _contactName; } }
    public TextMessageObject[] ContactHistory { get {  return _contactHistory; } }
}
