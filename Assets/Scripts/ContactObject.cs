using UnityEngine;

[CreateAssetMenu(fileName = "Contact", menuName = "Contact")]
public class ContactObject : ScriptableObject
{
    [SerializeField] private string _contactName;
    [SerializeField] private TextMessageObject[] _contactHistory;

    public string ContactName => _contactName;
    public TextMessageObject[] ContactHistory => _contactHistory;
}
