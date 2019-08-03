using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    Vector2 movement;
    public Animator move;
    // Start is called before the first frame update
    void Start()
    {
        movement.x = 0;
        movement.y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        move.SetBool("Moving", true);
        // Read the input and make a vector out of them
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");
        Vector2 inputVector = new Vector2(inputHorizontal, inputVertical);

        // make player's controls affect the movement
        movement += movement.normalized * (-acceleration) * 0.5f;
        // fixing the bug causing the player to continue moving if the speed is below the acceleration
        if (inputVector.magnitude == 0.0f && movement.magnitude < acceleration * 0.5)
        {
            movement = Vector2.zero;
            move.SetBool("Moving", false);
        }
        movement += inputVector * acceleration;
        float movementSpeed = movement.magnitude;
        Debug.Log(movementSpeed);

        // Don't go past the maximum speed
        if (movementSpeed > maxSpeed)
        {
            movement = movement.normalized * maxSpeed;
        }
        movementSpeed = movement.magnitude;


        // Define new position
        Vector3 newPosition = transform.position + new Vector3(movement.x, movement.y, 0.0f) * Time.deltaTime;
        // Define new angle
        float movementAngle = Mathf.Atan2(movement.y,movement.x);
        Quaternion newQuaternion = Quaternion.Euler(0.0f, 0.0f, movementAngle/Mathf.PI*180.0f + 90);
        // Move and rotate player
        transform.position = newPosition;
        if(inputVector.magnitude > 0.0f)
        {
            transform.rotation = newQuaternion;
        }
    }
}
