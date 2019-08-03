using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    PlayerStats player;
    [HideInInspector]
    public bool attacking = false;
    float damageTimer = 0.2f;

    bool dealingDamage = false;

    void Start()
    {
        player = GameObject.Find("PlayerObject").GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }
        else if (attacking && dealingDamage)
        {
            player.DoDamage(2);
            damageTimer = 0.30f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            dealingDamage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            dealingDamage = false;
        }
    }
}
