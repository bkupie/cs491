using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{

    public Light lt;
    void Start()
    {
        lt = GetComponent<Light>();
    }
    void Update()
    {

        if (Input.GetButton("Fire1") && lt.intensity < 1.8)
        {
            lt.intensity+= 0.1f;
        }
        else if (Input.GetButton("Fire2") && lt.intensity > 0)
        {
            lt.intensity -= 0.1f;
        }


   
    }
}