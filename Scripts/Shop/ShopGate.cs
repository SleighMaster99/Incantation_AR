using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopGate : MonoBehaviour
{
    public PlayerInformation pi;

    public Button buyRenewalButton;
    public Button buyMaxMentalityButton;
    public Button buyMentalityPotionButton;
    public Button buyRebornBallButton;

    public Text maxMentalityPrice;
    public Text playerCoinText;

    AudioSource audioPlayer;
    public AudioClip buySound;

    int maxMentalityBookPrice;

    // Start is called before the first frame update
    void Start()
    {
        maxMentalityBookPrice = (int)pi.playerBasicInfo.playerMaxMentality * 2;
        audioPlayer = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        maxMentalityBookPrice = (int)pi.playerBasicInfo.playerMaxMentality * 2;
        maxMentalityPrice.text = maxMentalityBookPrice.ToString() + "³É";

        if (pi.playerBasicInfo.playerCoin >= 2)
            buyRenewalButton.interactable = true;
        else
            buyRenewalButton.interactable = false;

        if (pi.playerBasicInfo.playerCoin >= maxMentalityBookPrice)
            buyMaxMentalityButton.interactable = true;
        else
            buyMaxMentalityButton.interactable = false;

        if (pi.playerBasicInfo.playerCoin >= 5)
            buyMentalityPotionButton.interactable = true;
        else
            buyMentalityPotionButton.interactable = false;

        if (pi.playerBasicInfo.playerCoin >= 5000)
            buyRebornBallButton.interactable = true;
        else
            buyRebornBallButton.interactable = false;

        playerCoinText.text = pi.playerBasicInfo.playerCoin.ToString();
    }

    public void BuyRenewalButton()
    {
        pi.playerBasicInfo.playerCoin -= 2;
        pi.playerItemCnt.questRenewal += 1;

        if (!audioPlayer.isPlaying)
            audioPlayer.PlayOneShot(buySound);
    }

    public void BuyMaxMentalityButton()
    {
        pi.playerBasicInfo.playerCoin -= maxMentalityBookPrice;
        pi.playerBasicInfo.playerMaxMentality += 100.0f;

        if (!audioPlayer.isPlaying)
            audioPlayer.PlayOneShot(buySound);
    }

    public void BuyMentalityPotionButton()
    {
        pi.playerBasicInfo.playerCoin -= 5;
        pi.playerItemCnt.mentalPotion += 10;

        if (!audioPlayer.isPlaying)
            audioPlayer.PlayOneShot(buySound);
    }

    public void BuyRebornBallButton()
    {
        pi.playerBasicInfo.playerCoin -= 5000;
        pi.playerItemCnt.rebornBall += 1;

        if (!audioPlayer.isPlaying)
            audioPlayer.PlayOneShot(buySound);
    }
}
