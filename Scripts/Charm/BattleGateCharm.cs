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

            // 전투지역에서 마커가 인식되었을 때(귀환할 때)
            if (GameManager.currentLocation != GameManager.CurrentLocation.normalGround)
            {
                if (trackingTime >= 1.5f)
                {
                    trackingTime = 0;

                    if (!isOpenGate)
                    {// 게이트가 닫혀있는 상태
                        returnGate.transform.position = spawnPoint.transform.position;

                        GameObject.Find("GameManager").GetComponent<GameManager>().PlaySpawnSound();

                        returnGate.SetActive(true);
                        spawnGateParticle.SetActive(false);

                        isOpenGate = true;
                        isTracked = false;
                    }
                    else
                    {// 게이트가 열려있는 상태
                        returnGate.SetActive(false);
                        spawnGateParticle.SetActive(false);

                        isOpenGate = false;
                        isTracked = false;
                    }
                }
            }
            // 기본지역에서 마커가 인식되었을 때
            else
            {
                if (trackingTime >= 1.5f)
                {
                    trackingTime = 0;

                    if (!isOpenGate)
                    {// 게이트가 닫혀있는 상태
                        battleGate.transform.position = spawnPoint.transform.position;

                        battleGate.SetActive(true);
                        spawnGateParticle.SetActive(false);

                        isOpenGate = true;
                        isTracked = false;
                    }
                    else
                    {// 게이트가 열려있는 상태
                        battleGate.SetActive(false);
                        spawnGateParticle.SetActive(false);

                        isOpenGate = false;
                        isTracked = false;
                    }
                }
            }
        }
    }

    // 배틀게이트 마커가 인식 했을 때
    public void BattleGateCharmTracked()
    {
        isTracked = true;
    }

    // 배틀게이트 마커가 인식 못했을 때
    public void BattleGateCharmLost()
    {
        spawnGateParticle.SetActive(false);

        isTracked = false;
        trackingTime = 0;
    }

    // 배틀게이트 비활성화
    void HideBattleGate()
    {
        battleGate.SetActive(false);
        isOpenGate = false;
    }

    // 귀환게이트 비활성화
    void HideReturnGate()
    {
        returnGate.SetActive(false);
        isOpenGate = false;
    }

    /// <summary>
    /// 게이트 버튼
    /// </summary>
    /// 

    // 하 배틀게이트 선택
    public void BasicBattleGateButton()
    {
        GameManager.isMovingLocation = true;
        GameManager.currentLocation = GameManager.CurrentLocation.basicBattleGround;
        GameManager.moveRandomTime = Random.Range(1.0f, 2.0f);
        scrollExitButton.SetActive(false);
        Invoke("HideBattleGate", 1.2f);
    }

    // 중 배틀게이트 선택
    public void MiddleBattleGateButton()
    {
        GameManager.isMovingLocation = true;
        GameManager.currentLocation = GameManager.CurrentLocation.middleBattleGround;
        GameManager.moveRandomTime = Random.Range(1.0f, 2.0f);
        scrollExitButton.SetActive(false);
        Invoke("HideBattleGate", 1.2f);
    }

    // 상 배틀게이트 선택
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

    // 귀환 게이트 선택
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
