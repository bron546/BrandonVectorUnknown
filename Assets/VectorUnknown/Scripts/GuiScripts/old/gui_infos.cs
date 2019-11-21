using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gui_infos : MonoBehaviour {

	public GUISkin style_skin;
    public Texture2D warning_background, warning_close_button;
    public TextAsset info_gameinstructions;

    public GameObject ObjectOfCanvasGroup;
    private CanvasGroup infosGroup;

    private int fontSize;

	void Start(){
        warning_background = Resources.Load("gui4/png/object/16", typeof(Texture2D)) as Texture2D;
        warning_close_button = Resources.Load("gui4/png/button/512/34", typeof(Texture2D)) as Texture2D;

        //infosGroup = ObjectOfCanvasGroup.GetComponent<CanvasGroup>();

        infosGroup = GetComponent<CanvasGroup>();

        infosGroup.alpha = 0.5f;

        fontSize = 11;
	}

	void OnGUI( ){

        // INFO OVERLAY (IS ACTIVATED BY INFO BUTTON)

        GUI.BeginGroup(AspectUtility.screenRect);

        GUI.skin.box.wordWrap = true;
        GUI.skin.box.normal.background = warning_background;
        GUI.skin.box.hover.background = warning_background;
        GUI.skin.box.normal.textColor = Color.white;
        GUI.skin.box.hover.textColor = Color.white;
        GUI.skin.box.fontSize = fontSize;
        GUI.skin.box.alignment = TextAnchor.UpperLeft;

        RectOffset rectOff = new RectOffset(50, 40, 40, 40);
        GUI.skin.box.padding = rectOff;

        //CanvasGroup cvg = new CanvasGroup();

        GUI.Box(new Rect(
            AspectUtility.screenWidth * (2f / 12f),
            AspectUtility.screenHeight * (2f / 12f),
            AspectUtility.screenWidth * (8f / 12f),
            AspectUtility.screenHeight * (8f / 12f)), ReadTextAsset());
        

        // CLOSE INFO BUTTON

        Rect loc = new Rect(
            AspectUtility.screenWidth * (9f / 12f),
            AspectUtility.screenHeight * (8f / 12f),
            AspectUtility.screenWidth * (1f / 24f),
            AspectUtility.screenHeight * (1f / 12f)
        );

        GUI.skin = style_skin;

        GUI.skin.button.normal.background = warning_close_button;
        GUI.skin.button.hover.background = warning_close_button;
        GUI.skin.button.border.left = 0;
        GUI.skin.button.border.right = 0;
        GUI.skin.button.border.top = 0;
        GUI.skin.button.border.bottom = 0;

        if (GUI.Button(loc, ""))
        {
            infosGroup.alpha = 0f;
            infosGroup.interactable = false;
            infosGroup.blocksRaycasts = false;

            ObjectOfCanvasGroup.SetActive(false);
        }

        GUI.EndGroup();
    }

    string ReadTextAsset()
    {
        string text = info_gameinstructions.text;
        return text;
    }


}


