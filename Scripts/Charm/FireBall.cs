using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    float destroyTime;
    private void Start()
    {
        destroyTime = 0;
    }
    private void Update()
    {
        destroyTime += Time.deltaTime;
        if (destroyTime >= 20.0f)
            Destroy(this.gameObject);
    }
    // 충돌
    // BasicGhost 이면 50 데미지 줌
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BasicGhost"))
        {
            other.GetComponent<BasicGhost>().hp -= 50.0f;
            Destroy(this.gameObject);
        }   
        else if (other.gameObject.CompareTag("MiddleGhost"))
        {
            other.GetComponent<MiddleGhost>().hp -= 50.0f;
            Destroy(this.gameObject);
        } 
        else if (other.gameObject.CompareTag("BossGhost"))
        {
            other.GetComponent<MiddleGhost>().hp -= 50.0f;
            Destroy(this.gameObject);
        }
    }
}
