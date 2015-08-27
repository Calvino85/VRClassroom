using UnityEngine;
using System.Collections;

public class RotateY : MonoBehaviour {

	public float horizontalSpeed = 2f;
	void Update() {
		float h = horizontalSpeed * Input.GetAxis("Mouse X");
		transform.Rotate(0f, h, 0f);
	}

}
