using UnityEngine;
using System.Collections;

public class RotateX : MonoBehaviour {

	public float verticalSpeed = 2f;
	void Update() {
		float v = -verticalSpeed * Input.GetAxis("Mouse Y");
		transform.Rotate(v, 0f, 0f);
	}

}
