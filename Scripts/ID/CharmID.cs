using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmID : MonoBehaviour
{
    public GameObject fireCharm;
    public GameObject questCharm;
    public GameObject battleGateCharm;
    public GameObject ShieldCharm;
    public GameObject ShopCharm;

    public GameObject startText;

    public PlayerInformation pi;

    bool isTracked;

    private void Start()
    {
        isTracked = false;
    }

    private void Update()
    {
        if (isTracked && !GameManager.isSelectClass)
        {
            fireCharm.SetActive(true);
            questCharm.SetActive(true);
            battleGateCharm.SetActive(true);
            ShieldCharm.SetActive(true);
            ShopCharm.SetActive(true);
            GameManager.isSelectClass = true;
        }
    }

    public void CharmIDTracked()
    {
        isTracked = true;
        startText.SetActive(false);
        pi.playerBasicInfo.playerClass = "부 적 술 사";
    }
}
