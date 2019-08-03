using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopRefresh : MonoBehaviour
{


    public int buttonSpacing;
    public RectTransform backGroundTransform;
    public GameObject buttonPrefab;
    public GameObject buttonLockedPrefab;
    private GameObject player;
    private PlayerStats playerStats;
    private PlayerWeapon playerWeapon;

    void Start()
    {
        player = GameObject.Find("PlayerObject");
        playerStats = player.GetComponent<PlayerStats>();
        playerWeapon = player.GetComponent<PlayerWeapon>();
    }

    private int shopIndex;

    [System.Serializable]
    public struct Shop
    {
        public Item[] items;
    }
    public Shop[] shops;

    public void OnClick(Item item, int shopIndex)
    {
        if(playerStats.Souls <= item.price)
        {
            // Tell player they can't sell their whole soul
        }
        else
        {
            playerStats.Souls -= item.price;
        }
        switch (item.type)
        {
            case 0: // Spear
                playerStats.Weapon = 1;
                playerWeapon.WeaponSize = 0.5f;
                break;
            case 1: // Also spear
                playerStats.Weapon = 1;
                playerWeapon.WeaponSize = 0.5f;
                break;
            case 2: // Scythe
                playerStats.Weapon = 2;
                playerWeapon.WeaponSize = 0.7f;
                break;
            case 3: // Healing
                playerStats.Health = playerStats.maxHealth;
                break;
            case 4: // Potions
            case 5:
            case 6:
            case 7:
            case 8:
                playerStats.PotionToUse = item.type - 4;
                break;
            default:
                break;
        }
        LoadShop(shopIndex);
    }
    private List<GameObject> currentButtons;
    public void LoadShop(int index)
    {
        // Destroy the previous shop
        if(currentButtons != null)
        {
            for (int i = 0; i < currentButtons.Count; i++)
            {
                Destroy(currentButtons[i]);
            }
        }
        currentButtons = new List<GameObject>();
        Shop shop = shops[index];
        int numItems = shop.items.Length;
        int itemsListed = 0;
        for (int i = 0; i < numItems; i++)
        {
            Item item = shop.items[i];

            bool locked = playerStats.Souls > item.price;

            Debug.Log(string.Format("Loading Item {0}...", i));
            GameObject itemButton;
            if (locked)
            {
                itemButton = GameObject.Instantiate<GameObject>(buttonPrefab);
            }
            else
            {
                itemButton = GameObject.Instantiate<GameObject>(buttonLockedPrefab);
            }
            currentButtons.Add(itemButton);

            itemButton.transform.SetParent(backGroundTransform, false);
            RectTransform buttonTransform = itemButton.GetComponent<RectTransform>();
            Image buttonImage = itemButton.GetComponent<Image>();
            Text buttonText = itemButton.GetComponentInChildren<Text>();

            buttonTransform.anchoredPosition = new Vector2(0.0f, -itemsListed * buttonSpacing);
            buttonImage.sprite = item.sprite;
            buttonText.text = string.Format("{0}\nPrice: {1} piece{2} of soul",item.description, item.price, item.price == 1 ? "" : "s");
            if (playerStats.Souls > item.price)
            {
                Button button = itemButton.GetComponent<Button>();
                button.onClick.AddListener(() => { OnClick(item, index); });
            }
            itemsListed++;
        }
    }
}
