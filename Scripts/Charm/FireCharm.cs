using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireCharm : MonoBehaviour
{
    public GameObject fireCharmRayPoint;        // Liner GameObject
    public GameObject[] fireBallSpawner;        // fireBall 생성 위치 GameObject
    GameObject[] fireBalls;                     // 생성된 fireBall Prefabs

    public Transform fireBall;                  // fireBall Prefab

    public Text warningNewCharmText;            // 새로운 부적 경고문 텍스트
    public GameObject warningNewCharm;

    RaycastHit hit;

    public Vector3 targetPosition;              // Ray에 맞은 타겟의 포지션

    public PlayerInformation pi;

    AudioSource audioPlayer;
    public AudioClip fireSound;
    public AudioClip shootSound;

    bool isTargetFound;                         // 이미지 타겟을 찾았을 때
    bool isHit;                                 // 적이 목표물에 맞았을 때

    float fireBallSpeed;                        // 파이어볼이 날아가는 속도
    float measuredTime;

    // Start is called before the first frame update
    void Start()
    {
        isTargetFound = false;
        isHit = false;

        fireBallSpeed = 25.0f;
        measuredTime = 0;

        audioPlayer = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // 부적이 트래킹 됐을 때
        if (isTargetFound)
        {
            if (!audioPlayer.isPlaying)
                audioPlayer.PlayOneShot(fireSound);

            RayTracingFunction();
        }

        // Ray를 맞춰 목표가 지정됐을 때
        if (isHit)
        {
            Fire();
        }

        if (GameManager.currentLocation == GameManager.CurrentLocation.normalGround)
        {
            warningNewCharmText.gameObject.SetActive(false);
        }
    }

    // Ray를 쐈을 때
    // Tag가 Player, BasicGhost 이면 isHit 활성화
    void RayTracingFunction()
    {
        Ray ray = new Ray(fireCharmRayPoint.transform.position, -fireCharmRayPoint.transform.up + fireCharmRayPoint.transform.forward);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.CompareTag("BasicGhost") ||
                hit.transform.gameObject.CompareTag("MiddleGhost") ||
                hit.transform.gameObject.CompareTag("BossGhost"))
            {
                targetPosition = hit.point;

                isHit = true;
            }
        }
    }

    // fireBall 발사
    // RayTracingFunction에서 hit한 물체의 position에 이동
    void Fire()
    {
        foreach (GameObject fbs in fireBalls)
        {
            if (fbs != null)
            {
                fbs.transform.SetParent(null);
                fbs.transform.position = Vector3.MoveTowards(fbs.transform.position, targetPosition, fireBallSpeed * Time.deltaTime);
            }
        }

        
        // 새로운 부적 경고문 텍스트
        if (!GameObject.FindGameObjectWithTag("FireBall"))
        {
            //warningNewCharmText.gameObject.SetActive(false);
            warningNewCharmText.text = "부적을 인식하시오";
        }
        else
        {
            warningNewCharmText.text = "새로운 부적을 인식하지 마시오";
            warningNewCharmText.gameObject.SetActive(true);
        }
    }

    // FireCharm 이미지 타겟을 찾았을 때
    public void TargetFoundAction()
    {
        isTargetFound = true;
        isHit = false;

        warningNewCharmText.gameObject.SetActive(false);

        if (pi.playerBasicInfo.playerCharmLevel.Equals("하급"))
            Instantiate(fireBall, fireBallSpawner[0].transform);
        else if (pi.playerBasicInfo.playerCharmLevel.Equals("중급"))
        {
            for (int i = 0; i < 3; i++)
            {
                Instantiate(fireBall, fireBallSpawner[i].transform);
            }
        }
        else
        {
            foreach (GameObject spawnPoint in fireBallSpawner)
            {
                Instantiate(fireBall, spawnPoint.transform);
            }
        }

        fireBalls = GameObject.FindGameObjectsWithTag("FireBall");
    }

    // FireCharm 이미지 타겟을 잃어버렸을 때
    public void TargetLostAction()
    {
        isTargetFound = false;
        warningNewCharmText.text = "부적을 인식하시오";
        audioPlayer.Stop();
    }

}
