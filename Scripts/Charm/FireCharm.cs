using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireCharm : MonoBehaviour
{
    public GameObject fireCharmRayPoint;        // Liner GameObject
    public GameObject[] fireBallSpawner;        // fireBall ���� ��ġ GameObject
    GameObject[] fireBalls;                     // ������ fireBall Prefabs

    public Transform fireBall;                  // fireBall Prefab

    public Text warningNewCharmText;            // ���ο� ���� ��� �ؽ�Ʈ
    public GameObject warningNewCharm;

    RaycastHit hit;

    public Vector3 targetPosition;              // Ray�� ���� Ÿ���� ������

    public PlayerInformation pi;

    AudioSource audioPlayer;
    public AudioClip fireSound;
    public AudioClip shootSound;

    bool isTargetFound;                         // �̹��� Ÿ���� ã���� ��
    bool isHit;                                 // ���� ��ǥ���� �¾��� ��

    float fireBallSpeed;                        // ���̾�� ���ư��� �ӵ�
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
        // ������ Ʈ��ŷ ���� ��
        if (isTargetFound)
        {
            if (!audioPlayer.isPlaying)
                audioPlayer.PlayOneShot(fireSound);

            RayTracingFunction();
        }

        // Ray�� ���� ��ǥ�� �������� ��
        if (isHit)
        {
            Fire();
        }

        if (GameManager.currentLocation == GameManager.CurrentLocation.normalGround)
        {
            warningNewCharmText.gameObject.SetActive(false);
        }
    }

    // Ray�� ���� ��
    // Tag�� Player, BasicGhost �̸� isHit Ȱ��ȭ
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

    // fireBall �߻�
    // RayTracingFunction���� hit�� ��ü�� position�� �̵�
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

        
        // ���ο� ���� ��� �ؽ�Ʈ
        if (!GameObject.FindGameObjectWithTag("FireBall"))
        {
            //warningNewCharmText.gameObject.SetActive(false);
            warningNewCharmText.text = "������ �ν��Ͻÿ�";
        }
        else
        {
            warningNewCharmText.text = "���ο� ������ �ν����� ���ÿ�";
            warningNewCharmText.gameObject.SetActive(true);
        }
    }

    // FireCharm �̹��� Ÿ���� ã���� ��
    public void TargetFoundAction()
    {
        isTargetFound = true;
        isHit = false;

        warningNewCharmText.gameObject.SetActive(false);

        if (pi.playerBasicInfo.playerCharmLevel.Equals("�ϱ�"))
            Instantiate(fireBall, fireBallSpawner[0].transform);
        else if (pi.playerBasicInfo.playerCharmLevel.Equals("�߱�"))
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

    // FireCharm �̹��� Ÿ���� �Ҿ������ ��
    public void TargetLostAction()
    {
        isTargetFound = false;
        warningNewCharmText.text = "������ �ν��Ͻÿ�";
        audioPlayer.Stop();
    }

}
