using UnityEngine;
using System.Collections;
using Nod;

public class Select : MonoBehaviour {
	
	private NodControler nod;
	private NodRing ring;	
	private bool nodRingConnected = false;	
	private const int ringID = 0; //0 for the first connected ring
	public float lastTime=0;
	public bool up = false;
	public int buttonValue=-1;
	
	private bool RingConnectedAndInitialized()
	{
		
		//Debug.Log("inicio");
		if (!nodRingConnected) {
			//Ring connections happen asynchronously on mobile devices, check each frame for a connected ring
			int numRingsPaired = nod.getNumDevices();
			if (numRingsPaired > 0) {
				ring = nod.getRing(ringID);
				ring.Subscribe(NodSubscriptionType.Gesture);
				ring.Subscribe(NodSubscriptionType.Position2D);
				ring.Subscribe(NodSubscriptionType.Button);
				ring.Subscribe(NodSubscriptionType.GameStick);		
				ring.Subscribe(NodSubscriptionType.Orientation);
				nodRingConnected = true;
			} else 
				return false;
		}
		
		return true;
	}
	
	
	void OnEnable() 
	{
		//This will create a GameObject in your Hierarchy called "NodControler" which will manage
		//interactions with all connected nod rings.  It will presist between scene loads.  Only
		//one instance will be created if you request a nod interface from multiple locations 
		//in your code.
		nod = NodControler.GetNodInterface();		
	}
	
	void OnDisable()
	{
		if (null == ring)
			return;
		
		ring.Unsubscribe(NodSubscriptionType.Gesture);
		ring.Unsubscribe(NodSubscriptionType.Position2D);
	}
	
	void Update() 
	{
		if (!RingConnectedAndInitialized())
			return;
		
		//Call this once per update to check for updated ring values.
		ring.CheckForUpdate();
		
		//Example of applying the rings orientation to the local transform.
		//Vector3 updatePosition = new Vector3(0.001f * (float)ring.position2D.x, -0.001f * ring.position2D.y);

		//Debug.Log (lastTime+" "+Time.time);
		
		buttonValue=buttonPress();


		bool bs=GameObject.Find("ButtonS").GetComponent<SelectSpanish>().selected;
		bool be=GameObject.Find("ButtonEn").GetComponent<SelectEnglish>().selected;

		Debug.Log ("BTT "+buttonValue+" BS "+bs);
		Debug.Log ("BTT "+buttonValue+" BE "+be);
		if(be==true&&buttonValue==4){
			GameObject.Find("PlaneO").GetComponent<lenguage>().l=0;
			Application.LoadLevel("Classroom");
			Object.DontDestroyOnLoad(GameObject.Find("PlaneO"));
		}

		if(bs==true&&buttonValue==4){
			GameObject.Find("PlaneO").GetComponent<lenguage>().l=1;
			Application.LoadLevel("Classroom");
			Object.DontDestroyOnLoad(GameObject.Find("PlaneO"));
		}

		//Debug.Log (buttonValue);
		/**/
	}
	
	private GestureEventType mostRecentGesture = GestureEventType.NONE;

	private int buttonPress(){
		//Debug.Log ("Hola");
		if (nodRingConnected) {	
			string [] buttonNames = {"touch0", "touch1", "touch2", "tactile0", "tactile1"};
			string [] buttonPressStatus = {
				ring.buttonState.touch0 ? "Down" : "Up",
				ring.buttonState.touch1 ? "Down" : "Up",
				ring.buttonState.touch2 ? "Down" : "Up",
				ring.buttonState.tactile0 ? "Down" : "Up",
				ring.buttonState.tactile1 ? "Down" : "Up"
			};
			
			//if(ring.buttonState.touch0==true){
			//Debug.Log (ring.buttonState.touch0);
			//}
			
			if(ring.buttonState.touch0 == true){
				return 1;
			}else{
				if(ring.buttonState.touch1 == true){
					return 2;
				}else{
					if(ring.buttonState.touch2 == true){
						return 3;
					}else{
						if(ring.buttonState.tactile0 == true){
							return 4;
						}else{
							if(ring.buttonState.tactile1 == true){
								return 5;
							}	
						}	
					}	
				}
			}
		}	
		return -1;
	}
	
}
