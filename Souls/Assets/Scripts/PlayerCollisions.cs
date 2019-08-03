using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public float damageTime;
    public Rigidbody2D rb2d;
    public PlayerStats playerStats;
    public float impact;
    private float timeSinceDamage = 0;

    // Update is called once per frame
    void Update()
    {
        timeSinceDamage += Time.deltaTime;
    }
    void CollisionFunction(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (timeSinceDamage > damageTime)
            {
                playerStats.DoDamage(1);
                timeSinceDamage = 0.0f;
                Vector3 thrustDirection3D = rb2d.transform.position - collision.gameObject.transform.position;
                Vector2 thrustDirection = new Vector2(thrustDirection3D.x, thrustDirection3D.y).normalized * impact;
                rb2d.AddForce(thrustDirection, ForceMode2D.Impulse);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionFunction(collision);
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        CollisionFunction(collision);
    }
}
