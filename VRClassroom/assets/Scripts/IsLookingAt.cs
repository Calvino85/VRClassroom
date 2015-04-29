using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IsLookingAt : MonoBehaviour {
	public GameObject origin;
	public Text output;
	public GameObject output2; // Assuming it has a Text mesh
	GameObject lastSelected;
	bool targetWasHit;
	float lastTime;

	// Use this for initialization
	void Start () {
		lastSelected = null;
		targetWasHit =  false; 
		lastTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
		RaycastHit hit;
//		var cameraCenter = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, camera.nearClipPlane));
		//		if (Physics.Raycast(cameraCenter, this.transform.forward, out hit, 1000))
		var colliderHit = Physics.Raycast (origin.transform.position, origin.transform.forward, out hit, Mathf.Infinity);
		Debug.DrawRay(origin.transform.position, origin.transform.forward, Color.green);
		if (colliderHit && !targetWasHit && Time.time - lastTime > 1.0) {
			targetWasHit =  true;
			lastTime = Time.time;
			var obj = hit.transform.gameObject;
			if (lastSelected != obj) {
				deSelect( );					
				select( obj );
			}
		} else if (!colliderHit && targetWasHit) {
			targetWasHit =  false;
			deSelect( );	
		}
	}

	void deSelect( ) 
	{
		if (lastSelected != null) {
//			lastSelected.transform.Rotate (new Vector3 (0, 92, 0));
//			lastSelected.SetActive (true);
		}
		lastSelected = null;
		showText( "Nothing" );
	}

	void select( GameObject obj ) 
	{
		lastSelected = obj;
		showText ( obj.name != null? obj.name : "Nothing" );
//		lastSelected.transform.Rotate (new Vector3 (0, -92, 0));
//		lastSelected.SetActive (false);
	}

	void showText( string t )
	{
		if( output != null )
			output.text = t;
		if (output2 != null)
			output2.GetComponent<TextMesh> ().text = t;
	}
	
}
