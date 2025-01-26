using Interfaces;
using UnityEngine;

[System.Serializable]
public struct TextMessageObject 
{
    [SerializeField] private string _text;
    [SerializeField] private bool _isVictim;
    [SerializeField] private Information _textInformation;

    public string Text => _text;
    public bool IsVictim => _isVictim;

    public Information GetInformation() => _textInformation;
}
