using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTextMessages : MonoBehaviour
{
    [SerializeField] private ContactObject contact;
    [SerializeField] private Transform textMessageParent;
    [SerializeField] private Transform _otherPersonMessagePrefab;
    [SerializeField] private Transform _victimMessagePrefab;
    // Start is called before the first frame update
    void Start()
    {
        CreateMessages();
    }

    private void CreateMessages(){
        // loop through all messages and create the message objects
        for(int i = 0; i < contact.ContactHistory.Length; i++)
        {
            var _message = contact.ContactHistory[i];
            TextMessage _textMessage;
            if (_message.IsVictim)
            {
                _textMessage = Instantiate(_victimMessagePrefab, textMessageParent).GetComponent<TextMessage>();
            }
            else
            {
                _textMessage = Instantiate(_otherPersonMessagePrefab, textMessageParent).GetComponent<TextMessage>();
            }
            if (_textMessage.GetComponentInChildren<Text>() == null) Debug.Log("Fuck");
            _textMessage.GetComponentInChildren<Text>().text = _message.Text;
            //_textMessage.TextMessageInfo = _message;
        }
    }
}
