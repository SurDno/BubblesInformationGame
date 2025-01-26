using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInfoDraggable : DraggableMindMapElement
{
    public NoteObject NoteInfo { get; set; }
    protected override Information GetInformation() => NoteInfo.GetInformation();
}
