using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossGhost : MonoBehaviour
{
    public GameObject spawnBasicGhostPoint1;
    public GameObject spawnBasicGhostPoint2;
    public GameObject spawnRayPoint;
    public GameObject bossBasicGhost;
    public GameObject fire;
    PlayerInformation pi;

    public Slider hpSlider;

    Vector3 targetPosition;

    float measureTime;
    public float hp;
    float maxHP;

    bool isFire;

    // Start is called before the first frame update
    void Start()
    {
        pi = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformation>();

        targetPosition = Camera.main.transform.position + new Vector3(0, -15.0f, 0);
        maxHP = Random.Range(2000.0f, 4000.0f);
        hp = maxHP;

        hpSlider.maxValue = maxHP;
        hpSlider.value = hp;

        measureTime = 0;

        isFire = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(targetPosition);

        measureTime += Time.deltaTime;
        hpSlider.value = hp;

        if (measureTime >= 20.0f)
        {
            if (RandomBool())
            {
                Instantiate(bossBasicGhost, spawnBasicGhostPoint1.transform.position, spawnBasicGhostPoint1.transform.rotation);
                Instantiate(bossBasicGhost, spawnBasicGhostPoint2.transform.position, spawnBasicGhostPoint2.transform.rotation);
            }
            else
                isFire = true;

            measureTime = 0;
        }

        if (isFire)
            FireWallAttack();

        // Á×¾úÀ» ¶§
        if (hp <= 0)
        {
            if (pi.playerQuest.ghost == "¸¶¿Õ")
            {
                pi.playerQuest.exorcismCnt++;
            }
            pi.playerBasicInfo.magicPower += 1;

            Destroy(this.gameObject);
        }
    }

    // ·£´ý true false
    bool RandomBool()
    {
        return (Random.value > 0.5f);
    }

    // ºÒ±âµÕ ½î±â °ø°Ý
    void FireWallAttack()
    {
        fire.SetActive(true);
        Invoke("ShootRay", 2.0f);
    }

    // ºÒ±âµÕ Ray
    void ShootRay()
    {
        Ray ray = new Ray(spawnRayPoint.transform.position, spawnRayPoint.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 80.0f))
        {
            if (hit.transform.CompareTag("Shield") && isFire)
            {
                Shield shield = GameObject.Find("ShieldCollider").GetComponent<Shield>();
                shield.PlayShieldSound();
                fire.SetActive(false);
                isFire = false;
            }
            
            if (hit.transform.CompareTag("MainCamera") && isFire)
            {
                pi.playerBasicInfo.playerMentality -= 50.0f;
                if (!pi.audioPlayer.isPlaying)
                    pi.audioPlayer.PlayOneShot(pi.hitSound);
                fire.SetActive(false);
                isFire = false;
            }
        }

        Debug.DrawRay(spawnRayPoint.transform.position, spawnRayPoint.transform.forward * 80.0f, Color.red);
    }
}
