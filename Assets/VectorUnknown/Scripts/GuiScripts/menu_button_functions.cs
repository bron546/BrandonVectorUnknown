using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class menu_button_functions : MonoBehaviour
{
    public TextMeshProUGUI Title_Banner;

    public void exit()
    {
        Application.Quit();
    }

    public void set_title_banner( string title)
    {
        Title_Banner.text = title;
    }
}
