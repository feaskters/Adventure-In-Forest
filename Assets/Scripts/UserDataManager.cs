using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("level", int.Parse(this.name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
