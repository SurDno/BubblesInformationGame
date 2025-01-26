using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HistoryObject
{
    [SerializeField] private string _searchHistory;
    [SerializeField] private Information _historyInfo;

    public string GetSearchHistory()
    {
        return _searchHistory;
    }

    public Information GetInformation() => _historyInfo;

}
