using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public GameObject[] rayPoint;
    public GameObject brushScroll;

    public PlayerInformation pi;

    /// <summary>
    /// ���� ����Ʈ
    /// </summary>
    public GameObject spawnGatePoint;
    public GameObject exitGateButton;
    float measuredTime;

    // ����Ʈ ����Ʈ
    public GameObject questGate;
    public GameObject fogParticle;
    bool isQuestGateSpawn;

    // ��Ʋ ����Ʈ
    public GameObject battleGateParticle;
    public GameObject battleGate;
    public GameObject returnGate;
    bool isBattleGateSpawn;
    bool isReturnGateSpawn;

    // ���� ����Ʈ
    public GameObject shopGate;
    bool isShopGateSpawn;

    // ���� �� ��ȯ
    public GameObject attackBird;
    public GameObject attackBirdSpawnPoint;

    // �ǵ�
    public GameObject bubbleParticle;
    public GameObject shieldCollider;
    public GameObject shieldImage;
    bool isShield;

    Ray[] ray;
    RaycastHit[] hit;

    bool[] isHit;

    // Start is called before the first frame update
    void Start()
    {
        ray = new Ray[9];
        hit = new RaycastHit[9];
        isHit = new bool[9];

        measuredTime = 0;

        isQuestGateSpawn = false;
        isBattleGateSpawn = false;
        isReturnGateSpawn = false;
        isShopGateSpawn = false;

        for (int i = 0; i < 9; i++)
        {
            ray[i] = new Ray();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pi.isdrawSpawnTargeting)
        {
            if(isHit[0] && isHit[1] && isHit[2])
            {
                // ����Ʈ ����Ʈ
                isQuestGateSpawn = true;
            }
            else if (isHit[3] && isHit[4] && isHit[5])
            {
                // ��Ʋ ����Ʈ
                if (GameManager.currentLocation == GameManager.CurrentLocation.normalGround)
                    isBattleGateSpawn = true;
                else if (GameManager.currentLocation != GameManager.CurrentLocation.normalGround)
                    isReturnGateSpawn = true;
            }
            else if (isHit[6] && isHit[7] && isHit[8])
            {
                // ���� ����Ʈ
                isShopGateSpawn = true;
            }
            else if (isHit[0] && isHit[3] && isHit[6])
            {
                // ����
                Instantiate(attackBird, attackBirdSpawnPoint.transform.position, attackBirdSpawnPoint.transform.rotation);
            }
            else if (isHit[2] && isHit[5] && isHit[8])
            {
                // ����
                isShield = true;
            }

            for (int i = 0; i < 9; i++)
            {
                isHit[i] = false;
            }
            measuredTime = 0;
            pi.isdrawSpawnTargeting = false;
            GameObject[] paints = GameObject.FindGameObjectsWithTag("Paint");
            foreach (GameObject p in paints)
            {
                Destroy(p);
            }
            brushScroll.SetActive(false);
        }

        if (isQuestGateSpawn)
            QuestGateManager();

        if (isBattleGateSpawn)
            BattleGateManager();
        
        if (isReturnGateSpawn)
            ReturnGateManager();

        if (isShopGateSpawn)
            ShopGateManager();

        if (isShield)
            ShieldManager();
    }

    // Ray���
    public void ScrollRay()
    {
        for (int i = 0; i < 9; i++)
        {
            ray[i] = new Ray(rayPoint[i].transform.position, rayPoint[i].transform.forward);

            if (Physics.Raycast(ray[i], out hit[i], Mathf.Infinity))
            {
                if(hit[i].transform.CompareTag("Paint"))
                    isHit[i] = true;
            }
        }

        for (int i = 0; i < 9; i++)
        {
            Debug.Log(isHit[i]);
        }
    }

    // ����Ʈ ����Ʈ ȿ��
    void QuestGateManager()
    {
        fogParticle.SetActive(true);
        questGate.transform.position = spawnGatePoint.transform.position;
        questGate.SetActive(true);
        exitGateButton.SetActive(true);

        measuredTime += Time.deltaTime;

        if (measuredTime >= 3.0f)
        {
            isQuestGateSpawn = false;
            fogParticle.SetActive(false);
            measuredTime = 0;
        }
    }

    // ��Ʋ ����Ʈ ȿ��
    void BattleGateManager()
    {
        battleGateParticle.SetActive(true);
        exitGateButton.SetActive(true);

        measuredTime += Time.deltaTime;

        if (measuredTime >= 1.5f)
        {
            battleGate.transform.position = spawnGatePoint.transform.position;
            battleGate.SetActive(true);
            isBattleGateSpawn = false;
            battleGateParticle.SetActive(false);
            measuredTime = 0;
        }
    }

    // ���� ����Ʈ ȿ��
    void ReturnGateManager()
    {
        battleGateParticle.SetActive(true);
        exitGateButton.SetActive(true);

        measuredTime += Time.deltaTime;

        if (measuredTime >= 1.5f)
        {
            returnGate.transform.position = spawnGatePoint.transform.position;
            returnGate.SetActive(true);
            battleGateParticle.SetActive(false);
            measuredTime = 0;
            isReturnGateSpawn = false;
        }
    }

    // ���� ����Ʈ ȿ��
    void ShopGateManager()
    {
        fogParticle.SetActive(true);
        shopGate.transform.position = spawnGatePoint.transform.position;
        shopGate.SetActive(true);
        exitGateButton.SetActive(true);

        measuredTime += Time.deltaTime;

        if (measuredTime >= 3.0f)
        {
            fogParticle.SetActive(false);
            measuredTime = 0;
            isShopGateSpawn = false;
        }
    }

    // �ǵ�
    void ShieldManager()
    {
        shieldImage.SetActive(true);
        shieldCollider.SetActive(true);
        bubbleParticle.SetActive(true);

        measuredTime += Time.deltaTime;

        if (measuredTime >= 3.0f)
        {
            shieldImage.SetActive(false);
            shieldCollider.SetActive(false);
            bubbleParticle.SetActive(false);
            measuredTime = 0;
            isShield = false;
        }
    }

    // ����Ʈ ����Ʈ �� ���� ����Ʈ ������ ��ư
    public void ExitButton()
    {
        for (int i = 0; i < 9; i++)
        {
            isHit[i] = false;
        }
        measuredTime = 0;
        questGate.SetActive(false);
        battleGate.SetActive(false);
        returnGate.SetActive(false);
        shopGate.SetActive(false);
        exitGateButton.SetActive(false);
    }
}
