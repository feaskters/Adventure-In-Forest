using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
public class globalLightController : MonoBehaviour
{
    bool isLight = false;
    Light2D light2d;
    // Start is called before the first frame update
    void Start()
    {
        light2d = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLight && light2d.intensity < 1)
        {
            light2d.intensity = Mathf.Clamp01(light2d.intensity + Time.deltaTime);
            if (light2d.intensity == 1)
            {
                isLight = false;
            }
        }
        if (!isLight && light2d.intensity > 0)
        {
            light2d.intensity = Mathf.Clamp01(light2d.intensity - Time.deltaTime);
        }
    }

    public void starGet(){
        isLight = true;
    }
}
