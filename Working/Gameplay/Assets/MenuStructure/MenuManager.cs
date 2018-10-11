using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN menu_manager
public class MenuManager : MonoBehaviour {

    [SerializeField] List<Menu> menus = new List<Menu>();

    private void Start()
    {
        // Show the first menu on start
        ShowMenu(menus[0]);
    }
    
    public void ShowMenu(Menu menuToShow) {
  
        // Ensure that this menu is one that we're tracking.
        if (menus.Contains(menuToShow) == false) {
            
            Debug.LogErrorFormat(
                "{0} is not in the list of menus", 
                menuToShow.name
            );
            return;
        }

        // Enable this menu, and disable the others
        foreach (var otherMenu in menus) {
            
            // Is this the menu we want to display?
            if (otherMenu == menuToShow) {

                // Mark it as active
                otherMenu.gameObject.SetActive(true);                

                // Tell the Menu object to invoke its "did appear" action
                otherMenu.menuDidAppear.Invoke();

            } else {

                // Is this menu currently active?
                if (otherMenu.gameObject.activeInHierarchy)
                {
                    // If so, tell the Menu object to invoke its "will disappear" action
                    otherMenu.menuWillDisappear.Invoke();
                }

                // And mark it as inactive
                otherMenu.gameObject.SetActive(false);    
            }
        }
    }

    // BEGIN menu_manager_extra_methods
    public void PlayGame() {
        Debug.Log("Starting the game!");
    }

    public void OptionChanged() {
        Debug.Log("Option changed!");
    }
    // END menu_manager_extra_methods
}

// END menu_manager
