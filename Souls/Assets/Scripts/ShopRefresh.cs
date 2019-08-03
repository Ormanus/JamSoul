using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopRefresh : MonoBehaviour
{


    public int buttonSpacing;
    public RectTransform backGroundTransform;
    public GameObject buttonPrefab;
    public GameObject buttonLockedPrefab;
    public PlayerStats playerStats;

    private int shopIndex = 2;

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
            case 0: // Better weapon
                playerStats.Weapon = 1;
                break;
            case 1: // Even better weapon
                playerStats.Weapon = 2;
                break;
            case 2: // The best weapon
                playerStats.Weapon = 3;
                break;
            default:
                break;
        }
        LoadShop(shopIndex);
    }
    public static List<GameObject> currentButtons;
    public void LoadShop(int index)
    {
        // Destroy the previous shop
        if(currentButtons != null)
        {
            for (int i = 0; i < currentButtons.Count; i++)
            {
                Destroy(currentButtons[i]);
            }
            currentButtons.Clear();
        }
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

    UnityEvent unityEvent = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        currentButtons = new List<GameObject>();
        unityEvent.AddListener(() => LoadShop(shopIndex));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("r") && unityEvent != null)
        {
            unityEvent.Invoke();
        }
    }
}
