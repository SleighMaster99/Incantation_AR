using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushID : MonoBehaviour
{
    public GameObject brush;
    public GameObject scrollButton;
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
            brush.SetActive(true);
            scrollButton.SetActive(true);
            GameManager.isSelectClass = true;
        }
    }

    public void CharmIDTracked()
    {
        isTracked = true;
        startText.SetActive(false);
        pi.playerBasicInfo.playerClass = "¹¬ È­ ¼ú »ç";
    }
}
