using UnityEngine;
using UnityEngine.EventSystems;

public class TextMessage : DraggableMindMapElement {
    public TextMessageObject TextMessageInfo {  get; set; }

    protected override Information GetInformation() => TextMessageInfo.GetInformation();
}
