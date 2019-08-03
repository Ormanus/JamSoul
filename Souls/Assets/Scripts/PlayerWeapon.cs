using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeapon : MonoBehaviour
{
    public BoxCollider2D weaponHitBox;
    public float maxHitRate;
    private float timeSinceHit = 0.0f;
    private float weaponSize;
    private bool hitting = false;
    public bool isHitting()
    {
        return hitting;
    }
    public float getTimeSinceHit()
    {
        return timeSinceHit;
    }
    public float getMaxHitRate()
    {
        return maxHitRate;
    }
    public float WeaponSize
    {
        get
        {
            return weaponSize;
        }
        set
        {
            weaponSize = value;
            weaponHitBox.offset = new Vector2(weaponSize, 0.0f);
            weaponHitBox.size = new Vector2(weaponSize, 0.5f);
        }
    }
    void doHit()
    {
        Debug.Log("Hit Dealt");
        weaponHitBox.enabled = true;
        timeSinceHit = 0.0f;
        hitting = true;
    }

    UnityEvent hitEvent = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        WeaponSize = 0.5f;
        hitEvent.AddListener(doHit);
        weaponHitBox.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceHit += Time.deltaTime;
        if(timeSinceHit > maxHitRate / 4.0f)
        {
            hitting = false;
            weaponHitBox.enabled = false;
        }
        if (Input.GetKey("space") && hitEvent != null && timeSinceHit > maxHitRate)
        {
            hitEvent.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Enemy")
        {
            Destroy(collider.gameObject);
        }
    }
}
