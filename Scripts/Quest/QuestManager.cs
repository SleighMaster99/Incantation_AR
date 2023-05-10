using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    // ���� ������ �ӹ� ����
    public Text[] questLevel;                   // �ӹ� ����
    public Text[] questTitle;                   // ����Ʈ ����
    string[] questContent;                      // �ӹ� ����
    string[] questReward;                       // �ӹ� ����
    string[] questGhost;                        // �ӹ� �� ����
    string[] questGhostCnt;                     // �ӹ� ��ġ�� �� ����

    // ������ �ӹ� ����
    int selectQuestIndex;                       // ������ �ӹ� �ε��� �������� 0, 1, 2 �ӹ� ������ 3
    public Text selectQuestLevel;               // ������ �ӹ� ����
    public Text selectQuestTitle;               // ������ �ӹ� ����
    public Text selectQuestContent;             // ������ �ӹ� ����
    public Text selectQuestReward;              // ������ �ӹ� ����
    public Text selectQuestGhost;               // ������ �ӹ� �� ����
    public Text selectQuestGhostCnt;            // ������ �ӹ� ��ġ�� �� ����

    // �ӹ� ���� ��ư
    public Button questAcceptButton;            // �ӹ� ���� ��ư
    public Text questAcceptButtonText;          // �ӹ� ���� ��ư �ؽ�Ʈ

    // �ӹ� ����
    public Text renewalTimeText;                // ���� ���� ���� �ð�
    public Text renewalCnt;                     // ������ �� �ִ� Ƚ��
    public Button renewalButton;                // ���� ��ư
    

    Dictionary<int, string[]> questData;        // �ӹ� ������

    public PlayerInformation playerInfo;        // �÷��̾� ����

    [SerializeField] float questRenewalTime;    // �ӹ� ���� �ð�

    int newIndex;                               // ���� ����Ʈ �����Ϳ��� ������ �ε���

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
            // �ӹ� ����
            RenewalQuest();
        }
    }

    // �ӹ� �����͵�
    void GenerateQuestData()
    {
        // �ϱ� �ӹ�
        questData.Add(0, new string[] { "�ϱ�", "�Ƿ� ��ġ ���� �ޱ�", "�Ƿ� �ϳ��� ���� ���ٰ� �ּ�.\n�ִ��� ���� �ߴ޸� �Ƿ��� ��ġ���ֽÿ�", "150", "����", "1" });
        questData.Add(1, new string[] { "�ϱ�", "�ּ��� �ޱ�!", "�߽��� �� ���翡 �Ⱬ�� ���� ���پ� ������ ���� �� ���� ����!", "700", "����", "5" });
        questData.Add(2, new string[] { "�ϱ�", "��Ÿ� û��", "��Ÿ��� �Ƿ��� ���ĳ� ���ذ� �����Ͽ�.\n������ ������ ������ �������� �ϼ�.", "250", "����", "3" });
        questData.Add(3, new string[] { "�ϱ�", "�ǷɼҸ��� �ȳ��� �ض�!", "�ò�����������", "100", "����", "1" });
        questData.Add(4, new string[] { "�ϱ�", "����� ����", "������ ���� �ְڴ�.\n������ ó�� �ٶ���.", "300", "����", "2" });
        questData.Add(5, new string[] { "�ϱ�", "����ּ���", "��� ���ڱ� ���� ���� ���� ��� ����մϴ�.\n�׷������� ��������� ������ ��ϴ�.\n�̳༮�� �����ּ���.", "150", "����", "1" });
        questData.Add(6, new string[] { "�߱�", "�ξ�ôϸ� ���Ҽ�", "���� �ξ�ôϸ� ���Ҽ�.\n����� ���� �Ⱬ�ϴ� � ����ֽÿ�.", "1000", "�ξ�ô�", "1" });
        questData.Add(7, new string[] { "�߱�", "�ξ�ô� �����", "�ξ�ô� ����븦 ���Ѵ�.\n�߱��̻��� ���縸 �����ٶ�.", "2500", "�ξ�ô�", "2" });
        questData.Add(8, new string[] { "���", "����̿�!!!", "��� �����ÿ�.\n������ ������� ���� ���޵Ǿ���\n�ſ� �����ϴ� ��޼��縸 ������ �ްڼ�.", "10000", "����", "1" });
    }

    // �ʺ� �ӹ� ���� �ð� ��ȯ
    void ChangeTimePerSeconds()
    {
        if (questRenewalTime >= 60.0f)
            renewalTimeText.text = (int)(questRenewalTime / 60.0f) + "�� " + (int)(questRenewalTime % 60.0f) + "��";
        else
            renewalTimeText.text = (int)questRenewalTime + "��";
    }

    // �ӹ� ���� �ð��� �Ǹ� �ӹ� ����
    void RenewalQuest()
    {
        // ���� �ӹ����� ����Ʈ�� ���� ����
        switch (playerInfo.playerQuest.index)
        {
            // ���� �÷��̾ �ϰ� �ִ� �ӹ��� ù��° ĭ �ӹ��� ��
            case 0:
                // �ι�° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count); Debug.Log("case0");

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                // ����° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[2].text = questData[newIndex][0];
                questTitle[2].text = questData[newIndex][1];
                questContent[2] = questData[newIndex][2];
                questReward[2] = questData[newIndex][3];
                questGhost[2] = questData[newIndex][4];
                questGhostCnt[2] = questData[newIndex][5];

                break;

            // ���� �÷��̾ �ϰ� �ִ� �ӹ��� �ι�° ĭ �ӹ��� ��
            case 1:
                // ù��° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                // ����° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[2].text = questData[newIndex][0];
                questTitle[2].text = questData[newIndex][1];
                questContent[2] = questData[newIndex][2];
                questReward[2] = questData[newIndex][3];
                questGhost[2] = questData[newIndex][4];
                questGhostCnt[2] = questData[newIndex][5];

                break;

            // ���� �÷��̾ �ϰ� �ִ� �ӹ��� ����° ĭ �ӹ��� ��
            case 2:
                // ù��° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                // �ι�° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                break;

            // ���� �÷��̾ �ϰ� �ִ� �ӹ��� ���� ��
            default:
                // ù��° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                // �ι�° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                // ����° ĭ �ӹ� �ʱ�ȭ
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

    // ������ ���� �ӹ� ���� ��ư ����
    void RenewalButtonManage()
    {
        renewalCnt.text = playerInfo.playerItemCnt.questRenewal.ToString();

        if (playerInfo.playerItemCnt.questRenewal > 0)
            renewalButton.interactable = true;
        else
            renewalButton.interactable = false;
    }

    // �ӹ� �Ϸ�� �ش� �ε��� �ӹ� ����
    void AfterQuestCompleteRenewal()
    {
        switch (playerInfo.playerQuest.index)
        {
            case 0:
                // ù��° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count); Debug.Log("case1");

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                break;
            case 1:
                // �ι�° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                break;

            case 2:
                // ����° ĭ �ӹ� �ʱ�ȭ
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
    /// �ӹ� ���ֿ� ������ ���� ��ư��
    /// </summary>

    // ù��° �ӹ� ���� ��ư
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
            questAcceptButtonText.text = "�ӹ���";
        }
        else
        {
            questAcceptButton.interactable = true;
            questAcceptButtonText.text = "�ӹ� ����";
        }
    }

    // �ι�° �ӹ� ���� ��ư
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
            questAcceptButtonText.text = "�ӹ���";
        }
        else
        {
            questAcceptButton.interactable = true;
            questAcceptButtonText.text = "�ӹ� ����";
        }
    }

    // ����° �ӹ� ���� ��ư
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
            questAcceptButtonText.text = "�ӹ���";
        }
        else
        {
            questAcceptButton.interactable = true;
            questAcceptButtonText.text = "�ӹ� ����";
        }
    }

    // �ӹ� ���� ��ư
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
        questAcceptButtonText.text = "�ӹ���";

        playerInfo.playerQuest.exorcismCnt = 0;
    }

    // �ӹ� ���� ��ư
    public void RenewalQuestButton()
    {
        // ���� �ӹ����� ����Ʈ�� ���� ����
        switch (playerInfo.playerQuest.index)
        {
            // ���� �÷��̾ �ϰ� �ִ� �ӹ��� ù��° ĭ �ӹ��� ��
            case 0:
                // �ι�° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                // ����° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[2].text = questData[newIndex][0];
                questTitle[2].text = questData[newIndex][1];
                questContent[2] = questData[newIndex][2];
                questReward[2] = questData[newIndex][3];
                questGhost[2] = questData[newIndex][4];
                questGhostCnt[2] = questData[newIndex][5];

                break;

            // ���� �÷��̾ �ϰ� �ִ� �ӹ��� �ι�° ĭ �ӹ��� ��
            case 1:
                // ù��° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                // ����° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[2].text = questData[newIndex][0];
                questTitle[2].text = questData[newIndex][1];
                questContent[2] = questData[newIndex][2];
                questReward[2] = questData[newIndex][3];
                questGhost[2] = questData[newIndex][4];
                questGhostCnt[2] = questData[newIndex][5];

                break;

            // ���� �÷��̾ �ϰ� �ִ� �ӹ��� ����° ĭ �ӹ��� ��
            case 2:
                // ù��° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                // �ι�° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                break;

            // ���� �÷��̾ �ϰ� �ִ� �ӹ��� ���� ��
            default:
                // ù��° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[0].text = questData[newIndex][0];
                questTitle[0].text = questData[newIndex][1];
                questContent[0] = questData[newIndex][2];
                questReward[0] = questData[newIndex][3];
                questGhost[0] = questData[newIndex][4];
                questGhostCnt[0] = questData[newIndex][5];

                // �ι�° ĭ �ӹ� �ʱ�ȭ
                newIndex = Random.Range(0, questData.Count);

                questLevel[1].text = questData[newIndex][0];
                questTitle[1].text = questData[newIndex][1];
                questContent[1] = questData[newIndex][2];
                questReward[1] = questData[newIndex][3];
                questGhost[1] = questData[newIndex][4];
                questGhostCnt[1] = questData[newIndex][5];

                // ����° ĭ �ӹ� �ʱ�ȭ
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

    // �ӹ� �Ϸ� ��ư
    public void QuestCompleteButton()
    {
        playerInfo.playerBasicInfo.playerCoin += int.Parse(playerInfo.playerQuest.reward);

        if(playerInfo.playerBasicInfo.playerClass.Equals("�� �� �� ��"))
            playerInfo.playerBasicInfo.completeCharmQuest += 1;
        else if (playerInfo.playerBasicInfo.playerClass.Equals("�� ȭ �� ��"))
            playerInfo.playerBasicInfo.completeBrushQuest += 1;

        AfterQuestCompleteRenewal();

        playerInfo.playerQuest.index = 3;
        playerInfo.playerQuest.level = null;
        playerInfo.playerQuest.title = "�������� �ӹ� ����";
        playerInfo.playerQuest.content = null;
        playerInfo.playerQuest.reward = "0";
        playerInfo.playerQuest.ghost = null;
        playerInfo.playerQuest.ghostCnt = "0";
        playerInfo.playerQuest.exorcismCnt = 0;
    }
}
