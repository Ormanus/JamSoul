using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    public float fadeTime;
    private float timeSinceStart;
    // Start is called before the first frame update
    void Start()
    {
        timeSinceStart = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceStart += Time.deltaTime;
        gameObject.GetComponent<SpriteRenderer>().color = new Vector4(1.0f, 1.0f, 1.0f, fadeTime - timeSinceStart);
    }
}
