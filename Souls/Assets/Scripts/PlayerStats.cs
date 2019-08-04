using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
    private GameObject deathUI;
    private Image deathBackground;
    private float timeSinceDeath = 0.0f;
    [HideInInspector]
    public bool playerDead = false;
    private bool playerWon = false;
    private GameObject winUI;
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

    internal void Win()
    {
        if (!playerWon)
        {
            playerWon = true;
            winUI.SetActive(true);
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
        deathUI = GameObject.Find("DeathUI");
        deathBackground = deathUI.GetComponentInChildren<Image>();
        deathUI.SetActive(false);
        winUI = GameObject.Find("WinUI");
        winUI.SetActive(false);
    }

    public void Die()
    {
        if (!playerDead)
        {
            deathUI.SetActive(true);
            timeSinceDeath = 0.0f;
            playerDead = true;
        }
    }

    void Update()
    {
        if (playerDead)
        {
            timeSinceDeath += Time.deltaTime;
            deathBackground.color = new Vector4(0.0f, 0.0f, 0.0f, timeSinceDeath);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("GameScene");
            }
        }
        if (playerWon)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("GameScene");
            }
        }
    }

    public void DoDamage(int damage)
    {
        if (toughnessPotion && damage >= 1)
        {
            damage--;
        }
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
        if(Souls <= 0)
        {
            Die();
        }
    }
}
