using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBird : MonoBehaviour
{
    public AudioClip birdSounds;
    AudioSource audioPlayer;
    bool isAttack;

    PlayerInformation pi;
    float damage;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        pi = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformation>();
        audioPlayer = this.GetComponent<AudioSource>();
        isAttack = false;

        switch (pi.playerBasicInfo.playerBrushLevel)
        {
            case "하급":
                damage = 100.0f;
                break;
            case "중급":
                damage = 200.0f;
                break;
            case "상급":
                damage = 300.0f;
                break;
        }

        this.transform.SetParent(Camera.main.transform);
    }

    // Update is called once per frame
    void Update()
    {
        FindTargetRay();
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.red);
    }

    // Ray
    void FindTargetRay()
    {
        Ray ray = new Ray(this.transform.position, this.transform.forward);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.CompareTag("BasicGhost") ||
                hit.transform.gameObject.CompareTag("MiddleGhost") ||
                hit.transform.gameObject.CompareTag("BossGhost"))
            {
                if (!isAttack)
                {
                    audioPlayer.PlayOneShot(birdSounds);
                    isAttack = true;
                }
                this.transform.SetParent(null);
                this.transform.position = Vector3.MoveTowards(this.transform.position, hit.point, 25.0f * Time.deltaTime);
            }
        }
    }

    // 충돌
    // BasicGhost 이면 50 데미지 줌
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BasicGhost"))
        {
            other.GetComponent<BasicGhost>().hp -= damage;
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("MiddleGhost"))
        {
            other.GetComponent<MiddleGhost>().hp -= damage;
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("BossGhost"))
        {
            other.GetComponent<BossGhost>().hp -= damage;
            Destroy(this.gameObject);
        }
    }
}
