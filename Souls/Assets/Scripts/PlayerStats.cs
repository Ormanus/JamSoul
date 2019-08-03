using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    public Image healthBar;
    public Image soulBar;
    public Image weaponImage;
    public Image potionImage;
    public Sprite[] weaponSprites;
    public Sprite[] potionSprites;

    [HideInInspector]
    public int maxHealth;
    [HideInInspector]
    public bool strengthPotion;
    [HideInInspector]
    public bool toughnessPotion;
    [HideInInspector]
    public bool timePotion;
    private int maxSouls;

    private int potionToUse;
    public int PotionToUse
    {
        get
        {
            return potionToUse;
        }
        set
        {
            potionToUse = value;
            if(potionToUse >= 0 && potionToUse < 5)
            {
                potionImage.enabled = true;
                potionImage.sprite = potionSprites[potionToUse];
            }
            else
            {
                potionImage.enabled = false;
            }
        }
    }
    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            healthBar.fillAmount = (float)health / maxHealth;
        }
    }
    
    public Sprite[] soulSprites;
    private int souls;
    public int Souls
    {
        get
        {
            return souls;
        }
        set
        {
            souls = value;
            if (souls > 0 && souls <= maxSouls)
            {
                soulBar.enabled = true;
                soulBar.sprite = soulSprites[souls - 1];
            }
            else
            {
                soulBar.enabled = false;
            }
        }
    }
    private int weapon;
    public int Weapon
    {
        get
        {
            return weapon;
        }
        set
        {
            weapon = value;
            weaponImage.sprite = weaponSprites[weapon];
        }
    }
    void Start()
    {
        maxHealth = 8;
        maxSouls = 9;
        Weapon = 0;
        Health = maxHealth;
        Souls = maxSouls;
        PotionToUse = -1;
    }

    public void DoDamage(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            if (Health > 0)
            {
                Health--;
            }
            else
            {
                Souls--;
            }
        }
    }
}
