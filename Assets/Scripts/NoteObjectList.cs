using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Notes", menuName = "Note List")]
public class NoteObjectList : ScriptableObject
{
    [SerializeField] private List<NoteObject> noteObjects = new List<NoteObject>();

    public List<NoteObject> GetNoteObjects() => noteObjects;
}