using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject basicGhost;           // ����
    public GameObject middleGhost;          // �ξ�ô�
    public GameObject BossGhost;            // ����
    

    Vector3 spawnPoint;

    float randomSpawnTime;                  // spawnTime�� randomSpawnTime �ð��� �Ǹ� ����
    float spawnTime;                        // ������ �ð� ����
    float maxDistance;                      // ���� �ִ� �Ÿ�
    float xPosition;
    float yPosition;
    float zPosition;

    public bool isBossSpawn;

    // Start is called before the first frame update
    void Start()
    {
        randomSpawnTime = Random.Range(20.0f, 30.0f);
        spawnTime = randomSpawnTime;
        maxDistance = 80.0f;

        isBossSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.currentLocation)
        {
            case GameManager.CurrentLocation.basicBattleGround:
                BasicGhostSpawn();
                break;

            case GameManager.CurrentLocation.middleBattleGround:
                MiddleGhostSpawn();
                break;

            case GameManager.CurrentLocation.bossBattleGround:
                if (!isBossSpawn)
                    BossGhostSpawn();
                break;
        }
    }

    void BasicGhostSpawn()
    {
        spawnTime += Time.deltaTime;

        if (randomSpawnTime <= spawnTime)
        {
            // �� �ȿ� ���� ����
            // �ִ�Ÿ� ���� �ּҰŸ� �̻� ����

            //float d = (maxDistance - minDistance) * Random.value;
            //Vector3 point = Random.onUnitSphere * (maxDistance - d);

            GhostRandomPosition();

            Instantiate(basicGhost, spawnPoint, basicGhost.transform.rotation);

            spawnTime = 0;
            randomSpawnTime = Random.Range(20.0f, 30.0f);
        }
    }

    void MiddleGhostSpawn()
    {
        spawnTime += Time.deltaTime;

        if (randomSpawnTime <= spawnTime)
        {
            // �� �ȿ� ���� ����
            // �ִ�Ÿ� ���� �ּҰŸ� �̻� ����

            //float d = (maxDistance - minDistance) * Random.value;
            //Vector3 point = Random.onUnitSphere * (maxDistance - d);

            GhostRandomPosition();

            Instantiate(middleGhost, spawnPoint, middleGhost.transform.rotation);

            spawnTime = 0;
            randomSpawnTime = Random.Range(20.0f, 30.0f);
        }
    }

    void BossGhostSpawn()
    {
        xPosition = Random.Range(-40.0f, 40.0f);
        yPosition = -1.0f;
        zPosition = Mathf.Sqrt(Mathf.Pow(40.0f, 2) - Mathf.Pow(xPosition, 2)) * RandomPlusMinus();
        spawnPoint = new Vector3(xPosition, yPosition, zPosition);

        Instantiate(BossGhost, spawnPoint, BossGhost.transform.rotation);

        isBossSpawn = true;
    }

    // -1 �Ǵ� 1�� ���ϴ� �Լ�
    float RandomPlusMinus()
    {
        float rn;

        if (Random.value > 0.5f)
            rn = 1.0f;
        else
            rn = -1.0f;

        return rn;
    }


    // ghost�� ������ ����
    void GhostRandomPosition()
    {
        xPosition = (maxDistance - Random.Range(0, 30.0f)) * RandomPlusMinus();
        yPosition = Random.Range(-15.0f, 11.0f);
        zPosition = (maxDistance - Random.Range(0, 30.0f)) * RandomPlusMinus();
        spawnPoint = new Vector3(xPosition, yPosition, zPosition);
    }
}
