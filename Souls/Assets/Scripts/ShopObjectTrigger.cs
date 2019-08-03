using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopObjectTrigger : MonoBehaviour
{
    public Vector2 worldOrigo;
    public float shop2Distance;
    public float shop3Distance;
    private GameObject shopCanvas;
    private ShopRefresh shopRefresh;
    private int shopType = 0;
    //private GameObject enterShopCanvas;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initialazing shop");
        shopCanvas = GameObject.Find("ShopUI").transform.GetChild(0).gameObject;
        shopCanvas.SetActive(false);
        shopRefresh = shopCanvas.GetComponent<ShopRefresh>();
        Vector3 position3D = gameObject.transform.position;
        float distance = (worldOrigo - new Vector2(position3D.x, position3D.y)).magnitude;
        if (distance > shop3Distance)
        {
            shopType = 2;
        }
        else if (distance > shop2Distance)
        {
            shopType = 1;
        }
        //enterShopCanvas = GameObject.Find("EnterShopCanvas");
    }

    void inTrigger()
    {
        //if (Input.GetKeyDown("enter"))
        //{
            //enterShopCanvas.SetActive(false);
            shopCanvas.SetActive(true);
        //}
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            //enterShopCanvas.SetActive(true);
            inTrigger();
            shopRefresh.LoadShop(shopType);
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            inTrigger();
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //enterShopCanvas.SetActive(false);
            shopCanvas.SetActive(false);
        }
    }
}
