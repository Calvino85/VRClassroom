using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class SelectSpanish : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public bool selected;

	void Start(){
		selected = false;
	}
		
	//Do this when the cursor enters the rect area of this selectable UI object.
	public void OnPointerEnter (PointerEventData eventData) 
	{
		selected = true;
	}

	//Do this when the cursor enters the rect area of this selectable UI object.
	public void OnPointerExit (PointerEventData eventData) 
	{
		selected = false;
	}


}