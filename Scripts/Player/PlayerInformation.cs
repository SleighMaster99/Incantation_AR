using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformation : MonoBehaviour
{
    // 플레이어가 수주한 현재 임무 구조체
    public struct PlayerQuest
    {
        public int index;          // 수락한 임무의 임무 게이트 창 인덱스
        public string level;        // 임무 레벨
        public string title;        // 임무 제목
        public string content;      // 임무 내용
        public string reward;       // 임무 보상
        public string ghost;        // 퇴치할 령 종류
        public string ghostCnt;     // 퇴치할 령 수
        public int exorcismCnt;     // 퇴치한 령 수
    }

    // 플레이어가 가지고 있는 아이템 개수 구조체
    public struct PlayerItemCnt
    {
        public int questRenewal;    // 임무 갱신 아이템
        public int rebornBall;      // 환생구
        public int mentalPotion;    // 정신력 포션
    }

    // 플레이어 기본 정보
    public struct PlayerBasicInfo
    {
        public string playerClass;           // 플레이어 직업(부적술사와 묵화술사 택1)
        public string playerCharmLevel;      // 부적술사 레벨(하급술사, 중급술사, 상급술사)
        public string playerBrushLevel;      // 묵화술사 레벨(하급술사, 중급술사, 상급술사)
        public int playerCoin;               // 플레이어 엽전
        public float playerMaxMentality;     // 플레이어 최대 정신력(체력)
        public float playerMentality;        // 플레이어 정신력(체력)
        public int completeCharmQuest;       // 플레이어가 부적술사 일 때 처리한 임무
        public int completeBrushQuest;       // 플레이어가 묵화술사 일 때 처리한 임무
        public float magicPower;             // 플레이어 도력
    }

    public PlayerQuest playerQuest;          // 플레이어가 수주한 현재 임무
    public PlayerItemCnt playerItemCnt;      // 플레이어가 가지고 있는 아이템 갯수
    public PlayerBasicInfo playerBasicInfo;  // 플레이어 기본 정보

    public AudioSource audioPlayer;
    public AudioClip hitSound;
    public AudioClip deadSound;

    public GameObject shield;

    GameObject[] ghost;

    public Text potionText;

    public bool isGameOver;

    public bool isdrawSpawnTargeting;
    public bool isdrawSpawned;

    // PlayerPrefs 키
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
        // 버그
        /*
        playerBasicInfo.playerCoin = 20000;
        playerBasicInfo.completeBrushQuest = 25;
        playerBasicInfo.completeCharmQuest = 25;
        */
    }

    // Update is called once per frame
    void Update()
    {
        // player 사망
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
    /// 데이터 로드
    /// </summary>
    /// 

    // 플레이어가 수주한 임무 데이터 불러오기
    void PlayerQuestInitialize()
    {
        if (PlayerPrefs.HasKey(playerQuestSavedDataKey[0]))
            playerQuest.index = PlayerPrefs.GetInt(playerQuestSavedDataKey[0]);
        else
            playerQuest.index = 3;

        // 수행중인 임무 없을 때
        if(playerQuest.index == 3)
        {
            playerQuest.title = "수행중인 임무 없음";
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

    // 플레이어가 소지한 아이템 데이터 불러오기
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

    // 플레이어 정보 데이터 불러오기
    void PlayerBasicInfoInitialize()
    {
        /*
        if (PlayerPrefs.HasKey(playerBasicInfoSavedDataKey[0]))
            playerBasicInfo.playerClass = PlayerPrefs.GetString(playerBasicInfoSavedDataKey[0]);
        else*/
            playerBasicInfo.playerClass = "일 반 술 사";

        if (PlayerPrefs.HasKey(playerBasicInfoSavedDataKey[1]))
            playerBasicInfo.playerCharmLevel = PlayerPrefs.GetString(playerBasicInfoSavedDataKey[1]);
        else
            playerBasicInfo.playerCharmLevel = "하급";

        if (PlayerPrefs.HasKey(playerBasicInfoSavedDataKey[2]))
            playerBasicInfo.playerBrushLevel = PlayerPrefs.GetString(playerBasicInfoSavedDataKey[2]);
        else
            playerBasicInfo.playerBrushLevel = "하급";

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
    /// 데이터 초기화
    /// </summary>
    /// 

    // 임무 데이터 초기화
    public void ClearPlayerQuest()
    {
        playerQuest.index = 3;
        playerQuest.level = "하급";
        playerQuest.title = "수행중인 임무 없음";
        playerQuest.content = null;
        playerQuest.reward = "0";
        playerQuest.ghost = null;
        playerQuest.ghostCnt = "0";
        playerQuest.exorcismCnt = 0;
    }

    // 소지한 아이템 초기화
    public void ClearPlayerItemCnt()
    {
        playerItemCnt.mentalPotion = 0;
        playerItemCnt.questRenewal = 0;
        playerItemCnt.rebornBall = 0;
    }

    // 플레이어 정보 초기화
    public void ClearPlayerBasicInfo()
    {
        
        if(playerBasicInfo.playerClass.Equals("부 적 술 사"))
        {
            playerBasicInfo.completeCharmQuest = 0;
            playerBasicInfo.playerCharmLevel = "하급";
        }
        else if(playerBasicInfo.playerClass.Equals("묵 화 술 사"))
        {
            playerBasicInfo.completeBrushQuest = 0;
            playerBasicInfo.playerBrushLevel = "하급";
        }
        else
        {
            playerBasicInfo.playerCharmLevel = "하급";
            playerBasicInfo.playerBrushLevel = "하급";
        }
        playerBasicInfo.playerClass = "일 반 술 사";
        playerBasicInfo.playerCoin = 0;
        playerBasicInfo.playerMaxMentality = 100.0f;
        playerBasicInfo.playerMentality = playerBasicInfo.playerMaxMentality;
        playerBasicInfo.magicPower = 0;
    }

    /// <summary>
    /// 데이터 저장
    /// </summary>
    /// 

    // 플레이어가 수주한 임무 데이터 저장
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

    // 플레이어가 소지한 아이템 데이터 저장
    public void SaveItemData()
    {
        PlayerPrefs.SetInt(playerItemSavedDataKey[0], playerItemCnt.questRenewal);
        PlayerPrefs.SetInt(playerItemSavedDataKey[1], playerItemCnt.rebornBall);
        PlayerPrefs.SetInt(playerItemSavedDataKey[2], playerItemCnt.mentalPotion);
    }

    // 플레이어 정보 데이터 저장
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
    /// 플레이어 정보 설정
    /// </summary>

    // 정신력, 도력이 최댓값 이상일 때 값 설정
    void MaxValueManage()
    {
        if (playerBasicInfo.playerMentality > playerBasicInfo.playerMaxMentality)
            playerBasicInfo.playerMentality = playerBasicInfo.playerMaxMentality;

        // 도력이 100이 되면 환생구 1개 추가
        if (playerBasicInfo.magicPower >= 100.0f)
        {
            playerBasicInfo.magicPower = 0;
            playerItemCnt.rebornBall += 1;
        }
    }

    // 플레이어 클래스별 레벨
    void PlayerLevelManage()
    {
        switch (playerBasicInfo.playerClass)
        {
            case "부 적 술 사":
                if (playerBasicInfo.completeCharmQuest >= 20)
                    playerBasicInfo.playerCharmLevel = "상급";
                else if (playerBasicInfo.completeCharmQuest >= 10 && playerBasicInfo.completeCharmQuest < 20)
                    playerBasicInfo.playerCharmLevel = "중급";
                else
                    playerBasicInfo.playerCharmLevel = "하급";

                break;

            case "묵 화 술 사":
                if (playerBasicInfo.completeBrushQuest >= 20)
                    playerBasicInfo.playerBrushLevel = "상급";
                else if (playerBasicInfo.completeBrushQuest >= 10 && playerBasicInfo.completeBrushQuest < 20)
                    playerBasicInfo.playerBrushLevel = "중급";
                else
                    playerBasicInfo.playerBrushLevel = "하급";

                break;
            default:
                if (playerBasicInfo.completeCharmQuest >= 20)
                    playerBasicInfo.playerCharmLevel = "상급";
                else if (playerBasicInfo.completeCharmQuest >= 10 && playerBasicInfo.completeCharmQuest < 20)
                    playerBasicInfo.playerCharmLevel = "중급";
                else
                    playerBasicInfo.playerCharmLevel = "하급";

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

    // 포션 사용 버튼
    public void UsePotion()
    {
        if(playerItemCnt.mentalPotion > 0)
        {
            playerItemCnt.mentalPotion -= 1;
            playerBasicInfo.playerMentality += Random.Range(100.0f, 200.0f);
        }
    }
}
