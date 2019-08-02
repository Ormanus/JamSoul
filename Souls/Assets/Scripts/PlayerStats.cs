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
    public Sprite[] weaponSprites;

    private int maxHealth;
    private int maxSouls;

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
            soulBar.fillAmount = (float)souls / maxSouls;
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
        maxSouls = 4;
        Weapon = 0;
        Health = maxHealth;
        Souls = maxSouls;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
