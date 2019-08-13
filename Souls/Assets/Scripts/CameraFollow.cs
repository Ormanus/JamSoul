using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public GameObject cameraObject;

    void FixedUpdate()
    {
        cameraObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10.0f);
    }
}
