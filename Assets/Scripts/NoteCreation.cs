using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteCreation : MonoBehaviour
{
    [SerializeField] private NoteObjectList _noteObjects;
    [SerializeField] private GameObject _notePrefab;
    [SerializeField] private Transform _noteContentFolder;

    private void Start()
    {
        CreateNotes();
    }

    private void CreateNotes()
    {
        //for(int i = 0; i < _noteObjects.GetNoteObjects().Count; i++)
        //{
        //    var noteObject = _noteObjects.GetNoteObjects()[i];
        //    var note = Instantiate(_notePrefab, _noteContentFolder);
        //    note.GetComponent<NoteInfoDraggable>().NoteInfo = _noteObjects.GetNoteObjects()[i];
        //    // set the title
        //}
        foreach (var noteObject in _noteObjects.GetNoteObjects())
        {
            var note = Instantiate(_notePrefab, _noteContentFolder);
            note.GetComponent<NoteInfoDraggable>().NoteInfo = noteObject;
            var child1 = note.transform.GetChild(0);
            var child2 = child1.transform.GetChild(1);
            var child3 = child1.transform.GetChild(2);
            child2.GetComponent<Text>().text = noteObject.GetTitle();
            child3.GetComponent<Text>().text = noteObject.GetContent();
        }
    }
}