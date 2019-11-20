using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockTextToggle : MonoBehaviour
{
    public Text text;

    // Use this for initialization
    void Start()
    {
        text.text = "Grid Locked";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void toggleGridLock()
    {
        Debug.Log("Help!");
        if (GameConstants.locked)
        {
            
            text.text = "Grid Unlocked";
            GameConstants.locked = false;
        }
        else
        {
            text.text = "Grid Locked";
            GameConstants.locked = true;
        }
    }
}
