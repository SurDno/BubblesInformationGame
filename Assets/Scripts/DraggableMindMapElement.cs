using System.Collections;
using System.Collections.Generic;
using Abstract;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DraggableMindMapElement : DraggableElement {
	private Transform _originalParent;
	private Vector2 _originalPosition;
	private int _originalSiblingIndex;
	private GameObject _placeholder;

	[SerializeField] private AudioClip _mindMapSuccess, _mindMapFailure;
	[SerializeField] private AudioSource src;

	protected override void Awake() {
		src = FindObjectOfType<AudioSource>();
		base.Awake();
		_originalParent = transform.parent;
		_originalPosition = _rect.anchoredPosition;
		_originalSiblingIndex = transform.GetSiblingIndex();
	}

	public override void OnBeginDrag(PointerEventData data) {
		if (!CanDrag(data)) return;
       
		_originalPosition = _rect.anchoredPosition;
		_originalParent = transform.parent;
		_originalSiblingIndex = transform.GetSiblingIndex();
		_placeholder = new GameObject("Placeholder");
		var placeholderRect = _placeholder.AddComponent<RectTransform>();
		placeholderRect.SetParent(_originalParent);
		placeholderRect.sizeDelta = _rect.sizeDelta;
		placeholderRect.anchoredPosition = _rect.anchoredPosition;
		placeholderRect.SetSiblingIndex(_originalSiblingIndex);
		transform.SetParent(CanvasManager.Instance.GetCanvasTransform());
		base.OnBeginDrag(data);
	}

	public override void OnEndDrag(PointerEventData eventData) {
		base.OnEndDrag(eventData);
		transform.SetParent(_originalParent);
		transform.SetSiblingIndex(_originalSiblingIndex);
		_rect.anchoredPosition = _originalPosition;
		Destroy(_placeholder);

		if (GetInformation() == null) {
			src.PlayOneShot(_mindMapFailure);
			HintManager.Instance.ShowErrorMessage();
			return;
		}

		if (NodeManager.Instance.HasNode(GetInformation())) {
			HintManager.Instance.ShowDuplicateMessage();
			src.PlayOneShot(_mindMapFailure);
			return;
		}

		NodeManager.Instance.AddNode(GetInformation());
		src.PlayOneShot(_mindMapSuccess);
	}

	protected abstract Information GetInformation();
	
	// Always allow to drag stuff even when no info.
	protected override bool CanDrag(PointerEventData data) => true;
}