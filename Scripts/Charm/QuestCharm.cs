using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestCharm : MonoBehaviour
{
    public GameObject questGate;
    public GameObject questGateSpawnPoint;

    public Image charmTreeColorImage;

    bool isGate;
    bool isTracked;

    // Start is called before the first frame update
    void Start()
    {
        isGate = false;
        isTracked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTracked && !isGate)
        {
            charmTreeColorImage.fillAmount += Time.deltaTime;

            if(charmTreeColorImage.fillAmount >= 1.0f)
            {
                isGate = !isGate;

                questGate.transform.position = questGateSpawnPoint.transform.position;
                questGate.transform.rotation = questGateSpawnPoint.transform.rotation;

                GameObject.Find("GameManager").GetComponent<GameManager>().PlaySpawnSound();

                questGate.SetActive(isGate);

                isTracked = false;
            }
        }
        else if(isTracked && isGate)
        {
            charmTreeColorImage.fillAmount -= Time.deltaTime;

            if(charmTreeColorImage.fillAmount <= 0)
            {
                isGate = !isGate;
                questGate.SetActive(isGate);

                isTracked = false;
            }
        }
    }

    public void QuestCharmTracked()
    {
        isTracked = true;
    }
}
