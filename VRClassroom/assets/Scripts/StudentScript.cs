using UnityEngine;
using System.Collections;

public class StudentScript : MonoBehaviour {
	Transform head = null;
	bool toStare = false;
	bool toInitialState = false;
	Quaternion initialDirection;
	Quaternion finalDirection;
	float initTime;
	float delta = 0.0f;
	public float howFast = 5.0f;
	
	// Use this for initialization
	void Start () {
		head = transform.FindChild("Hips/Spine/Spine1/Spine2/Neck/Neck1/Head");
		initialDirection = head.rotation;
	}
	
	public void Stare(Vector3 position) {
		if (!toStare && !toInitialState) {
			// stare at the same height as the original avatar... Right now it is too high...
			Vector3 p = new Vector3(position.x,transform.position.y,position.z);
			toStare = true;
			initTime = Time.time;
			finalDirection = Quaternion.LookRotation( (p - transform.position), transform.up );
			delta = 0.0f;
		}
	}
	
	public void ResetRotation() {
//		if( !toInitialState && toStare ) {
			toStare = false;
			toInitialState = true;
			initTime = Time.time;
			//finalDirection = transform.rotation;
			delta = 0.0f;
//		}
	}	

	// Update is called once per frame
	void Update () {
		if (toStare) {
			delta = (Time.time - initTime) / howFast;
			if( delta > 1.0f ) {
				delta = 1.0f;
				toStare = false;
			}
			head.rotation = Quaternion.Slerp(initialDirection,finalDirection, delta );
		}
		if(toInitialState) {
			delta = (Time.time - initTime) / howFast;
			if( delta > 1.0f ) {
				delta = 1.0f;
				toInitialState = false;
			}
			head.rotation = Quaternion.Slerp(finalDirection,initialDirection, delta );
		}
	}
}


/*
public class StudentScript : MonoBehaviour {
	Transform head = null;
	Vector3 angles;
	bool inRotation = false;
	float initTime;

	// Use this for initialization
	void Start () {
		head = transform.FindChild("Hips/Spine/Spine1/Spine2/Neck/Neck1/Head");
		angles = new Vector3 ();
	}
	
	// Update is called once per frame
	void Update () {
		if (inRotation && head != null) {
			angles.y = angles.y + (Time.time - initTime);
			head.Rotate(angles);
		}
	}

	public void StartRotation() {
		if (! inRotation) {
			inRotation = true;
			initTime = Time.time;
			angles.x = angles.y = angles.z = 0.0f;
		}
	}

	public void ResetRotation() {
		if (inRotation) {
			inRotation = false;
			angles.x = angles.y = angles.z = 0.0f;
			head.Rotate ( angles );
		}
	}	
}
*/
