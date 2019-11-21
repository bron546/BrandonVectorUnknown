using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/* Author : Nate Cortes
 * Holds a choice_vector and implements draggable interface. Made for CPI441 capstone. 
 */

public class choice_holder : MonoBehaviour{

	[SerializeField]
	public Vector2 choice;
	private Text text;

	// Use this for initialization
	void Awake() {
		choice = new Vector2 (0, 0);
		text = transform.GetChild(0).GetComponent< Text>();
		text.text = choice.x.ToString ("F0") + "\n" + choice.y.ToString("F0");
	}
		
	public void update_choice( Vector2 ch){
		choice = ch;
		text.text = choice.x.ToString ("F0") + "\n" + choice.y.ToString("F0");
	}
}

