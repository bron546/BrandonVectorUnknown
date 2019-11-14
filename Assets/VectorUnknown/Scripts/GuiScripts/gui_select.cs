using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/* Author : Nate Cortes
 * Load Level Methods. Made for CPI441 capstone. 
 */

public class gui_select : MonoBehaviour {

	public int attempt_count = 0;
	public int display_upcoming_path = 0;
	public int display_past_paths = 0;
	public int load_game_mode = 0;
    public bool tutorial = false;
    [Range(1, 5)]
    public int level_number;

	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
	}

	public void load_game(){
		SceneManager.LoadScene ("VectorGame");
	}

	public void level_one(){
        Psychometrics.logEvent("L1");
        puzzle_settings (-1, 1, 1, 0);
        PlayerPrefs.SetInt("CurrentLevel", 1);
		SceneManager.LoadScene ("VectorGame");
	}

	public void level_two(){
        Psychometrics.logEvent("L2");
		puzzle_settings (-1, 0, 1, 0);
        PlayerPrefs.SetInt("CurrentLevel", 2);
        SceneManager.LoadScene ("VectorGame");
	}

	public void level_three(){
        Psychometrics.logEvent("L3");
        puzzle_settings ( 5, 1, 1, 0);
        PlayerPrefs.SetInt("CurrentLevel", 3);
        SceneManager.LoadScene ("VectorGame");
	}

	public void level_four(){
        Psychometrics.logEvent("L4");
        puzzle_settings ( 5, 0, 0, 0);
        PlayerPrefs.SetInt("CurrentLevel", 4);
        SceneManager.LoadScene ("VectorGame");
	}

	public void level_five(){
        Psychometrics.logEvent("L5");
        puzzle_settings ( -1, 1, 1, 1);
        PlayerPrefs.SetInt("CurrentLevel", 5);
        SceneManager.LoadScene ("VectorGame");

        Debug.Log("Button 5 Pressed");
	}
    public void level_six()
    {
        Psychometrics.logEvent("L6");
        puzzle_settings(-1, 1, 1, 2);
        PlayerPrefs.SetInt("CurrentLevel", 6);
        SceneManager.LoadScene("VectorGame");

        Debug.Log("Button 6 Pressed");
    }
    public void level_seven()
    {
        Psychometrics.logEvent("L7");
        puzzle_settings(-1, 1, 1, 3);
        PlayerPrefs.SetInt("CurrentLevel", 7);
        SceneManager.LoadScene("VectorGame");

        Debug.Log("Button 7 Pressed");
    }

    public void level_tutorial()
    {
        Psychometrics.logEvent("LT");
        puzzle_settings(-1, 1, 1, 3);
        set_tutorial();
        PlayerPrefs.SetInt("CurrentLevel", 0);
        SceneManager.LoadScene("tutorial");
    }

	/***************************************/
	/* Helper Method, sets values of Puzzle*/
	/***************************************/
	private void puzzle_settings( int attempts, int future_paths, int previous_paths, int game_mode){
		attempt_count = attempts;
		display_upcoming_path = future_paths;
		display_past_paths = previous_paths;
		load_game_mode = game_mode;
	}

    private void set_tutorial()
    {
        this.tutorial = true;
    }

}
