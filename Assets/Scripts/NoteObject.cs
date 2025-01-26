using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NoteObject {
    [SerializeField] private string _title;
    [SerializeField] private string _content;
    [SerializeField] private Information _information;

    public Information GetInformation() => _information;
    public string GetTitle() => _title;
    public string GetContent() => _content;
}