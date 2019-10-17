using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinking : MonoBehaviour
{
    public bool onLeft;

    private Image im;
    private Color gray = Color.gray;
    private Color yellow = Color.yellow;

    private const float DELAY = 1.0f;

    // Use this for initialization
    void Start()
    {
        im = GetComponent<Image>();
        im.color = yellow;
        StartCoroutine(delayGray());
    }

    private IEnumerator delayGray()
    {
        yield return new WaitForSeconds(DELAY);
        im.color = gray;
        StartCoroutine(delayYellow());
    }

    private IEnumerator delayYellow()
    {
        yield return new WaitForSeconds(DELAY);
        im.color = yellow;
        StartCoroutine(delayGray());
    }
}
