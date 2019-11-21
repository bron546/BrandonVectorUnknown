using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public static GameObject item_dragged;
    public static Transform start_parent;
    public Vector3 start_position;
    public Transform Canvas;

    private static List<GameObject> blinkers = null;
    private static int blinkMode = 0;

    void Awake()
    {
        Canvas = GameObject.Find("Canvas").GetComponent<Transform>();
        if (blinkers == null)
        {
            blinkers = new List<GameObject>();
            GameObject[] blinks = GameObject.FindGameObjectsWithTag("Blinker");
            foreach (GameObject b in blinks)
            {
                blinkers.Add(b);
                if (b.GetComponent<Blinking>().onLeft)
                {
                    b.SetActive(true);
                }
                else
                {
                    b.SetActive(false);
                }
            }
        }
        else if(blinkMode != 0)
        {
            GameObject[] blinks = GameObject.FindGameObjectsWithTag("Blinker");
            foreach (GameObject b in blinks)
            {
                    b.SetActive(false);
            }
        }
    }

    #region IBeginDragHandler implementation

    public void OnBeginDrag(PointerEventData eventData)
    {
        item_dragged = gameObject;
        item_dragged.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        start_position = transform.position;
        start_parent = transform.parent;
        transform.SetParent(Canvas);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (blinkMode == 0)
        {
            foreach (GameObject b in blinkers)
            {
                if (b != null)
                {
                    if (b.GetComponent<Blinking>().onLeft)
                    {
                        b.SetActive(false);
                    }
                    else
                    {
                        b.SetActive(true);
                    }
                }
            }
            blinkMode++;
        }
    }

    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        Image drop = transform.GetComponent<Image>();
        float height = drop.sprite.rect.height / 1.25f;
        float width = drop.sprite.rect.width / 1.25f;
        transform.Translate(new Vector3(height / 8, width / 8, 0.0f));
    }

    #endregion

    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        item_dragged.transform.localScale = new Vector3(1f, 1f, 1f);
        item_dragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == Canvas)
        {
            transform.SetParent(start_parent);
            transform.position = start_position;
        }
        else
        {
            GameObject[] droppers = GameObject.FindGameObjectsWithTag("Dropper");
            foreach (GameObject drop in droppers)
            {
                if (drop.transform.childCount != 0 && drop.GetComponent<Drop>().formula)
                {
                    drop.GetComponent<Drop>().Constant.GetComponent<constant_counter>().testStillFit();
                }
            }
        }
        if (blinkMode == 1)
        {
            foreach (GameObject b in blinkers)
            {
                b.SetActive(false);
            }
            blinkMode++;
        }
    }


    #endregion

}
