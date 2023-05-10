using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainCanvasManager : MonoBehaviour
{
    public PlayerInformation playerInform;

    public Slider playerMentalitySlider;
    public Slider magicPowerSlider;

    public Image mentalityRightBackground2;

    // �����г�
    public Text playerClassText;
    public Text playerLevelText;
    public Text completeQuestCntText;
    public Text rebornBallCntText;
    public Button saveButton;
    
    // �÷��̾ ������ �ӹ� ��� ����
    public Text playerQuestLevelText;
    public Text playerQuestTitleText;
    public Text playerQuestContentText;
    public Text playerQuestRewardText;
    public Text playerQuestGhostText;
    public Text playerQuestGhostCntText;
    public Text playerQuestExocismCntText;
    public Button questCompleteButton;

    // GameOver �г�
    public GameObject gameOverPanel;
    public Button rebornButton;

    // Brush ��ư
    public Button openScrollButton;
    public Button drawSpawnButton;
    public GameObject scroll;
    public GameObject scrollParent;
    public GameObject scrollSpawnPoint;
    public GameObject brushCube;
    public AudioSource audioPlayer;
    public AudioClip brushSounds;
    bool isScroll;

    // ���ӽ��� ��ư
    public GameObject startPanel;


    // Start is called before the first frame update
    void Start()
    {
        isScroll = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMental();
        MainMenuPannel();
        PlayerQuestPanel();
        if (playerInform.isGameOver)
        {
            GameOverPanel();
        }
    }

    // ��� �÷��̾� ���ŷ� �г�
    void PlayerMental()
    {
        playerMentalitySlider.maxValue = playerInform.playerBasicInfo.playerMaxMentality;
        playerMentalitySlider.value = playerInform.playerBasicInfo.playerMentality;

        if (playerInform.playerBasicInfo.playerMentality >= 100.0f)
            mentalityRightBackground2.gameObject.SetActive(true);
        else
            mentalityRightBackground2.gameObject.SetActive(false);
    }

    // ���θ޴� �г�
    void MainMenuPannel()
    {
        playerClassText.text = playerInform.playerBasicInfo.playerClass;
        if (playerInform.playerBasicInfo.playerClass.Equals("�� �� �� ��"))
        {
            playerLevelText.text = playerInform.playerBasicInfo.playerCharmLevel;
            completeQuestCntText.text = playerInform.playerBasicInfo.completeCharmQuest.ToString();
        }
        else if(playerInform.playerBasicInfo.playerClass.Equals("�� ȭ �� ��"))
        {
            playerLevelText.text = playerInform.playerBasicInfo.playerBrushLevel;
            completeQuestCntText.text = playerInform.playerBasicInfo.completeBrushQuest.ToString();
        }
        else
        {
            playerLevelText.text = playerInform.playerBasicInfo.playerCharmLevel;
            completeQuestCntText.text = playerInform.playerBasicInfo.completeCharmQuest.ToString();
        }
        
        rebornBallCntText.text = playerInform.playerItemCnt.rebornBall.ToString();
        magicPowerSlider.value = playerInform.playerBasicInfo.magicPower;

        if (GameManager.currentLocation != GameManager.CurrentLocation.normalGround)
            saveButton.interactable = false;
        else
            saveButton.interactable = true;
    }

    // ���� �������� �ӹ� �г�
    void PlayerQuestPanel()
    {
        playerQuestLevelText.text = playerInform.playerQuest.level;
        playerQuestTitleText.text = playerInform.playerQuest.title;
        playerQuestContentText.text = playerInform.playerQuest.content;
        playerQuestRewardText.text = playerInform.playerQuest.reward;
        playerQuestGhostText.text = playerInform.playerQuest.ghost;
        playerQuestGhostCntText.text = playerInform.playerQuest.ghostCnt + "����";
        playerQuestExocismCntText.text = playerInform.playerQuest.exorcismCnt.ToString() + "����";

        if ((playerInform.playerQuest.exorcismCnt >= int.Parse(playerInform.playerQuest.ghostCnt))
            && playerInform.playerQuest.index != 3)
            questCompleteButton.interactable = true;
        else
            questCompleteButton.interactable = false;
    }

    // ���� ���� �г�
    void GameOverPanel()
    {
        gameOverPanel.SetActive(true);
        if (playerInform.playerItemCnt.rebornBall > 0)
            rebornButton.interactable = true;
        else
            rebornButton.interactable = false;
    }
    /*
    // BrushPanel
    void BrushPanel()
    {
        openScrollButton.gameObject.SetActive(true);
    }*/

    /// <summary>
    /// ��ư
    /// </summary>

    // ���� ��ư
    public void SaveButton()
    {
        playerInform.SaveQuestData();
        playerInform.SaveItemData();
        playerInform.SavePlayerInfoData();
    }

    // ���� ��ư
    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ���� ���� ��ư
    public void ChangeClassButton()
    {
        PlayerInformation pi = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformation>();
        pi.playerBasicInfo.playerClass = "�� �� �� ��";
        GameManager.isSelectClass = false;

        SceneManager.LoadScene("SampleScene");
    }

    // ��ȸ ��ư
    public void RebornButton()
    {
        playerInform.playerItemCnt.rebornBall--;

        playerInform.ClearPlayerQuest();
        playerInform.SaveItemData();
        playerInform.SavePlayerInfoData();

        SceneManager.LoadScene("SampleScene");
    }

    // ���ο� �� ��ư
    public void NewLifeButton()
    {
        playerInform.ClearPlayerQuest();
        playerInform.ClearPlayerItemCnt();
        playerInform.ClearPlayerBasicInfo();
        playerInform.SaveQuestData();
        playerInform.SaveItemData();
        playerInform.SavePlayerInfoData();

        SceneManager.LoadScene("SampleScene");
    }

    // ��ũ�� ���� ��ư
    public void OpenScrollButton()
    {
        isScroll = true;
        scroll.transform.position = scrollSpawnPoint.transform.position;
        scroll.transform.rotation = scrollSpawnPoint.transform.rotation;
        scroll.SetActive(true);
        if (!audioPlayer.isPlaying)
            audioPlayer.PlayOneShot(brushSounds);
        openScrollButton.gameObject.SetActive(false);
        drawSpawnButton.gameObject.SetActive(true);
    }

    // ��ȯ ��ư
    public void DrawSpawnButton()
    {
        isScroll = false;
        playerInform.isdrawSpawnTargeting = true;
        scrollParent.GetComponent<Scroll>().ScrollRay();

        if (!audioPlayer.isPlaying)
            audioPlayer.PlayOneShot(brushSounds);

        openScrollButton.gameObject.SetActive(true);
        drawSpawnButton.gameObject.SetActive(false);
    }

    // StartGameButton
    public void StartGameButton()
    {
        startPanel.SetActive(false);
    }
}
