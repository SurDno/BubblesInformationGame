using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactCreation : MonoBehaviour
{
    //[SerializeField] private ContactObject

    //not sure if this should be like a manger
    [SerializeField] private List<ContactObject> mContactObject;
    [SerializeField] private GameObject mContactFolder = null;

    [SerializeField] private GameObject gContactPrefab = null;
    [SerializeField] private GameObject gMessagePanelPrefab = null;
    [SerializeField] private UI_Manager gUIManager = null;
    [SerializeField] private Transform gMessageFolder = null;

    void Start()
    {
        if(!gUIManager)
            mContactFolder = GameObject.FindGameObjectWithTag("UI Manager");


        InitContacts();
    }

    private void InitContacts()
    {
        if (!mContactFolder)
            mContactFolder = GameObject.FindGameObjectWithTag("ContentFolder");

        if(!gMessageFolder)
            gMessageFolder = GameObject.FindGameObjectWithTag("MessageFolder").transform;

        foreach (ContactObject contactObject in mContactObject)
        {
            GameObject gameObj = Instantiate(gContactPrefab, mContactFolder.transform);
            GameObject message_content = Instantiate(gMessagePanelPrefab, gMessageFolder);

            gameObj.name = "Contact - " + contactObject.ContactName;  
            message_content.name = contactObject.ContactName + " " +
                "-- Message Content";

            if(message_content.TryGetComponent<CreateTextMessages>(out CreateTextMessages message))
            {
                message.Contact = contactObject;
            }

            //try find child with contact name text  
            gameObj.GetComponentInChildren<Text>().text = contactObject.ContactName; //<-- not ideal (terrible)
            
            if(gameObj.TryGetComponent<Button>(out Button button))
            {
                Debug.Log("Created a Button");
                button.onClick.AddListener(delegate { gUIManager.OpenMenu(message_content); });
            }
        }

    }
}
