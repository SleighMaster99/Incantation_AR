using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    public Transform cube;

    GameObject cp;
    float randomTime;
    
    // Start is called before the first frame update
    void Start()
    {
        randomTime = Random.Range(2.0f, 4.0f);

        cp = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        gameTime += Time.deltaTime;
        
        if(randomTime <= gameTime)
        {
            // �� �ȿ� ���� ����
            // �ִ�Ÿ� ���� �ּҰŸ� �̻� ����

            float d = (50.0f - 30.0f) * Random.value;
            Vector3 point = Random.onUnitSphere * (50.0f - d);

            Instantiate(cube, point, cp.transform.rotation);
            
            gameTime = 0;
            randomTime = Random.Range(2.0f, 4.0f);
        }
        */
    }
}
