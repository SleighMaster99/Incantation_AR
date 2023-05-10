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

    public static CurrentLocation currentLocation;      // 현재 플레이어 위치

    public static bool isMovingLocation;                // 공간을 움직이는 중인지
    bool isMovingLocation2;
    bool isDamage;

    float movingTime;                                   // 게이트 선택시 움직이는 시간
    public static float moveRandomTime;                 // 움직일 시간

    private void Awake()
    {
        // 자기장 나침반 실행
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

    // 공간 이동시 UI 애니메이션
    void MovingLocation()
    {
        // 검은 장막 올라감
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
        
        // 검은 장막 내려감
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

    // 도움말 텍스트 보이기
    void ShowHelpText1()
    {
        helpBattleText.text = "주의를 둘러보면서 적을 찾으시오";
        helpBattle.SetActive(true);
        Invoke("ShowHelpText2", 2.0f);
    }

    // 도움말 텍스트 보이기2
    void ShowHelpText2()
    {
        helpBattleText.text = "령을 발견하면 움직이지 말고 3초간 응시하시오";
        Invoke("HideHelpText", 2.0f);
    }

    // 도움말 텍스트 숨기기
    void HideHelpText()
    {
        helpBattle.SetActive(false);
    }

    // 위치에 따른 정신력 감소
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

    // 게이트 스폰 사운드
    public void PlaySpawnSound()
    {
        if (!audioPlayer.isPlaying)
            audioPlayer.PlayOneShot(spawnSound);
    }
}
