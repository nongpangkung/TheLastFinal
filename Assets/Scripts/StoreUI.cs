using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using FN;

public class StoreUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public Image itemIconImage;

    public Transform container;
    public GameObject shopItemButtonPrefab;

    PlayerManager player;

    [Header("Weapon Stats")]
    public TextMeshProUGUI priceText;
    GoldCountBar goldCountBar;

    public List<Item> Items;

    private void Update()
    {
        UpdateGoldCount();
    }

    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>();
        container = transform.Find("container");
        goldCountBar = FindObjectOfType<GoldCountBar>();
    }

    private void Start()
    {
        ShopItemUI(Items);
    }

    GameObject firstButton;
    private void OnEnable()
    {
        if(firstButton !=null)
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
    }

    private void ShopItemUI(List<Item> items)
    {
        float spacing = 5f;

        RectTransform templateRectTransform = shopItemButtonPrefab.GetComponent<RectTransform>();
        Vector2 originalPosition = templateRectTransform.anchoredPosition;

        bool isFirstButton = true;
        GameObject firstButtonObject = null;

        foreach (Item item in items)
        {
            GameObject shopItemButton = Instantiate(shopItemButtonPrefab, container);
            Button button = shopItemButton.GetComponent<Button>();

            RectTransform shopItemRectTransform = shopItemButton.GetComponent<RectTransform>();
            shopItemRectTransform.anchoredPosition = originalPosition;

            TextMeshProUGUI itemNameTextInstance = shopItemButton.transform.Find("nameText").GetComponent<TextMeshProUGUI>();
            Image itemIconImageInstance = shopItemButton.transform.Find("itemImage").GetComponent<Image>();
            TextMeshProUGUI priceTextInstance = shopItemButton.transform.Find("costText").GetComponent<TextMeshProUGUI>();

            if (item.itemName != null)
            {
                itemNameTextInstance.text = item.itemName;
            }
            else
            {
                itemNameTextInstance.text = "";
            }

            if (item.itemIcon != null)
            {
                itemIconImageInstance.gameObject.SetActive(true);
                itemIconImageInstance.enabled = true;
                itemIconImageInstance.sprite = item.itemIcon;
            }
            else
            {
                itemIconImageInstance.gameObject.SetActive(false);
                itemIconImageInstance.enabled = false;
                itemIconImageInstance.sprite = null;
            }

            priceTextInstance.text = item.price.ToString();

            button.onClick.AddListener(() => Buy(item));

            // Set the first button as pre-selected
            if (isFirstButton)
            {
                firstButtonObject = shopItemButton;
                firstButton = shopItemButton;
                isFirstButton = false;
            }

            originalPosition.y -= (shopItemRectTransform.sizeDelta.y + spacing);
        }

        Destroy(shopItemButtonPrefab);

        // Delay the selection to ensure the layout is properly updated
        StartCoroutine(SelectFirstButtonCoroutine(firstButtonObject));
    }

    private System.Collections.IEnumerator SelectFirstButtonCoroutine(GameObject buttonObject)
    {
        yield return null;

        EventSystem.current.SetSelectedGameObject(null);
        yield return null;

        EventSystem.current.SetSelectedGameObject(buttonObject);
    }

    public void Buy(Item item)
    {
        switch (item.itemType)
        {
            case itemType.health:
                if (item is FlaskItem)
                {
                    BuyFlaskItem(player, (FlaskItem)item);
                }
                break;
            case itemType.weapon:
                if (item is WeaponItem)
                {
                    BuyWeaponItem(player, (WeaponItem)item);
                }
                break;
        }
    }

    private void BuyFlaskItem(PlayerManager player, FlaskItem health)
    {
        if (player.playerStatsManager.currentGoldCount >= health.price)
        {
            int amountToAdd = 1;
            if (health.currentItemAmount + amountToAdd <= health.maxItemAmount)
            {
                Debug.Log("You have purchased " + health.itemName + " for " + health.price + " coins!");
                health.currentItemAmount += amountToAdd;
                player.playerStatsManager.currentGoldCount -= health.price;
            }
            else
            {
                Debug.Log("HealthItem inventory is full.");
            }
        }
        else
        {
            Debug.Log("Your gold is not enough..");
        }
    }

    private void BuyWeaponItem(PlayerManager player, WeaponItem weapon)
    {
        if (player.playerStatsManager.currentGoldCount >= weapon.price)
        {
            Debug.Log("You have purchased " + weapon.itemName + " for " + weapon.price + " coins!");
            player.playerInventoryManager.weaponsInventory.Add(weapon);
            player.playerStatsManager.currentGoldCount -= weapon.price;
        }
        else
        {
            Debug.Log("Your gold is not enough..");
        }
    }
    private void UpdateGoldCount()
    {
        int currentGoldCount = player.playerStatsManager.currentGoldCount;
        goldCountBar.SetGoldCountText(currentGoldCount, 0); // Pass 0 as the default value for increasedGold
    }

}
