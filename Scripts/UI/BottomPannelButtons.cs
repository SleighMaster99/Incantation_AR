using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class BottomPannelButtons : MonoBehaviour
{
    public GameObject playerInfoPanel;
    public GameObject playerQuestPanel;
    public GameObject playerInfoButton;     // ���θ޴� Ű�� ��ư
    public GameObject homeButton;           // ���θ޴� ���� ��ư

    public Button mainMenuButton;
    public Button questInfoButton;

    bool isOnLight;         // �ڵ��� ����Ʈ ��ۺ���
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

    // ���� �޴� �г� �����̱�
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

    // ����Ʈ �޴� �г� �����̱�
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
    /// ��ư
    /// </summary>

    // �ڵ��� ����Ʈ ��ư
    public void SetFlashButton()
    {
        isOnLight = !isOnLight;
        VuforiaBehaviour.Instance.CameraDevice.SetFlash(isOnLight);
    }

    // �÷��̾� ���� ��ư(���� �޴� ��ư)
    public void MainMenuButton()
    {
        isPlayerInfoPanel = !isPlayerInfoPanel;
        
        playerInfoButton.SetActive(!isPlayerInfoPanel);
        homeButton.SetActive(isPlayerInfoPanel);

        questInfoButton.interactable = !isPlayerInfoPanel;
    }

    // �÷��̾ ������ �ӹ� ��ư
    public void PlayerQuestButton()
    {
        isPlayerQuestPanel = !isPlayerQuestPanel;
        mainMenuButton.interactable = !isPlayerQuestPanel;
    }
}
