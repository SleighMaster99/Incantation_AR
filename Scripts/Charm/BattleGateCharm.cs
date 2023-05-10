using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGateCharm : MonoBehaviour
{
    public GameObject battleGate;
    public GameObject returnGate;
    public GameObject spawnGateParticle;
    public GameObject spawnPoint;
    public GameObject scrollExitButton;

    float trackingTime;

    bool isOpenGate;
    bool isTracked;

    // Start is called before the first frame update
    void Start()
    {
        trackingTime = 0;

        isTracked = false;
        isOpenGate = false;
    }

    private void Update()
    {
        if (isTracked)
        {
            spawnGateParticle.SetActive(true);
            trackingTime += Time.deltaTime;

            // ������������ ��Ŀ�� �νĵǾ��� ��(��ȯ�� ��)
            if (GameManager.currentLocation != GameManager.CurrentLocation.normalGround)
            {
                if (trackingTime >= 1.5f)
                {
                    trackingTime = 0;

                    if (!isOpenGate)
                    {// ����Ʈ�� �����ִ� ����
                        returnGate.transform.position = spawnPoint.transform.position;

                        GameObject.Find("GameManager").GetComponent<GameManager>().PlaySpawnSound();

                        returnGate.SetActive(true);
                        spawnGateParticle.SetActive(false);

                        isOpenGate = true;
                        isTracked = false;
                    }
                    else
                    {// ����Ʈ�� �����ִ� ����
                        returnGate.SetActive(false);
                        spawnGateParticle.SetActive(false);

                        isOpenGate = false;
                        isTracked = false;
                    }
                }
            }
            // �⺻�������� ��Ŀ�� �νĵǾ��� ��
            else
            {
                if (trackingTime >= 1.5f)
                {
                    trackingTime = 0;

                    if (!isOpenGate)
                    {// ����Ʈ�� �����ִ� ����
                        battleGate.transform.position = spawnPoint.transform.position;

                        battleGate.SetActive(true);
                        spawnGateParticle.SetActive(false);

                        isOpenGate = true;
                        isTracked = false;
                    }
                    else
                    {// ����Ʈ�� �����ִ� ����
                        battleGate.SetActive(false);
                        spawnGateParticle.SetActive(false);

                        isOpenGate = false;
                        isTracked = false;
                    }
                }
            }
        }
    }

    // ��Ʋ����Ʈ ��Ŀ�� �ν� ���� ��
    public void BattleGateCharmTracked()
    {
        isTracked = true;
    }

    // ��Ʋ����Ʈ ��Ŀ�� �ν� ������ ��
    public void BattleGateCharmLost()
    {
        spawnGateParticle.SetActive(false);

        isTracked = false;
        trackingTime = 0;
    }

    // ��Ʋ����Ʈ ��Ȱ��ȭ
    void HideBattleGate()
    {
        battleGate.SetActive(false);
        isOpenGate = false;
    }

    // ��ȯ����Ʈ ��Ȱ��ȭ
    void HideReturnGate()
    {
        returnGate.SetActive(false);
        isOpenGate = false;
    }

    /// <summary>
    /// ����Ʈ ��ư
    /// </summary>
    /// 

    // �� ��Ʋ����Ʈ ����
    public void BasicBattleGateButton()
    {
        GameManager.isMovingLocation = true;
        GameManager.currentLocation = GameManager.CurrentLocation.basicBattleGround;
        GameManager.moveRandomTime = Random.Range(1.0f, 2.0f);
        scrollExitButton.SetActive(false);
        Invoke("HideBattleGate", 1.2f);
    }

    // �� ��Ʋ����Ʈ ����
    public void MiddleBattleGateButton()
    {
        GameManager.isMovingLocation = true;
        GameManager.currentLocation = GameManager.CurrentLocation.middleBattleGround;
        GameManager.moveRandomTime = Random.Range(1.0f, 2.0f);
        scrollExitButton.SetActive(false);
        Invoke("HideBattleGate", 1.2f);
    }

    // �� ��Ʋ����Ʈ ����
    public void BossBattleGateButton()
    {
        GameManager.isMovingLocation = true;
        GameManager.currentLocation = GameManager.CurrentLocation.bossBattleGround;
        GameManager.moveRandomTime = Random.Range(1.0f, 2.0f);
        scrollExitButton.SetActive(false);
        GhostSpawner g = GameObject.Find("GhostSpawner").GetComponent<GhostSpawner>();
        g.isBossSpawn = false;
        Invoke("HideBattleGate", 1.2f);
    }

    // ��ȯ ����Ʈ ����
    public void ReturnGateButton()
    {
        GameObject[] ghost;

        if (GameManager.currentLocation == GameManager.CurrentLocation.basicBattleGround)
        {
            ghost = GameObject.FindGameObjectsWithTag("BasicGhost");

            foreach(GameObject g in ghost)
            {
                Destroy(g);
            }
        }
        else if(GameManager.currentLocation == GameManager.CurrentLocation.middleBattleGround)
        {
            ghost = GameObject.FindGameObjectsWithTag("MiddleGhost");

            foreach (GameObject g in ghost)
            {
                Destroy(g);
            }
        }
        else
        {
            GameObject bossGhost = GameObject.FindGameObjectWithTag("BossGhost");

            Destroy(bossGhost);
        }

        GameManager.isMovingLocation = true;
        GameManager.currentLocation = GameManager.CurrentLocation.normalGround;
        GameManager.moveRandomTime = Random.Range(1.0f, 2.0f);
        scrollExitButton.SetActive(false);
        Invoke("HideReturnGate", 1.2f);
    }
}
