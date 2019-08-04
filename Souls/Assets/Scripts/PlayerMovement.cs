using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public Rigidbody2D rb2d;
    public SpriteRenderer spriteRenderer;
    public Sprite standingSprite;
    public Sprite[] walkingSprites;
    public Sprite[] weapon0Sprites;
    public Sprite[] weapon1Sprites;
    public Sprite[] weapon2Sprites;
    public PlayerWeapon playerWeapon;
    public PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Read the input and make a vector out of them
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");
        Vector2 inputVector = new Vector2(inputHorizontal, inputVertical);
        rb2d.AddForce(inputVector.normalized * acceleration);
        float velocitySpeed = rb2d.velocity.magnitude;
        // Apply brakes
        //rb2d.AddForce(-rb2d.velocity.normalized * acceleration * 0.5f);
        // Don't let the player go too fast
        if (velocitySpeed > maxSpeed * (playerStats.timePotion ? 2f : 1f))
        {
            rb2d.velocity = rb2d.velocity.normalized * maxSpeed * (playerStats.timePotion ? 2f : 1f);
        }
        // Define new angle
        float movementAngle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x);
        Quaternion newQuaternion = Quaternion.Euler(0.0f, 0.0f, movementAngle/Mathf.PI*180.0f + 90.0f);
        // Rotate player
        //transform.position = newPosition;
        if(inputVector.magnitude > 0.0f)
        {
            transform.rotation = newQuaternion;
        }

        // Change the sprites
        if (playerWeapon.isHitting())
        {
            if(playerStats.Weapon == 0)
            {
                spriteRenderer.sprite = weapon0Sprites[(int)(playerWeapon.getTimeSinceHit() * 11.0f)];
            }
            if (playerStats.Weapon == 1)
            {
                spriteRenderer.sprite = weapon1Sprites[(int)(playerWeapon.getTimeSinceHit() * 11.0f)];
            }
            if (playerStats.Weapon == 2)
            {
                spriteRenderer.sprite = weapon2Sprites[(int)(playerWeapon.getTimeSinceHit() * 11.0f)];
            }
        }
        else
        {
            if (inputVector.magnitude > 0.0f)
            {
                if (Mathf.Repeat(Time.time, 0.5f) < 0.25f)
                {
                    spriteRenderer.sprite = walkingSprites[0];
                }
                else
                {
                    spriteRenderer.sprite = walkingSprites[1];
                }
            }
            else
            {
                spriteRenderer.sprite = standingSprite;
            }
        }
    }
}
