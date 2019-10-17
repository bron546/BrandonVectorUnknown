using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class load_grid_numbers_canvas : MonoBehaviour {

	public GameObject number;
	public GameObject line;
    

	private GameObject camera_reflect;
	private List< GameObject> numbers;
	 
	void Start () {
		numbers = new List< GameObject> ();
       
		camera_reflect = GameObject.FindGameObjectWithTag ("Reflect");

		for (int i = 1; i < 11; i++) {
			Vector3[] points = new Vector3[4];
			points[0] = new Vector3 ( i * 2.0f, 0, 0);   	// positive x values
			gen_number( points[0]);
			gen_line (points [0]);

			points[1] = new Vector3 ( -i * 2.0f, 0, 0);  	// negative x values
			gen_number( points[1]);
			gen_line( points[1]);

			points[2] = new Vector3 ( 0, 0, i * 2.0f); 		// positive y values
			gen_number( points[2]);
			gen_line( points[2]);

			points[3] = new Vector3 ( 0, 0, -i * 2.0f);		// negative y-values
			gen_number( points[3]);
			gen_line( points[3]);

		}
			

		//transform.position = new Vector3( transform.position.x, transform.position.y -3.0f, transform.position.z);
	}

	private void gen_line( Vector3 point){
		GameObject load_line = Instantiate (line, point, Quaternion.identity, transform);
		if (point.z != 0)
			load_line.transform.Rotate ( new Vector3( 0, 0, 90f));
		if (point.x != 0)
			load_line.transform.Rotate ( new Vector3( 90f, 0, 0));
	}

	private void gen_number( Vector3 point){
		GameObject load_number = Instantiate ( number, point, Quaternion.identity, transform);
        //load_number.transform.GetChild(0).GetComponent<Text>().font = mFont;
        load_number.transform.GetChild(0).GetComponent<Text>().fontSize = 16;
        load_number.transform.GetChild(0).GetComponent< Text>().text = point.x != 0.0f ? point.x.ToString() : "  "+point.z.ToString();
		load_number.transform.localPosition = point;

		numbers.Add (load_number);
	}

	void Update () {
		foreach (GameObject number in numbers) {
			number.transform.LookAt ( camera_reflect.transform);
		}
	}
}
