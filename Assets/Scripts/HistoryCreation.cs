using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryCreation : MonoBehaviour
{
    [SerializeField] private HistoryObjectList historyObjectList;
    [SerializeField] private GameObject historyObjectPrefab;
    [SerializeField] private Transform historyObjectParent;

    private void Start()
    {
        foreach (var historyObject in historyObjectList.GetHistoryObjects())
        {
            var newHistoryObject = Instantiate(historyObjectPrefab, historyObjectParent);
            newHistoryObject.GetComponent<HistoryInfoDraggable>().historyObject = historyObject;
            newHistoryObject.GetComponentInChildren<Text>().text = historyObject.GetSearchHistory();
        }
    }
}
