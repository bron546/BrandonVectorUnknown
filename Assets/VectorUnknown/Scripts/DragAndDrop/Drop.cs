using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler {
	public GameObject item { 
		get { 
			if (transform.childCount > 0) {
				return transform.GetChild (0).gameObject;
			}
			return null;
		}
	}

    public GameObject Constant;
    public bool formula;

	public Transform start_parent;
	public bool has_item(){
		return ( transform.childCount > 0);
	}

	#region IDropHandler implementation
	public void OnDrop (PointerEventData eventData)
	{
		if (!item) {
			Drag.item_dragged.transform.SetParent (transform);
		} else {
			Transform new_parent = Drag.start_parent;

			item.transform.SetParent (new_parent);
			Drag.item_dragged.transform.SetParent (transform);
            Constant.GetComponent<constant_counter>().reset();
		}
	}
	#endregion
}
