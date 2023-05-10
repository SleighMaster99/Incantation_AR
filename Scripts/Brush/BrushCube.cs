using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class BrushCube : MonoBehaviour
{
    public GameObject paintOrb;
    public GameObject paint;

    RaycastHit hit;

    bool isTracked;

    private void Start()
    {
        isTracked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTracked)
        {
            RayTracingFunction();
            Debug.DrawRay(paintOrb.transform.position, -this.transform.up, Color.red, Mathf.Infinity);
        }
        
        if (Input.GetMouseButton(2))
        {
            Instantiate(paint, paintOrb.transform.position, paintOrb.transform.rotation);
        }
        
    }


    // Ray
    void RayTracingFunction()
    {
        Ray ray = new Ray(paintOrb.transform.position, -this.transform.up);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.CompareTag("Scroll"))
            {
                Instantiate(paint, hit.point, this.transform.rotation);
            }
        }
    }

    public void BrushCubeTracked()
    {
        isTracked = true;
        paintOrb.SetActive(true);
    }

    public void BrushCubeLost()
    {
        isTracked = false;
        paintOrb.SetActive(false);
    }
}
