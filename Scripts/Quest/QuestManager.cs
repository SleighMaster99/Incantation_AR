using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    // 랜덤 생성한 임무 정보
    public Text[] questLevel;                   // 임무 레벨
    public Text[] questTitle;                   // 퀘스트 제목
    string[] questContent;                      // 임무 내용
    string[] questReward;                       // 임무 보상
    string[] questGhost;                        // 임무 령 종류
    string[] questGhostCnt;                     // 임무 퇴치할 령 숫자

    // 선택한 임무 정보
    int selectQuestIndex;                       // 선택한 임무 인덱스 위에부터 0, 1, 2 임무 없을시 3
    public Text selectQuestLevel;               // 선택한 임무 레벨
    public Text selectQuestTitle;               // 선택한 임무 제목
    public Text selectQuestContent;             // 선택한 임무 내용
    public Text selectQuestReward;              // 선택한 임무 보상
    public Text selectQuestGhost;               // 선택한 임무 령 종류
    public Text selectQuestGhostCnt;            // 선택한 임무 퇴치할 령 숫자

    // 임무 수주 버튼
    public Button questAcceptButton;            // 임무 수주 버튼
    public Text questAcceptButtonText;          // 임무 수주 버튼 텍스트

    // 임무 갱신
    public Text renewalTimeText;                // 갱신 까지 남은 시간
    public Text renewalCnt;                     // 갱신할 수 있는 횟수
    public Button renewalButton;                // 갱신 버튼
    

    Dictionary<int, string[]> questData;        // 임무 데이터

    public PlayerInformation playerInfo;        // 플레이어 정보

    [SerializeField] float questRenewalTime;    // 임무 갱신 시간

    int newIndex;                               // 현재 퀘스트 데이터에서 가져올 인덱스

    // Start is called before the first frame update
    void Start()
    {
        questContent = new string[3];
        questReward = new string[3];
        questGhost = new string[3];
        questGhostCnt = new string[3];

        questData = new Dictionary<int, string[]>();
        GenerateQuestData();

        newIndex = 0;

        if(playerInfo.isRoadData)
            RenewalQuest();
    }

    // Update is called once per frame
    void Update()
    {
        questRenewalTime -= Time.deltaTime;
        ChangeTimePerSeconds();

        RenewalButtonManage();

        if (questRenewalTime <= 0)
        {
            // 임무 갱신
            RenewalQuest();
        }
    }

    // 임무 데이터들
    void GenerateQuestData()
    {
        // 하급 임무
        questData.Add(0, new string[] { "하급", "악령 퇴치 도사 급구", "악령 하나가 미쳐 날뛰고 있소.\n최대한 빨리 발달린 악령을 퇴치해주시오", "150", "수비", "1" });
        questData.Add(1, new string[] { "하급", "주술사 급구!", "야심한 밤 마당에 기괴한 것이 날뛰어 도저히 잠을 잘 수가 없소!", "700", "수비", "5" });
        questData.Add(2, new string[] { "하급", "길거리 청소", "길거리에 악령이 넘쳐나 피해가 막심하오.\n보수는 적지만 도사의 의협심을 믿소.", "250", "수비", "3" });
        questData.Add(3, new string[] { "하급", "악령소리좀 안나게 해라!", "시끄러어어어어어어어", "100", "수비", "1" });
        questData.Add(4, new string[] { "하급", "도사놈 구함", "보수는 많이 주겠다.\n빠르게 처리 바란다.", "300", "수비", "2" });
        questData.Add(5, new string[] { "하급", "살려주세요", "흰색 보자기 같은 것이 나를 계속 통과합니다.\n그럴때마다 우울해지는 느낌이 듭니다.\n이녀석을 없애주세요.", "150", "수비", "1" });
        questData.Add(6, new string[] { "중급", "두억시니를 보았소", "내가 두억시니를 보았소.\n모습이 아주 기괴하니 어서 잡아주시오.", "1000", "두억시니", "1" });
        questData.Add(7, new string[] { "중급", "두억시니 토벌대", "두억시니 토벌대를 구한다.\n중급이상의 술사만 지원바람.", "2500", "두억시니", "2" });
        questData.Add(8, new string[] { "상급", "어명이오!!!", "모두 들으시오.\n마왕을 잡으라는 명이 전달되었소\n매우 위험하니 상급술사만 지원을 받겠소.", "10000", "마왕", "1" });
    }

    // 초별 임무 갱신 시간 변환
    void ChangeTimePerSeconds()
    {
        if (questRenewalTime >= 60.0f)
            renewalTimeText.text = (int)(questRenewalTime / 60.0f) + "분 " + (int)(questRenewalTime % 60.0f) + "초";
        else
            renewalTimeText.text = (int)questRenewalTime + "초";
    }

    // 임무 갱신 시간이 되면 임무 갱신
    void RenewalQuest()
    {
        // 현재 임무중인 퀘스트에 따른 갱신
        switch (playerInfo.playerQuest.index)
        {
            // 현재 플레이어가 하고 있는 임무가 첫번째 칸 임무일 때
            case 0:
                // 두번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count); Debug.Log("case0");

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                // 세번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[2].text = questData[newIndex][0];
                questTitle[2].text = questData[newIndex][1];
                questContent[2] = questData[newIndex][2];
                questReward[2] = questData[newIndex][3];
                questGhost[2] = questData[newIndex][4];
                questGhostCnt[2] = questData[newIndex][5];

                break;

            // 현재 플레이어가 하고 있는 임무가 두번째 칸 임무일 때
            case 1:
                // 첫번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                // 세번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[2].text = questData[newIndex][0];
                questTitle[2].text = questData[newIndex][1];
                questContent[2] = questData[newIndex][2];
                questReward[2] = questData[newIndex][3];
                questGhost[2] = questData[newIndex][4];
                questGhostCnt[2] = questData[newIndex][5];

                break;

            // 현재 플레이어가 하고 있는 임무가 세번째 칸 임무일 때
            case 2:
                // 첫번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                // 두번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                break;

            // 현재 플레이어가 하고 있는 임무가 없을 때
            default:
                // 첫번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                // 두번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                // 세번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[2].text = questData[newIndex][0];
                questTitle[2].text = questData[newIndex][1];
                questContent[2] = questData[newIndex][2];
                questReward[2] = questData[newIndex][3];
                questGhost[2] = questData[newIndex][4];
                questGhostCnt[2] = questData[newIndex][5];

                break;

        }

        questRenewalTime = 1800.0f;
    }

    // 갯수에 따른 임무 갱신 버튼 구성
    void RenewalButtonManage()
    {
        renewalCnt.text = playerInfo.playerItemCnt.questRenewal.ToString();

        if (playerInfo.playerItemCnt.questRenewal > 0)
            renewalButton.interactable = true;
        else
            renewalButton.interactable = false;
    }

    // 임무 완료시 해당 인덱스 임무 갱신
    void AfterQuestCompleteRenewal()
    {
        switch (playerInfo.playerQuest.index)
        {
            case 0:
                // 첫번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count); Debug.Log("case1");

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                break;
            case 1:
                // 두번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                break;

            case 2:
                // 세번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[2].text = questData[newIndex][0];
                questTitle[2].text = questData[newIndex][1];
                questContent[2] = questData[newIndex][2];
                questReward[2] = questData[newIndex][3];
                questGhost[2] = questData[newIndex][4];
                questGhostCnt[2] = questData[newIndex][5];

                break;
        }
    }



    /// <summary>
    /// 임무 수주와 생성에 관한 버튼들
    /// </summary>

    // 첫번째 임무 선택 버튼
    public void SelectQuest0()
    {
        selectQuestIndex = 0;
        selectQuestLevel.text = questLevel[0].text;
        selectQuestTitle.text = questTitle[0].text;
        selectQuestContent.text = questContent[0];
        selectQuestReward.text = questReward[0];
        selectQuestGhost.text = questGhost[0];
        selectQuestGhostCnt.text = questGhostCnt[0];

        if(playerInfo.playerQuest.index == selectQuestIndex)
        {
            questAcceptButton.interactable = false;
            questAcceptButtonText.text = "임무중";
        }
        else
        {
            questAcceptButton.interactable = true;
            questAcceptButtonText.text = "임무 수주";
        }
    }

    // 두번째 임무 선택 버튼
    public void SelectQuest1()
    {
        selectQuestIndex = 1;
        selectQuestLevel.text = questLevel[1].text;
        selectQuestTitle.text = questTitle[1].text;
        selectQuestContent.text = questContent[1];
        selectQuestReward.text = questReward[1];
        selectQuestGhost.text = questGhost[1];
        selectQuestGhostCnt.text = questGhostCnt[1];

        if (playerInfo.playerQuest.index == selectQuestIndex)
        {
            questAcceptButton.interactable = false;
            questAcceptButtonText.text = "임무중";
        }
        else
        {
            questAcceptButton.interactable = true;
            questAcceptButtonText.text = "임무 수주";
        }
    }

    // 세번째 임무 선택 버튼
    public void SelectQuest2()
    {
        selectQuestIndex = 2;
        selectQuestLevel.text = questLevel[2].text;
        selectQuestTitle.text = questTitle[2].text;
        selectQuestContent.text = questContent[2];
        selectQuestReward.text = questReward[2];
        selectQuestGhost.text = questGhost[2];
        selectQuestGhostCnt.text = questGhostCnt[2];

        if (playerInfo.playerQuest.index == selectQuestIndex)
        {
            questAcceptButton.interactable = false;
            questAcceptButtonText.text = "임무중";
        }
        else
        {
            questAcceptButton.interactable = true;
            questAcceptButtonText.text = "임무 수주";
        }
    }

    // 임무 수주 버튼
    public void AcceptSelectQuest()
    {
        playerInfo.playerQuest.index = selectQuestIndex;
        playerInfo.playerQuest.level = selectQuestLevel.text;
        playerInfo.playerQuest.title = selectQuestTitle.text;
        playerInfo.playerQuest.content = selectQuestContent.text;
        playerInfo.playerQuest.reward = selectQuestReward.text;
        playerInfo.playerQuest.ghost = selectQuestGhost.text;
        playerInfo.playerQuest.ghostCnt = selectQuestGhostCnt.text;

        questAcceptButton.interactable = false;
        questAcceptButtonText.text = "임무중";

        playerInfo.playerQuest.exorcismCnt = 0;
    }

    // 임무 갱신 버튼
    public void RenewalQuestButton()
    {
        // 현재 임무중인 퀘스트에 따른 갱신
        switch (playerInfo.playerQuest.index)
        {
            // 현재 플레이어가 하고 있는 임무가 첫번째 칸 임무일 때
            case 0:
                // 두번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                // 세번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[2].text = questData[newIndex][0];
                questTitle[2].text = questData[newIndex][1];
                questContent[2] = questData[newIndex][2];
                questReward[2] = questData[newIndex][3];
                questGhost[2] = questData[newIndex][4];
                questGhostCnt[2] = questData[newIndex][5];

                break;

            // 현재 플레이어가 하고 있는 임무가 두번째 칸 임무일 때
            case 1:
                // 첫번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                // 세번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[2].text = questData[newIndex][0];
                questTitle[2].text = questData[newIndex][1];
                questContent[2] = questData[newIndex][2];
                questReward[2] = questData[newIndex][3];
                questGhost[2] = questData[newIndex][4];
                questGhostCnt[2] = questData[newIndex][5];

                break;

            // 현재 플레이어가 하고 있는 임무가 세번째 칸 임무일 때
            case 2:
                // 첫번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                // 두번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                break;

            // 현재 플레이어가 하고 있는 임무가 없을 때
            default:
                // 첫번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                // 두번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                // 세번째 칸 임무 초기화
                newIndex = Random.Range(0, questData.Count);

                questLevel[2].text = questData[newIndex][0];
                questTitle[2].text = questData[newIndex][1];
                questContent[2] = questData[newIndex][2];
                questReward[2] = questData[newIndex][3];
                questGhost[2] = questData[newIndex][4];
                questGhostCnt[2] = questData[newIndex][5];

                break;

        }

        questRenewalTime = 1800.0f;
        playerInfo.playerItemCnt.questRenewal -= 1;
    }

    // 임무 완료 버튼
    public void QuestCompleteButton()
    {
        playerInfo.playerBasicInfo.playerCoin += int.Parse(playerInfo.playerQuest.reward);

        if(playerInfo.playerBasicInfo.playerClass.Equals("부 적 술 사"))
            playerInfo.playerBasicInfo.completeCharmQuest += 1;
        else if (playerInfo.playerBasicInfo.playerClass.Equals("묵 화 술 사"))
            playerInfo.playerBasicInfo.completeBrushQuest += 1;

        AfterQuestCompleteRenewal();

        playerInfo.playerQuest.index = 3;
        playerInfo.playerQuest.level = null;
        playerInfo.playerQuest.title = "수행중인 임무 없음";
        playerInfo.playerQuest.content = null;
        playerInfo.playerQuest.reward = "0";
        playerInfo.playerQuest.ghost = null;
        playerInfo.playerQuest.ghostCnt = "0";
        playerInfo.playerQuest.exorcismCnt = 0;
    }
}
