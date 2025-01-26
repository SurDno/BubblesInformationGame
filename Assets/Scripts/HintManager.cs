using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : Singleton<HintManager> {
	[SerializeField] private Text hintText;

	private string _baseText;

	private bool _userConnected, _userDisconnected;
    private void Start() {
        hintText.text = _baseText = "Find useful clues on the victim's phone and drag them onto your mind map.";
    }

    public void AfterFirstNode() {
	    hintText.text = _baseText = "Keep getting more facts from the phone to paint the whole picture.";
    }

    public void WhenConnectionPossible() {
	    hintText.text = _baseText = "Connect related facts to draw conclusions: right-click one, left-click another.";
    }
    
    public void AfterConnection() {
	    hintText.text = _baseText = "Check number near each node - if it is not 0, you miss a connection.";
    }

    public void ShowErrorMessage() {
	    StopAllCoroutines();
	    StartCoroutine(ErrorMessage("There's no useful info here..."));
    }

    public void ShowDuplicateMessage() {
	    StopAllCoroutines();
	    StartCoroutine(ErrorMessage("I am already aware of that."));
    }

    public void ShowUnrelatedMessage() {
	    StopAllCoroutines();
	    StartCoroutine(ErrorMessage("I don't think this is related."));
    }

    private IEnumerator ErrorMessage(string msg) {
	    hintText.text = msg;
	    yield return new WaitForSecondsRealtime(2f);
	    hintText.text = _baseText;
    }
}
