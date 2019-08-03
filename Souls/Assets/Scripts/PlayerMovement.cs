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
        if (velocitySpeed > maxSpeed)
        {
            rb2d.velocity = rb2d.velocity.normalized * maxSpeed;
        }
        // Define new angle
        float movementAngle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x);
        Quaternion newQuaternion = Quaternion.Euler(0.0f, 0.0f, movementAngle/Mathf.PI*180.0f + 90);
        // Rotate player
        //transform.position = newPosition;
        if(inputVector.magnitude > 0.0f)
        {
            transform.rotation = newQuaternion;
        }
    }
}
