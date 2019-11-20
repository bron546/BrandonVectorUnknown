using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scoredisplay : MonoBehaviour
{

    public List<Transform> level_buttons = new List<Transform>();

    void Start()
    {
        for( int i = 0; i < level_buttons.Count; i++)
            set_bests(level_buttons[i], (i + 1));

    }

    private void set_bests( Transform lvlbtn, int i)
    {//each button has three Text component children, one for level, one for best time and one for best stars
        string besttime = string.Format("besttime{0}", i.ToString()), beststars = string.Format("beststars{0}", i.ToString());
        if(PlayerPrefs.HasKey( besttime))
        {//check if a best time is logged for this level
            int time = PlayerPrefs.GetInt(besttime);
            Text best_time_text = lvlbtn.GetChild(1).GetComponent< Text>();
            best_time_text.text = rep_time(time);
        }

        if (PlayerPrefs.HasKey(beststars))
        {//check if a best star count is logged for this level
            int stars = PlayerPrefs.GetInt(beststars);
            Text best_stars_text = lvlbtn.GetChild(2).GetComponent<Text>();
            best_stars_text.text = rep_star(stars);
        }
    }

    private string rep_time( int seconds)
    {//represents time in --:-- format
        return string.Format( "{0} : {1}", 
            (( int)(seconds / 60)).ToString(), 
            (seconds % 60 > 9 ? (seconds % 60).ToString() : string.Format( "0{0}", seconds % 60)));
    }

    private string rep_star( int stars)
    {//represents stars as ★☆☆, ★★☆, or ★★★

        if ( stars == 1)
            return "*--";
        if (stars == 2)
            return "**-";
        if (stars == 3)
            return "***";

        return "";
    }
}
