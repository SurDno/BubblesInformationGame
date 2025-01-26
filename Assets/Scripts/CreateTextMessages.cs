using UnityEngine;
using UnityEngine.UI;

public class CreateTextMessages : MonoBehaviour
{
    [SerializeField] private ContactObject mContact;
    [SerializeField] private Transform textMessageParent;
    [SerializeField] private Transform _otherPersonMessagePrefab;
    [SerializeField] private Transform _victimMessagePrefab;
    // Start is called before the first frame update
    private void Start()
    {
        //CreateMessages();
    }

    public ContactObject Contact
    {
        get { return mContact; }
        set 
        { 
            mContact = value;
            CreateMessages();
        }
    }

    private void CreateMessages(){
        // loop through all messages and create the message objects
        for(int i = 0; i < mContact.ContactHistory.Length; i++)
        {
            var _message = mContact.ContactHistory[i];
            TextMessage _textMessage;
            
            var prefab = _message.IsVictim ? _victimMessagePrefab : _otherPersonMessagePrefab;
            _textMessage = Instantiate(prefab, textMessageParent).GetComponent<TextMessage>();
            if (_textMessage.GetComponentInChildren<Text>() == null) Debug.Log("Fuck");
            _textMessage.GetComponentInChildren<Text>().text = _message.Text;
            _textMessage.TextMessageInfo = _message;
            _textMessage.GetComponentInChildren<MesageTextBox>().Init();
        }
    }
}
