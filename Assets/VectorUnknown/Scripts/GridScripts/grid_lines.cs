using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid_lines : MonoBehaviour {

	public float line_width = 1.0f;
	public float width = 20f, height = 20f;

	public Vector3 top, bot;
	public Material line_material;
	public Vector3[] lines;
	//public Vector3[] lines_start, lines_end;
	//public List< GameObject> lines;

	void Start () {
		GameObject grid_lines = new GameObject ();
		grid_lines.name = "grid_lines";

		top = new Vector3( 0, 0, width);
		bot = new Vector3( 0, 0, -width);

		Vector3 offset = (Vector3.left * width);
		Vector3 bias = Vector3.up * 0.05f;

		line_width = 0.15f;
		lines = new Vector3[81];
		lines [0] = bot - offset + bias;


		/*lines [1] = lines[ ( 1 - 1)] + (Vector3.forward * 2 * width);
		lines [(1 + 1)] = lines [1] + Vector3.left;
		lines [(1 + 2)] = lines [(1 + 1)] + (Vector3.back * 2 * width);
		lines [(1 + 3)] = lines [(1 + 2)] + Vector3.left;*/

		for (int i = 1; i < 80; i = i + 4) {
			lines [i] = lines[ ( i - 1)] + (Vector3.forward * 2 * width);
			lines [(i + 1)] = lines [i] + Vector3.left;
			lines [(i + 2)] = lines [(i + 1)] + (Vector3.back * 2 * width);
			lines [(i + 3)] = lines [(i + 2)] + Vector3.left;
		}

		lines [80] = lines [79] + Vector3.left;

		LineRenderer line_render = grid_lines.AddComponent< LineRenderer> ( ) as LineRenderer;

		line_render.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
		line_render.positionCount = (int)width * 4;
		line_render.shadowCastingMode = 0;
		line_render.startWidth = line_width;
		line_render.endWidth = line_width;
		line_render.material = line_material;

		line_render.SetPositions (lines);

		/*lines_start = new Vector3[(int)width + 1];
		lines_end = new Vector3[ (int)width + 1];


		for (int i = 0; i < width; i++){
			lines_start[ i] = new Vector3( -width/2  , 0, i -width/2);
			lines_end[i] = new Vector3( width/2f, 0, i -width/2);
			lines.Add( gen_line_render( lines_start[i], lines_end[i]));
		//}

		//for (int i = 0; i < 11; i++){
			lines_start[ i] = new Vector3( i-width/2f, 0,  -width/2f);
			lines_end[i] = new Vector3( i-width/2f, 0, width/2f);
			lines.Add( gen_line_render( lines_start[i], lines_end[i]));
		}

		foreach (GameObject go in lines)
			go.transform.SetParent (grid_lines.transform); */
	}
		
	void Update () {

	}

	/*GameObject gen_line_render( Vector3 start, Vector3 end){
		GameObject line_to_render = new GameObject ();
		line_to_render.name = "Grid Line";
		//line_to_render.layer = 9;

		LineRenderer line_render = line_to_render.AddComponent< LineRenderer> ( ) as LineRenderer;

		line_render.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
		line_render.shadowCastingMode = 0;
		line_render.startWidth = line_width;
		line_render.endWidth = line_width;
		line_render.SetPosition (0, start);
		line_render.SetPosition( 1, end);

		return line_to_render;
	}*/

}
