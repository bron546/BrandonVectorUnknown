using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gui_game : MonoBehaviour {

	public GUISkin style_skin;
	public Texture2D home_button, info_button, bar;

    public GameObject ObjectOfCanvasGroup;
    private CanvasGroup infosGroup;

	void Start(){
		home_button = Resources.Load ("gui4/png/button/512/93", typeof(Texture2D)) as Texture2D;
        info_button = Resources.Load("gui4/png/button/512/22", typeof(Texture2D)) as Texture2D;
        bar = Resources.Load ("gui4/png/object/16", typeof(Texture2D)) as Texture2D;


        ObjectOfCanvasGroup.SetActive(false);
        //infosGroup = ObjectOfCanvasGroup.GetComponent<CanvasGroup>();
        //infosGroup.alpha = 0f;
        //infosGroup.interactable = false;
        //infosGroup.blocksRaycasts = false;


        //fontSize = 11;
	}

	void OnGUI( ){


        // MAIN MENU HOME BUTTON

        GUI.BeginGroup(AspectUtility.screenRect);

        Rect location = new Rect(
            AspectUtility.screenWidth * (11f / 12f),
            AspectUtility.screenHeight * (10f / 12f),
            AspectUtility.screenWidth * (1f / 24f),
            AspectUtility.screenHeight * (1f / 12f)
        );

        GUI.skin = style_skin;

		GUI.skin.button.normal.background = home_button;
		GUI.skin.button.hover.background = home_button;
		GUI.skin.button.border.left = 0;
		GUI.skin.button.border.right = 0;
		GUI.skin.button.border.top = 0;
		GUI.skin.button.border.bottom = 0;

		if( GUI.Button( location, "")){
			SceneManager.LoadScene ("menu_scene");
		}

        GUI.EndGroup();

        // INFO BUTTON (MAKES INFO OVERLAY VISIBLE)
        GUI.BeginGroup(AspectUtility.screenRect);

        Rect infoLocation = new Rect(
            AspectUtility.screenWidth * (10f / 12f),
            AspectUtility.screenHeight * (10f / 12f),
            AspectUtility.screenWidth * (1f / 24f),
            AspectUtility.screenHeight * (1f / 12f)
        );

        GUI.skin.button.normal.background = info_button;
        GUI.skin.button.hover.background = info_button;
        GUI.skin.button.border.left = 0;
        GUI.skin.button.border.right = 0;
        GUI.skin.button.border.top = 0;
        GUI.skin.button.border.bottom = 0;
        
        if (GUI.Button(infoLocation, ""))
        {
            ObjectOfCanvasGroup.SetActive(true);
            //infosGroup.alpha = 1f;
            //infosGroup.interactable = true;
            //infosGroup.blocksRaycasts = true;
        }

        GUI.EndGroup();
    }


}


