using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HistoryObjectList", menuName = "History Object List")]
public class HistoryObjectList : ScriptableObject
{
    [SerializeField] private HistoryObject[] historyObjects;

    public HistoryObject[] GetHistoryObjects() => historyObjects;
}
