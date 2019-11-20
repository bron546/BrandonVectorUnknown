using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class gui_game_layout : MonoBehaviour {

    public GameObject info;
    public bool infoShowing = false;

    public void play()
    {
        SceneManager.LoadScene("level_load_scene");
    }

    public void instruction()
    {
        if (infoShowing)
        {
            info.SetActive(false);
            infoShowing = false;
        }
        else
        {
            info.SetActive(true);
            infoShowing = true;
        }
    }

    public void exit()
    {
        Application.Quit();
    }
}
