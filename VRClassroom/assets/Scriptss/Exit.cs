using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {
	public Animation animation;
	public GameObject ObjectAnim;
	private bool play;
	private bool exit;

	// Use this for initialization
	void Start () {
		play=true;
	}
	
	// Update is called once per frame
	void Update () {
		int att=GameObject.Find("Teacher").GetComponent<speak>().att;
		int pause=GameObject.Find("Teacher").GetComponent<speak>().pause;

		if(att>4&&pause==1&&play==true){
			ObjectAnim.animation.Play("exit");
			play=false;
		}else{
			//Application.LoadLevel(“menu”); //menu scene
			Application.Quit();//exit 
		}
	}
}
