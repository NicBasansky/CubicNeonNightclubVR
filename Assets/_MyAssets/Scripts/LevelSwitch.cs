using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LevelSwitch : MonoBehaviour
{
    public static int currentLevel = 0;

    public string[] levelNames = new string[2] { "Outside", "Club" };

    static LevelSwitch s = null;

    
    void Start()
    {
        if (s == null)
        {
            s = this;
        }    
        else
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLevel = (currentLevel + 1) % 2; // next level
            SteamVR_LoadLevel.Begin(levelNames[currentLevel], false, 1.0f, 1.0f);
        }
    }
}
