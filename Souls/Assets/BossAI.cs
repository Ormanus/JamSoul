using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    PlayerStats player;
    float attackTime = 0f;
    bool attacking = false;

    void Start()
    {
        player = GameObject.Find("PlayerObject").GetComponent<PlayerStats>();
    }

    private void Update()
    {
        attacking = false;
        attackTime += Time.deltaTime;
        if(attackTime > 1f)
        {
            attackTime -= 1f;
            attacking = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(attacking)
                player.DoDamage(2);
        }
    }
}
