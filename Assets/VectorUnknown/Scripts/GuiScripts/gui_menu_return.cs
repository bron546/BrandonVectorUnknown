using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class gui_menu_return : MonoBehaviour
{
    [SerializeField]
    private Transform menu_buttons, level_buttons;
    [SerializeField]
    private TextMeshProUGUI TitleBanner;
    void Start()
    {

        if( GameConstants.menu_level_select == true)
        {//jump to level select screen if constant value is true
            menu_buttons.gameObject.SetActive(false);
            level_buttons.gameObject.SetActive(true);
            TitleBanner.text = "Level Select";
        }

        if (GameConstants.menu_level_select == false)
        {//jump to main menu scene if constant value is false
            menu_buttons.gameObject.SetActive(true);
            level_buttons.gameObject.SetActive(false);
            TitleBanner.text = "Vector Unknown";
        }
    }
}
