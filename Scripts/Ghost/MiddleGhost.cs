using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiddleGhost : MonoBehaviour
{
    PlayerInformation pi;

    public Animator middleGhostAnimator;

    public Slider hpSlider;

    Vector3 targetPosition;

    float measureTime;
    public float hp;

    // Start is called before the first frame update
    void Start()
    {
        pi = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformation>();

        targetPosition = Camera.main.transform.position + new Vector3(0, -7.0f, 0);
        
        measureTime = 0;
        hpSlider.maxValue = 200.0f;
        hp = 200.0f;
    }

    // Update is called once per frame
    void Update()
    {
        measureTime += Time.deltaTime;
        hpSlider.value = hp;

        this.transform.LookAt(targetPosition);

        if (measureTime > 20.0f)
        {
            middleGhostAnimator.SetTrigger("isAttack");

            if (pi.shield.activeSelf)
                pi.shield.GetComponent<Shield>().PlayShieldSound();
            else
            {
                pi.playerBasicInfo.playerMentality -= 20.0f;

                if (!pi.audioPlayer.isPlaying)
                    pi.audioPlayer.PlayOneShot(pi.hitSound);
            }

            measureTime = 0;
        }

        // 죽었을 때
        if (hp <= 0)
        {
            if (pi.playerQuest.ghost == "두억시니")
            {
                pi.playerQuest.exorcismCnt++;
            }
            pi.playerBasicInfo.magicPower += 1;

            Destroy(this.gameObject);
        }
    }
}
