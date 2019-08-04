using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossAI : MonoBehaviour
{
    const int maxHealth = 5;
    public int hp = maxHealth;
    public BossWeapon weapon;
    public Transform player;
    public Animator animator;
    public float rushCooldown = 2.0f;
    public Image healthBar;
    float invTime = 0.0f;

    Phase phase = Phase.Idle;
    Vector2 rushTarget;

    enum Phase
    {
        Idle,
        Swing,
        Rush,
        Dead,
    }

    public void takeDamage(int amount)
    {
        if(invTime < 0f)
        {
            invTime = 0.3f;
            hp -= amount;
            healthBar.fillAmount = (float)hp / maxHealth;
            if (hp <= 0)
            {
                phase = Phase.Dead;
                animator.enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                healthBar.transform.parent.parent.gameObject.SetActive(false);
                player.GetComponent<PlayerStats>().Win();
            }
        }
    }

    private void Start()
    {
        player = GameObject.Find("PlayerObject").transform;
    }

    void Update()
    {
        Vector2 delta = player.position - transform.position;
        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg + 90f);
        rushCooldown -= Time.deltaTime;
        invTime -= Time.deltaTime;

        switch (phase)
        {
            case Phase.Idle when (player.position - transform.position).sqrMagnitude < 32:
                healthBar.transform.parent.parent.gameObject.SetActive(true);
                startRush();
                break;
            case Phase.Rush:
                {
                    Vector3 rushDirection = (Vector3)rushTarget - transform.position;
                    if (rushDirection.magnitude < Time.deltaTime * 10f)
                    {
                        swingTheScythe();
                    }
                    else
                    {
                        transform.position += rushDirection.normalized * Time.deltaTime * 10f;
                    }
                    break;
                }

            case Phase.Swing:
                {
                    if(delta.magnitude > 5f && rushCooldown < 0f)
                    {
                        startRush();
                    }
                }
                break;
            case Phase.Dead:
                weapon.attacking = false;
                break;
        }
    }

    void startRush()
    {
        Vector2 delta = player.position - transform.position;
        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(delta.y, delta.x));
        animator.SetTrigger("Rush");
        rushTarget = player.position;
        phase = Phase.Rush;
        weapon.attacking = true;
        rushCooldown = 2.0f;
    }

    void swingTheScythe()
    {
        weapon.attacking = false;
        animator.SetTrigger("Attack");
        phase = Phase.Swing;
        StartCoroutine(swingTimer());
    }

    IEnumerator swingTimer()
    {
        while(phase == Phase.Swing)
        {
            yield return new WaitForSeconds(0.4f);
            weapon.attacking = true;
            yield return new WaitForSeconds(0.1f);
            weapon.attacking = false;
        }
    }
}
