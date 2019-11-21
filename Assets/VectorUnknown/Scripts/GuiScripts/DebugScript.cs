using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour {
	public GameObject puzzle_manager;

	private puzzle_info puzzle;
	private UFO_PuzzleManager manager;
	private Text title;
	private Text elapsed_time;

	void Start () {
		puzzle = puzzle_manager.GetComponent< puzzle_info> ();
		manager = puzzle_manager.GetComponent< UFO_PuzzleManager> ();

		title = transform.GetChild (0).GetComponent< Text>();
		title.text = "Debug Panel\n";

		elapsed_time = transform.GetChild (1).GetComponent< Text>();

	}

	void Update(){

		string time = Time.timeSinceLevelLoad.ToString ("0.00");
		elapsed_time.text = "Level Time:\n" + time;
	}

	public void future_sight_toggle(){
		int fs = puzzle.GetDisplayUpcomingPath ();

		if (fs == 0)
			puzzle.SetDisplayUpcomingPath (1);
		else
			puzzle.SetDisplayUpcomingPath (0);
	}

	public void past_render_toggle(){
		int pr = puzzle.GetDisplayPastPaths ();

		if (pr == 0)
			puzzle.SetDisplayPastPaths(1);
		else
			puzzle.SetDisplayPastPaths(0);
	}

	public void reset_attempts(){
		manager.number_of_attempts = 0;
	}
}
