using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectSelection : MonoBehaviour
{
    public Button inventoryButton;
    public Button actionButton;

    private void Start()
    {
        // Set the inventory button as pre-selected
        //    EventSystem.current.SetSelectedGameObject(inventoryButton.gameObject);
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(inventoryButton.gameObject);   
    }

    public void OnInventoryButtonClick()
    {
        // Handle inventory button click
    }

    public void OnActionButtonClick()
    {
        // Handle action button click
    }
}
