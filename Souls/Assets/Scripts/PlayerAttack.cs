using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator attack;
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("press");
            attack.SetBool("Attacking", true);
        } else
        {
            attack.SetBool("Attacking", false);
        }
    }
}
