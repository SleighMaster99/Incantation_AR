using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformation : MonoBehaviour
{
    // �÷��̾ ������ ���� �ӹ� ����ü
    public struct PlayerQuest
    {
        public int index;          // ������ �ӹ��� �ӹ� ����Ʈ â �ε���
        public string level;        // �ӹ� ����
        public string title;        // �ӹ� ����
        public string content;      // �ӹ� ����
        public string reward;       // �ӹ� ����
        public string ghost;        // ��ġ�� �� ����
        public string ghostCnt;     // ��ġ�� �� ��
        public int exorcismCnt;     // ��ġ�� �� ��
    }

    // �÷��̾ ������ �ִ� ������ ���� ����ü
    public struct PlayerItemCnt
    {
        public int questRenewal;    // �ӹ� ���� ������
        public int rebornBall;      // ȯ����
        public int mentalPotion;    // ���ŷ� ����
    }

    // �÷��̾� �⺻ ����
    public struct PlayerBasicInfo
    {
        public string playerClass;           // �÷��̾� ����(��������� ��ȭ���� ��1)
        public string playerCharmLevel;      // �������� ����(�ϱ޼���, �߱޼���, ��޼���)
        public string playerBrushLevel;      // ��ȭ���� ����(�ϱ޼���, �߱޼���, ��޼���)
        public int playerCoin;               // �÷��̾� ����
        public float playerMaxMentality;     // �÷��̾� �ִ� ���ŷ�(ü��)
        public float playerMentality;        // �÷��̾� ���ŷ�(ü��)
        public int completeCharmQuest;       // �÷��̾ �������� �� �� ó���� �ӹ�
        public int completeBrushQuest;       // �÷��̾ ��ȭ���� �� �� ó���� �ӹ�
        public float magicPower;             // �÷��̾� ����
    }

    public PlayerQuest playerQuest;          // �÷��̾ ������ ���� �ӹ�
    public PlayerItemCnt playerItemCnt;      // �÷��̾ ������ �ִ� ������ ����
    public PlayerBasicInfo playerBasicInfo;  // �÷��̾� �⺻ ����

    public AudioSource audioPlayer;
    public AudioClip hitSound;
    public AudioClip deadSound;

    public GameObject shield;

    GameObject[] ghost;

    public Text potionText;

    public bool isGameOver;

    public bool isdrawSpawnTargeting;
    public bool isdrawSpawned;

    // PlayerPrefs Ű
    string[] playerQuestSavedDataKey = { "Index", "Level", "Title", "Content",
                                         "Reward", "Ghost", "GhostCnt",
                                         "ExorcismCnt" };

    string[] playerItemSavedDataKey = { "QuestRenewal", "RebornBall", "MentalPotion" };

    string[] playerBasicInfoSavedDataKey =  { "PlayerClass", "PlayerCharmLevel", "PlayerBrushLevel", 
                                              "PlayerCoin", "PlayerMaxMentality", "PlayerMentality",
                                              "CompleteCharmQuest", "CompleteBrushQuest", "MagicPower" };

    public bool isRoadData;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        isdrawSpawnTargeting = false;
        isdrawSpawned = false;
        isRoadData = false;
        PlayerQuestInitialize();
        PlayerItemInitaialize();
        PlayerBasicInfoInitialize();
        isRoadData = true;
        // ����
        /*
        playerBasicInfo.playerCoin = 20000;
        playerBasicInfo.completeBrushQuest = 25;
        playerBasicInfo.completeCharmQuest = 25;
        */
    }

    // Update is called once per frame
    void Update()
    {
        // player ���
        if(playerBasicInfo.playerMentality <= 0 && !isGameOver)
        {
            isGameOver = true;

            if(!audioPlayer.isPlaying)
                audioPlayer.PlayOneShot(deadSound);

            GameOverDestroyGhost();
            GameManager.isMovingLocation = true;
            GameManager.currentLocation = GameManager.CurrentLocation.heaven;
            GameManager.moveRandomTime = Random.Range(1.0f, 2.0f);
        }

        MaxValueManage();
        PlayerLevelManage();

        potionText.text = playerItemCnt.mentalPotion.ToString();
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    /// 

    // �÷��̾ ������ �ӹ� ������ �ҷ�����
    void PlayerQuestInitialize()
    {
        if (PlayerPrefs.HasKey(playerQuestSavedDataKey[0]))
            playerQuest.index = PlayerPrefs.GetInt(playerQuestSavedDataKey[0]);
        else
            playerQuest.index = 3;

        // �������� �ӹ� ���� ��
        if(playerQuest.index == 3)
        {
            playerQuest.title = "�������� �ӹ� ����";
            playerQuest.reward = "0";
            playerQuest.ghostCnt = "0";
            playerQuest.exorcismCnt = 0;
        }
        else
        {
            playerQuest.level = PlayerPrefs.GetString(playerQuestSavedDataKey[1]);
            playerQuest.title = PlayerPrefs.GetString(playerQuestSavedDataKey[2]);
            playerQuest.content = PlayerPrefs.GetString(playerQuestSavedDataKey[3]);
            playerQuest.reward = PlayerPrefs.GetString(playerQuestSavedDataKey[4]);
            playerQuest.ghost = PlayerPrefs.GetString(playerQuestSavedDataKey[5]);
            playerQuest.ghostCnt = PlayerPrefs.GetString(playerQuestSavedDataKey[6]);
            playerQuest.exorcismCnt = PlayerPrefs.GetInt(playerQuestSavedDataKey[7]);
        }
    }

    // �÷��̾ ������ ������ ������ �ҷ�����
    void PlayerItemInitaialize()
    {
        if (PlayerPrefs.HasKey(playerItemSavedDataKey[0]))
            playerItemCnt.questRenewal = PlayerPrefs.GetInt(playerItemSavedDataKey[0]);
        else
            playerItemCnt.questRenewal = 0;

        if (PlayerPrefs.HasKey(playerItemSavedDataKey[1]))
            playerItemCnt.rebornBall = PlayerPrefs.GetInt(playerItemSavedDataKey[1]);
        else
            playerItemCnt.rebornBall = 0;

        if (PlayerPrefs.HasKey(playerItemSavedDataKey[2]))
            playerItemCnt.mentalPotion = PlayerPrefs.GetInt(playerItemSavedDataKey[2]);
        else
            playerItemCnt.mentalPotion = 0;
    }

    // �÷��̾� ���� ������ �ҷ�����
    void PlayerBasicInfoInitialize()
    {
        /*
        if (PlayerPrefs.HasKey(playerBasicInfoSavedDataKey[0]))
            playerBasicInfo.playerClass = PlayerPrefs.GetString(playerBasicInfoSavedDataKey[0]);
        else*/
            playerBasicInfo.playerClass = "�� �� �� ��";

        if (PlayerPrefs.HasKey(playerBasicInfoSavedDataKey[1]))
            playerBasicInfo.playerCharmLevel = PlayerPrefs.GetString(playerBasicInfoSavedDataKey[1]);
        else
            playerBasicInfo.playerCharmLevel = "�ϱ�";

        if (PlayerPrefs.HasKey(playerBasicInfoSavedDataKey[2]))
            playerBasicInfo.playerBrushLevel = PlayerPrefs.GetString(playerBasicInfoSavedDataKey[2]);
        else
            playerBasicInfo.playerBrushLevel = "�ϱ�";

        if (PlayerPrefs.HasKey(playerBasicInfoSavedDataKey[3]))
            playerBasicInfo.playerCoin = PlayerPrefs.GetInt(playerBasicInfoSavedDataKey[3]);
        else
            playerBasicInfo.playerCoin = 0;

        if (PlayerPrefs.HasKey(playerBasicInfoSavedDataKey[4]))
            playerBasicInfo.playerMaxMentality = PlayerPrefs.GetFloat(playerBasicInfoSavedDataKey[4]);
        else
            playerBasicInfo.playerMaxMentality = 100.0f;

        if (PlayerPrefs.HasKey(playerBasicInfoSavedDataKey[5]))
            playerBasicInfo.playerMentality = PlayerPrefs.GetFloat(playerBasicInfoSavedDataKey[5]);
        else
            playerBasicInfo.playerMentality = playerBasicInfo.playerMaxMentality;

        if (PlayerPrefs.HasKey(playerBasicInfoSavedDataKey[6]))
            playerBasicInfo.completeCharmQuest = PlayerPrefs.GetInt(playerBasicInfoSavedDataKey[6]);
        else
            playerBasicInfo.completeCharmQuest = 0;

        if (PlayerPrefs.HasKey(playerBasicInfoSavedDataKey[7]))
            playerBasicInfo.completeBrushQuest = PlayerPrefs.GetInt(playerBasicInfoSavedDataKey[7]);
        else
            playerBasicInfo.completeBrushQuest = 0;

        if (PlayerPrefs.HasKey(playerBasicInfoSavedDataKey[8]))
            playerBasicInfo.magicPower = PlayerPrefs.GetFloat(playerBasicInfoSavedDataKey[8]);
        else
            playerBasicInfo.magicPower = 0;
    }

    /// <summary>
    /// ������ �ʱ�ȭ
    /// </summary>
    /// 

    // �ӹ� ������ �ʱ�ȭ
    public void ClearPlayerQuest()
    {
        playerQuest.index = 3;
        playerQuest.level = "�ϱ�";
        playerQuest.title = "�������� �ӹ� ����";
        playerQuest.content = null;
        playerQuest.reward = "0";
        playerQuest.ghost = null;
        playerQuest.ghostCnt = "0";
        playerQuest.exorcismCnt = 0;
    }

    // ������ ������ �ʱ�ȭ
    public void ClearPlayerItemCnt()
    {
        playerItemCnt.mentalPotion = 0;
        playerItemCnt.questRenewal = 0;
        playerItemCnt.rebornBall = 0;
    }

    // �÷��̾� ���� �ʱ�ȭ
    public void ClearPlayerBasicInfo()
    {
        
        if(playerBasicInfo.playerClass.Equals("�� �� �� ��"))
        {
            playerBasicInfo.completeCharmQuest = 0;
            playerBasicInfo.playerCharmLevel = "�ϱ�";
        }
        else if(playerBasicInfo.playerClass.Equals("�� ȭ �� ��"))
        {
            playerBasicInfo.completeBrushQuest = 0;
            playerBasicInfo.playerBrushLevel = "�ϱ�";
        }
        else
        {
            playerBasicInfo.playerCharmLevel = "�ϱ�";
            playerBasicInfo.playerBrushLevel = "�ϱ�";
        }
        playerBasicInfo.playerClass = "�� �� �� ��";
        playerBasicInfo.playerCoin = 0;
        playerBasicInfo.playerMaxMentality = 100.0f;
        playerBasicInfo.playerMentality = playerBasicInfo.playerMaxMentality;
        playerBasicInfo.magicPower = 0;
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    /// 

    // �÷��̾ ������ �ӹ� ������ ����
    public void SaveQuestData()
    {
        if (playerQuest.index != 3)
        {
            PlayerPrefs.SetInt(playerQuestSavedDataKey[0], playerQuest.index);
            PlayerPrefs.SetString(playerQuestSavedDataKey[1], playerQuest.level);
            PlayerPrefs.SetString(playerQuestSavedDataKey[2], playerQuest.title);
            PlayerPrefs.SetString(playerQuestSavedDataKey[3], playerQuest.content);
            PlayerPrefs.SetString(playerQuestSavedDataKey[4], playerQuest.reward);
            PlayerPrefs.SetString(playerQuestSavedDataKey[5], playerQuest.ghost);
            PlayerPrefs.SetString(playerQuestSavedDataKey[6], playerQuest.ghostCnt);
            PlayerPrefs.SetInt(playerQuestSavedDataKey[7], playerQuest.exorcismCnt);
        }
    }

    // �÷��̾ ������ ������ ������ ����
    public void SaveItemData()
    {
        PlayerPrefs.SetInt(playerItemSavedDataKey[0], playerItemCnt.questRenewal);
        PlayerPrefs.SetInt(playerItemSavedDataKey[1], playerItemCnt.rebornBall);
        PlayerPrefs.SetInt(playerItemSavedDataKey[2], playerItemCnt.mentalPotion);
    }

    // �÷��̾� ���� ������ ����
    public void SavePlayerInfoData()
    {
        //PlayerPrefs.SetString(playerBasicInfoSavedDataKey[0], playerBasicInfo.playerClass);
        PlayerPrefs.SetString(playerBasicInfoSavedDataKey[1], playerBasicInfo.playerCharmLevel);
        PlayerPrefs.SetString(playerBasicInfoSavedDataKey[2], playerBasicInfo.playerBrushLevel);
        PlayerPrefs.SetInt(playerBasicInfoSavedDataKey[3], playerBasicInfo.playerCoin);
        PlayerPrefs.SetFloat(playerBasicInfoSavedDataKey[4], playerBasicInfo.playerMaxMentality);
        PlayerPrefs.SetFloat(playerBasicInfoSavedDataKey[5], playerBasicInfo.playerMentality);
        PlayerPrefs.SetInt(playerBasicInfoSavedDataKey[6], playerBasicInfo.completeCharmQuest);
        PlayerPrefs.SetInt(playerBasicInfoSavedDataKey[7], playerBasicInfo.completeBrushQuest);
        PlayerPrefs.SetFloat(playerBasicInfoSavedDataKey[7], playerBasicInfo.magicPower);
    }

    /// <summary>
    /// �÷��̾� ���� ����
    /// </summary>

    // ���ŷ�, ������ �ִ� �̻��� �� �� ����
    void MaxValueManage()
    {
        if (playerBasicInfo.playerMentality > playerBasicInfo.playerMaxMentality)
            playerBasicInfo.playerMentality = playerBasicInfo.playerMaxMentality;

        // ������ 100�� �Ǹ� ȯ���� 1�� �߰�
        if (playerBasicInfo.magicPower >= 100.0f)
        {
            playerBasicInfo.magicPower = 0;
            playerItemCnt.rebornBall += 1;
        }
    }

    // �÷��̾� Ŭ������ ����
    void PlayerLevelManage()
    {
        switch (playerBasicInfo.playerClass)
        {
            case "�� �� �� ��":
                if (playerBasicInfo.completeCharmQuest >= 20)
                    playerBasicInfo.playerCharmLevel = "���";
                else if (playerBasicInfo.completeCharmQuest >= 10 && playerBasicInfo.completeCharmQuest < 20)
                    playerBasicInfo.playerCharmLevel = "�߱�";
                else
                    playerBasicInfo.playerCharmLevel = "�ϱ�";

                break;

            case "�� ȭ �� ��":
                if (playerBasicInfo.completeBrushQuest >= 20)
                    playerBasicInfo.playerBrushLevel = "���";
                else if (playerBasicInfo.completeBrushQuest >= 10 && playerBasicInfo.completeBrushQuest < 20)
                    playerBasicInfo.playerBrushLevel = "�߱�";
                else
                    playerBasicInfo.playerBrushLevel = "�ϱ�";

                break;
            default:
                if (playerBasicInfo.completeCharmQuest >= 20)
                    playerBasicInfo.playerCharmLevel = "���";
                else if (playerBasicInfo.completeCharmQuest >= 10 && playerBasicInfo.completeCharmQuest < 20)
                    playerBasicInfo.playerCharmLevel = "�߱�";
                else
                    playerBasicInfo.playerCharmLevel = "�ϱ�";

                break;

        }
    }

    // Game Over
    void GameOverDestroyGhost()
    {
        switch (GameManager.currentLocation)
        {
            case GameManager.CurrentLocation.basicBattleGround:
                ghost = GameObject.FindGameObjectsWithTag("BasicGhost");
                foreach (GameObject g in ghost)
                {
                    if (g != null)
                        Destroy(g);
                }
                break;
            case GameManager.CurrentLocation.middleBattleGround:
                ghost = GameObject.FindGameObjectsWithTag("MiddleGhost");
                foreach (GameObject g in ghost)
                {
                    if (g != null)
                        Destroy(g);
                }
                break;
            case GameManager.CurrentLocation.bossBattleGround:
                ghost = GameObject.FindGameObjectsWithTag("BossGhost");
                foreach (GameObject g in ghost)
                {
                    if (g != null)
                        Destroy(g);
                }
                break;
        }
    }

    // ���� ��� ��ư
    public void UsePotion()
    {
        if(playerItemCnt.mentalPotion > 0)
        {
            playerItemCnt.mentalPotion -= 1;
            playerBasicInfo.playerMentality += Random.Range(100.0f, 200.0f);
        }
    }
}
