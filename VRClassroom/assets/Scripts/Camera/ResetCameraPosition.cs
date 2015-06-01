using UnityEngine;
using System.Collections;

public class ResetCameraPosition : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.R)) {
			OVRDevice.ResetOrientation ();
		}
	}
}
