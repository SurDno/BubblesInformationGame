using UnityEngine;

[System.Serializable]
public struct TextMessageObject
{
    [SerializeField] string _text;
    [SerializeField] bool _isVictim;
    [SerializeField] Information _textInformation;

    public string Text { get { return _text; } }
    public bool IsVictim { get { return _isVictim; } }

    public Information TextInformation { get { return _textInformation; } }
}
