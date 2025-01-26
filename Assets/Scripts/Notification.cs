using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour {
    [SerializeField] private List<Information> tabInfo;
    private float _updateFrequency = 0.5f;
    [SerializeField] private Text _text;

    private void Start() {
	    if (NodeManager.Instance != null) UpdateNumber();
    }
    private void OnEnable() {
	    if (NodeManager.Instance != null) UpdateNumber();
    }
    
    private void UpdateNumber() {
	    var count = tabInfo.Count(i => !NodeManager.Instance.HasNode(i));
	    _text.text = count.ToString();
	    gameObject.SetActive(count != 0);
    }
}
