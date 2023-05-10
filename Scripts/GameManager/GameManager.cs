using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject helpBattle;

    public Image movingImage;
    public Text helpBattleText;

    public PlayerInformation piScript;

    AudioSource audioPlayer;

    public AudioClip spawnSound;

    public static bool isSelectClass;


    public enum CurrentLocation
    {
        normalGround,
        basicBattleGround,
        middleBattleGround,
        bossBattleGround,
        heaven
    }

    public static CurrentLocation currentLocation;      // ���� �÷��̾� ��ġ

    public static bool isMovingLocation;                // ������ �����̴� ������
    bool isMovingLocation2;
    bool isDamage;

    float movingTime;                                   // ����Ʈ ���ý� �����̴� �ð�
    public static float moveRandomTime;                 // ������ �ð�

    private void Awake()
    {
        // �ڱ��� ��ħ�� ����
        //Input.location.Start();
        //Input.compass.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLocation = CurrentLocation.normalGround;
        isMovingLocation = false;
        isMovingLocation2 = false;
        isDamage = false;
        movingTime = 0;
        moveRandomTime = Random.Range(1.0f, 2.5f);

        audioPlayer = this.GetComponent<AudioSource>();

        isSelectClass = false;
    }

    // Update is called once per frame
    void Update()
    {
        MovingLocation();

        if(isDamage)
            LocationDamage();
        else
            piScript.playerBasicInfo.playerMentality += Time.deltaTime;
    }

    // ���� �̵��� UI �ִϸ��̼�
    void MovingLocation()
    {
        // ���� �帷 �ö�
        if (isMovingLocation)
        {
            movingImage.gameObject.SetActive(true);
            movingImage.fillAmount += Time.deltaTime;

            if (movingImage.fillAmount >= 1.0f)
                movingTime += Time.deltaTime;

            if (movingTime >= moveRandomTime)
            {
                isMovingLocation2 = true;
                isMovingLocation = false;
                movingTime = 0;
            }
        }
        
        // ���� �帷 ������
        if(isMovingLocation2)
        {
            if(movingImage.fillAmount > 0)
                movingImage.fillAmount -= Time.deltaTime;
            
            if(movingImage.fillAmount <= 0)
            {
                movingImage.gameObject.SetActive(false);
                movingImage.fillAmount = 0;
                isMovingLocation2 = false;
                if(currentLocation != CurrentLocation.normalGround)
                {
                    ShowHelpText1();
                    isDamage = true;
                }
                else
                    isDamage = false;
            }
        }
    }

    // ���� �ؽ�Ʈ ���̱�
    void ShowHelpText1()
    {
        helpBattleText.text = "���Ǹ� �ѷ����鼭 ���� ã���ÿ�";
        helpBattle.SetActive(true);
        Invoke("ShowHelpText2", 2.0f);
    }

    // ���� �ؽ�Ʈ ���̱�2
    void ShowHelpText2()
    {
        helpBattleText.text = "���� �߰��ϸ� �������� ���� 3�ʰ� �����Ͻÿ�";
        Invoke("HideHelpText", 2.0f);
    }

    // ���� �ؽ�Ʈ �����
    void HideHelpText()
    {
        helpBattle.SetActive(false);
    }

    // ��ġ�� ���� ���ŷ� ����
    void LocationDamage()
    {
        switch (currentLocation)
        {
            case CurrentLocation.basicBattleGround:
                piScript.playerBasicInfo.playerMentality -= Time.deltaTime;
                break;
            case CurrentLocation.middleBattleGround:
                piScript.playerBasicInfo.playerMentality -= 2.0f * Time.deltaTime;
                break;
            case CurrentLocation.bossBattleGround:
                piScript.playerBasicInfo.playerMentality -= 5.0f * Time.deltaTime;
                break;
        }
    }

    // ����Ʈ ���� ����
    public void PlaySpawnSound()
    {
        if (!audioPlayer.isPlaying)
            audioPlayer.PlayOneShot(spawnSound);
    }
}
