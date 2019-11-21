using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHolder : MonoBehaviour, IDropHandler {

	public Drop first_coefficient;
	public Drop second_coefficient;
	public bool first_toggle = true;

	#region IDropHandler implementation

	public void OnDrop (PointerEventData eventData)
	{
		if ( !first_coefficient.has_item()) {
			first_coefficient.start_parent = Drag.start_parent;
			Drag.item_dragged.transform.SetParent ( first_coefficient.transform);
		} else if( !second_coefficient.has_item()){
			second_coefficient.start_parent = Drag.start_parent;
			Drag.item_dragged.transform.SetParent ( second_coefficient.transform);
		}else {
			Transform new_parent = Drag.start_parent;

			if (first_toggle == true) {
				Drag.item_dragged.transform.SetParent (first_coefficient.transform);
				first_coefficient.item.transform.SetParent(new_parent);
				first_toggle = false;
			} else if (first_toggle == false) {
				Drag.item_dragged.transform.SetParent (second_coefficient.transform);
				second_coefficient.item.transform.SetParent(new_parent);
				first_toggle = true;
			}
		}
	}

	#endregion
}
