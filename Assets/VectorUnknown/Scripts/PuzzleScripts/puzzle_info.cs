using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class puzzle_info : MonoBehaviour {

	public Vector3 player_position; 		    //player's starting position

	public int attempt_count;
	public int display_upcoming_path;
	public int display_past_paths;
	public int game_mode;
    public LineRenderer future, past;

    public bool tutorial;

    public void Start(){
		Reset (); // initializes all data structures for base game

		UFO_PuzzleManager manager = GameObject.FindGameObjectWithTag ("Manager").GetComponent< UFO_PuzzleManager>();
		manager.set_models ( game_mode);

        if( this.tutorial == false)
            manager.debug_new_puzzle();
        if( this.tutorial == true)
            manager.set_tutorial();

        if (GetDisplayUpcomingPath() == 1)
            future.enabled = true;
        else
            future.enabled = false;

        if (GetDisplayPastPaths() == 1)
            past.enabled = true;
        else
            past.enabled = false;
    }

    GameObject[] FindGameObjectsWithName(string nameIt)
    {
        int it = 0;
        GameObject[] objArr;
        bool b = false;
        while (!b)
        {
            if (GameObject.Find(nameIt))
            {
                GameObject.Find(nameIt).name = nameIt + it;
                it++;
            }
            else
            {
                b = true;
            }
        }

        objArr = new GameObject[it];
        while (it > 0)
        {
            it--;
            objArr[it] = GameObject.Find(nameIt + it);
            objArr[it].name = nameIt;
        }

        return objArr;
    }//END FindGameObjectsWithName 

    public void Reset(){
		
		player_position = new Vector3 (0, 0, 0); //inital starting point of <0, 0, 0>
        GameObject [] level_data_array = FindGameObjectsWithName("LevelData");
        GameObject level_data = null;
        
        foreach (GameObject go in level_data_array)
        {
            level_data = go;          
        }
        
        
        
        //GameObject level_data = GameObject.Find ("LevelData");
		gui_select data = level_data.GetComponent< gui_select> ();

		attempt_count = data.attempt_count;
		display_upcoming_path = data.display_upcoming_path;
		display_past_paths = data.display_past_paths;
		game_mode = data.load_game_mode;
        tutorial = data.tutorial;

	}

	public bool game_over(){
		return (attempt_count == 0); 
	}

	public int GetDisplayUpcomingPath()
	{
		return display_upcoming_path;
	}

	public void SetDisplayUpcomingPath(int choice)
	{
		if (choice == 0 || choice == 1)
			display_upcoming_path = choice;
	}

    public int GetDisplayPastPaths()
    {
		return display_past_paths;
    }

    public void SetDisplayPastPaths(int choice)
    {
        if (choice == 0 || choice == 1)
			display_past_paths = choice;
    }

}
