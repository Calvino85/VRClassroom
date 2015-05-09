using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DistractionListener : MonoBehaviour {
	public Text output;
	public GameObject output2; // Assuming it has a Text mesh

	// Use this for initialization
	void OnEnable() {
		IsLookingAt.OnDistraction += HandleDistraction;
	}
	
	void OnDisable() {
		IsLookingAt.OnDistraction -= HandleDistraction;
	}
	
	void HandleDistraction (){
		showText ("Student is distracted!");
	}

	void showText( string t )
	{
		if( output != null )
			output.text = t;
		if (output2 != null)
			output2.GetComponent<TextMesh> ().text = t;
	}

}
