using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject BackButton;
    [SerializeField] private GameObject rootMenu;
    [SerializeField] private GameObject currentMenu;


    [SerializeField] private Stack<GameObject> mMenuStack = new Stack<GameObject>();

    public void OpenMenu(GameObject menuToOpen)
    {
        currentMenu.SetActive(false);
        mMenuStack.Push(currentMenu);
        currentMenu = menuToOpen;
        currentMenu.SetActive(true);    

        if(menuToOpen == rootMenu) BackButton.SetActive(false);
        else BackButton.SetActive(true);
    }

    public void GoBack() 
    {
        currentMenu.SetActive(false);
        if(mMenuStack.Count > 0)
        {
            currentMenu = mMenuStack.Peek();
            mMenuStack.Pop();
        }
        currentMenu.SetActive(true);


        if (currentMenu == rootMenu) BackButton.SetActive(false);
        else BackButton.SetActive(true);
    }


}
