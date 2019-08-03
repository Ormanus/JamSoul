﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public Rigidbody2D rb2d;
    public float acceleration;
    public float maxSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 thrustDirection3D = player.transform.position - rb2d.transform.position;
        Vector2 thrustDirection = new Vector2(thrustDirection3D.x, thrustDirection3D.y).normalized;
        rb2d.AddForce(thrustDirection * acceleration);
        if(rb2d.velocity.magnitude > maxSpeed)
        {
            rb2d.velocity = rb2d.velocity.normalized * maxSpeed;
        }
    }
}