using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldCharm : MonoBehaviour
{
    public GameObject shieldImage;
    public GameObject bubbleParticle;
    public GameObject shieldCollider;
    public Image shieldCharmImage;

    Color newColor;

    bool isTracked;

    // Start is called before the first frame update
    void Start()
    {
        isTracked = false;
        newColor = shieldCharmImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTracked)
        {
            if (newColor.a >= 0)
                newColor.a -= 0.3f * Time.deltaTime;
            else
                newColor.a = 0;

            shieldCharmImage.color = newColor;

            if(shieldCharmImage.color.a > 0)
            {
                shieldImage.SetActive(true);
                bubbleParticle.SetActive(true);
                shieldCollider.SetActive(true);
            }
            else
            {
                shieldImage.SetActive(false);
                bubbleParticle.SetActive(false);
                shieldCollider.SetActive(false);
            }
        }
        else
        {
            if (newColor.a <= 1)
                newColor.a += 0.3f * Time.deltaTime;
            else
                newColor.a = 1;
        }
    }

    public void ShieldCharmTracked()
    {
        isTracked = true;
    }

    public void ShieldCharmLost()
    {
        shieldImage.SetActive(false);
        bubbleParticle.SetActive(false);
        shieldCollider.SetActive(false);
        isTracked = false;
    }
}
