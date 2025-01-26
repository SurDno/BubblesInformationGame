using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject BackButton;
    [SerializeField] private GameObject rootMenu;
    [SerializeField] private GameObject currentMenu;
    private GameObject previousMenu;

    public void OpenMenu(GameObject menuToOpen)
    {
        Debug.Log("OpenMenu");
        currentMenu.SetActive(false);
        previousMenu = currentMenu;
        currentMenu = menuToOpen;
        currentMenu.SetActive(true);

        if(menuToOpen == rootMenu) BackButton.SetActive(false);
        else BackButton.SetActive(true);
    }

    public void GoBack() { 
        currentMenu.SetActive(false);
        var newPreviousMenu = currentMenu;
        currentMenu = previousMenu;
        currentMenu.SetActive(true);
        previousMenu = newPreviousMenu;

        if (currentMenu == rootMenu) BackButton.SetActive(false);
        else BackButton.SetActive(true);
    }
}
