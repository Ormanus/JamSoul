using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public int hp = 5;
    public BossWeapon weapon;
    public Transform player;
    public Animator animator;
    public float rushCooldown = 2.0f;

    Phase phase = Phase.Idle;
    Vector2 rushTarget;

    enum Phase
    {
        Idle,
        Swing,
        Rush
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

        switch (phase)
        {
            case Phase.Idle when (player.position - transform.position).sqrMagnitude < 32:
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
