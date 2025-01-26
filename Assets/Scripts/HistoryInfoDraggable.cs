using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryInfoDraggable : DraggableMindMapElement
{
    public HistoryObject historyObject { get; set; }
    protected override Information GetInformation() => historyObject.GetInformation();
}
