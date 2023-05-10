using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class BottomPannelButtons : MonoBehaviour
{
    public GameObject playerInfoPanel;
    public GameObject playerQuestPanel;
    public GameObject playerInfoButton;     // 메인메뉴 키는 버튼
    public GameObject homeButton;           // 메인메뉴 끄는 버튼

    public Button mainMenuButton;
    public Button questInfoButton;

    bool isOnLight;         // 핸드폰 라이트 토글변수
    bool isPlayerInfoPanel;
    bool isPlayerQuestPanel;

    private void Start()
    {
        isOnLight = false;
        isPlayerInfoPanel = false;
        isPlayerQuestPanel = false;
    }

    private void Update()
    {
        MoveMainPanel();
        MovePlayerQuestPanel();
    }

    // 메인 메뉴 패널 움직이기
    void MoveMainPanel()
    {
        if (isPlayerInfoPanel)
        {
            if (playerInfoPanel.transform.position.y <= 900)
                playerInfoPanel.transform.position += new Vector3(0, 1600.0f, 0) * Time.deltaTime;

            if (playerInfoPanel.transform.position.y > 900)
                playerInfoPanel.transform.position = new Vector3(720.0f, 900.0f, 0);
        }
        else
        {
            if (playerInfoPanel.transform.position.y >= -700)
                playerInfoPanel.transform.position -= new Vector3(0, 1600.0f, 0) * Time.deltaTime;

            if (playerInfoPanel.transform.position.y < -700)
                playerInfoPanel.transform.position = new Vector3(720.0f, -700.0f, 0);
        }
    }

    // 퀘스트 메뉴 패널 움직이기
    void MovePlayerQuestPanel()
    {
        if (isPlayerQuestPanel)
        {
            if (playerQuestPanel.transform.position.y <= 900)
                playerQuestPanel.transform.position += new Vector3(0, 1600.0f, 0) * Time.deltaTime;

            if (playerQuestPanel.transform.position.y > 900)
                playerQuestPanel.transform.position = new Vector3(720.0f, 900.0f, 0);
        }
        else
        {
            if (playerQuestPanel.transform.position.y >= -700)
                playerQuestPanel.transform.position -= new Vector3(0, 1600.0f, 0) * Time.deltaTime;

            if (playerQuestPanel.transform.position.y < -700)
                playerQuestPanel.transform.position = new Vector3(720.0f, -700.0f, 0);
        }
    }

    /// <summary>
    /// 버튼
    /// </summary>

    // 핸드폰 라이트 버튼
    public void SetFlashButton()
    {
        isOnLight = !isOnLight;
        VuforiaBehaviour.Instance.CameraDevice.SetFlash(isOnLight);
    }

    // 플레이어 정보 버튼(메인 메뉴 버튼)
    public void MainMenuButton()
    {
        isPlayerInfoPanel = !isPlayerInfoPanel;
        
        playerInfoButton.SetActive(!isPlayerInfoPanel);
        homeButton.SetActive(isPlayerInfoPanel);

        questInfoButton.interactable = !isPlayerInfoPanel;
    }

    // 플레이어가 수주한 임무 버튼
    public void PlayerQuestButton()
    {
        isPlayerQuestPanel = !isPlayerQuestPanel;
        mainMenuButton.interactable = !isPlayerQuestPanel;
    }
}
