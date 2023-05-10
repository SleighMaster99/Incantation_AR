using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBasicGhost : MonoBehaviour
{
    public Slider hpSlider;

    PlayerInformation pi;

    Vector3 targetPosition;

    float moveSpeed;
    public float hp;
    float maxHP;

    // Start is called before the first frame update
    void Start()
    {
        pi = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformation>();
        targetPosition = Camera.main.transform.position + new Vector3(0, -7.0f, 0);

        moveSpeed = 10.0f;
        maxHP = 100.0f;
        hpSlider.maxValue = maxHP;
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(targetPosition);

        hpSlider.value = hp;

        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Á×¾úÀ» ¶§
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            pi.playerBasicInfo.playerMentality -= 10.0f;

            if (!pi.audioPlayer.isPlaying)
                pi.audioPlayer.PlayOneShot(pi.hitSound);

            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Shield"))
            Destroy(this.gameObject);
    }
}
