using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("changeScene");
	}

	IEnumerator changeScene(){
		yield return new WaitForSeconds(1);
		Application.LoadLevel ("Start");
	}
}
