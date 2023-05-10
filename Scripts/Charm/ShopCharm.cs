using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCharm : MonoBehaviour
{
    public GameObject shopGate;
    public GameObject shopGateParticle;

    float measuredTime;

    bool isTracked;
    bool isGate;

    // Start is called before the first frame update
    void Start()
    {
        measuredTime = 0;

        isTracked = false;
        isGate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTracked && isGate)
        {
            measuredTime += Time.deltaTime;

            if(measuredTime >= 3.0f)
            {
                shopGateParticle.SetActive(false);
                isGate = false;
            }
        }
    }

    public void ShopCharmTracked()
    {
        isTracked = !isTracked;
        isGate = true;
        shopGate.SetActive(isTracked);
        measuredTime = 0;
        shopGateParticle.SetActive(true);
        GameObject.Find("GameManager").GetComponent<GameManager>().PlaySpawnSound();
    }
}
