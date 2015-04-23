using UnityEngine;
using System.Collections;

public class IsLookitAt : MonoBehaviour {
	public GameObject origin;
	public GameObject lastSelected;

	// Use this for initialization
	void Start () {
		lastSelected = null;
	}

	// Update is called once per frame
	void Update () {
		RaycastHit hit;
//		var cameraCenter = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, camera.nearClipPlane));
		//		if (Physics.Raycast(cameraCenter, this.transform.forward, out hit, 1000))
		if (Physics.Raycast(origin.transform.position,origin.transform.forward, out hit))
		{
			var obj = hit.transform.gameObject;
			if( lastSelected != obj )
			{
				if( lastSelected != null )
					lastSelected.SetActive(true);
				lastSelected = obj;
				lastSelected.SetActive(false);
			}
		}
	}
}
