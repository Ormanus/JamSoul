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
    private PlayerStats playerStats;
    private GameObject player;
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
            weaponHitBox.offset = new Vector2(0.0f, -weaponSize);
            weaponHitBox.size = new Vector2(0.3f, weaponSize);
        }
    }
    void doHit()
    {
        weaponHitBox.enabled = true;
        timeSinceHit = 0.0f;
        hitting = true;
    }

    UnityEvent hitEvent = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        WeaponSize = 0.3f;
        hitEvent.AddListener(doHit);
        weaponHitBox.enabled = false;
        player = GameObject.Find("PlayerObject");
        playerStats = player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        // Do a hit if conditions allow
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
        // Use potion
        if (Input.GetKey("q"))
        {
            switch (playerStats.PotionToUse)
            {
                case 0: // Healing potion
                    playerStats.Health = playerStats.maxHealth;
                    break;
                case 1: // Vision potion
                    GameObject cameraObject = GameObject.Find("Main Camera");
                    Camera camera = cameraObject.GetComponent<Camera>();
                    camera.orthographicSize += 2;
                    StartCoroutine(ResetVision());
                    break;
                case 2: // Time potion
                    playerStats.timePotion = true;
                    StartCoroutine(ResetTime());
                    break;
                case 3: // Toughness potion
                    playerStats.toughnessPotion = true;
                    StartCoroutine(ResetToughness());
                    break;
                case 4: // Strength potion
                    playerStats.strengthPotion = true;
                    StartCoroutine(ResetStrength());
                    break;
                default:
                    break;
            }
            playerStats.PotionToUse = -1;
        }
    }

    IEnumerator ResetVision()
    {
        yield return new WaitForSeconds(10f);
        GameObject cameraObject = GameObject.Find("Main Camera");
        Camera camera = cameraObject.GetComponent<Camera>();
        camera.orthographicSize -= 2;
    }

    IEnumerator ResetTime()
    {
        yield return new WaitForSeconds(10f);
        playerStats.timePotion = false;
    }
    IEnumerator ResetToughness()
    {
        yield return new WaitForSeconds(10f);
        playerStats.toughnessPotion = false;
    }
    IEnumerator ResetStrength()
    {
        yield return new WaitForSeconds(10f);
        playerStats.strengthPotion = false;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            BossAI boss = collider.gameObject.GetComponent<BossAI>();
            if (boss)
            {
                boss.takeDamage(1 + (playerStats.strengthPotion ? 1 : 0));
            }
            else
            {
                Destroy(collider.gameObject);
            }
        }
    }
}
