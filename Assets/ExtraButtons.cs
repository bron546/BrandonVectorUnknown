using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraButtons : MonoBehaviour
{
    public List<GameObject> buttons;

    private bool showing = false;

    // Use this for initialization
    void Start()
    {
        foreach(GameObject button in buttons)
        {
            button.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void toggleButtons()
    {
        showing = !showing;
        foreach (GameObject button in buttons)
        {
            button.SetActive(showing);
        }
    }
}
