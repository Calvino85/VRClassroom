using UnityEngine;
using System.Collections;
using Nod;

public class Hand : MonoBehaviour {

		private NodControler nod;
		private NodRing ring;	
		private bool nodRingConnected = false;	
		private const int ringID = 0; //0 for the first connected ring
		public float lastTime=0;
		public bool up = false;
		public int buttonValue=-1;
		public Vector3 initialPosition;
		public GameObject sphere;
		private Quaternion inverseInitialRotation = Quaternion.identity;
		public GameObject cube;
		public bool takeTime = false;
		public bool takeTimeRecenter = false;
		public bool grabar=false;

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
					initialPosition=cube.transform.localPosition;
					sphere.GetComponent<Renderer>().enabled=false;
//					Debug.Log (initialPosition);
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
			Vector3 updatePosition = new Vector3(0.001f * (float)ring.position2D.x, -0.001f * ring.position2D.y);
			
			cube.transform.localPosition = cube.transform.localPosition + updatePosition;
			//Debug.Log (updatePosition.x+" "+updatePosition.y);

			//Example of applying the rings orientation to the local transform.
			//transform.localRotation = inverseInitialRotation  * ring.ringRotation;
			
			//Debug.Log ("resta= "+(cube.transform.localPosition.y-initialPosition.y));
			
			
			if (cube.transform.localPosition.y - initialPosition.y > 0.5) {
					up = true;
			} else {
				up=false;
				takeTime=false;
			}

			if (up == true) {
					if(takeTime==false){
						lastTime = Time.time;
						takeTime=true;
					}
					
					sphere.GetComponent<Renderer> ().enabled = true;
					sphere.GetComponent<Renderer> ().material.color = Color.red;
					//Debug.Log ("time: "+lastTime);
			} else {
				sphere.GetComponent<Renderer> ().enabled = false;
			}
			
			//Debug.Log ("time: "+(Time.time - lastTime));

			if(Time.time - lastTime > 5.0&& up==true){				
				
				Debug.Log ("Pasaron 5 seg");
				sphere.GetComponent<Renderer>().material.color = Color.green;
				grabar=true;

			}

			if(grabar==true){
				sphere.GetComponent<Renderer>().material.color = Color.green;
			}

			//Debug.Log (lastTime+" "+Time.time);
		
			buttonValue=buttonPress();
			
			if(buttonValue==3){
				if(takeTimeRecenter==false){
					lastTime = Time.time;
					takeTimeRecenter=true;
				}
			}else{
				takeTimeRecenter=false;
			}
		
			if(Time.time - lastTime > 1.0&&buttonValue==3){
				//recentrar
			}

			Debug.Log (buttonValue);
		           /**/
		}
		
		private GestureEventType mostRecentGesture = GestureEventType.NONE;
		
		/*void OnGUI()
		{
			if (!nodRingConnected) {		
				Rect windowRect = new Rect(Screen.width/2f - Screen.width/8f, 
				                           Screen.height/2f - Screen.height/8f, 
				                           Screen.width/4f,
				                           Screen.height/4f);
				string message = "Unable to find a paired Nod rings.\nLoad the blue tooth settings for your\nmachine and make sure a Nod ring is connected.";
				GUI.Window(0, windowRect, noConnectionWindow, message);
			} else {
				if (ring.gestureState != GestureEventType.NONE)
					mostRecentGesture = ring.gestureState;
				Rect windowRect = new Rect(0, 0, Screen.width, 30);
				string text = "Most recent gesture: " + mostRecentGesture.ToString();
				GUI.Button(windowRect, text);		
			}
		}*/

		/*public void OnGUI()
		{
			if (!nodRingConnected) {		
				Rect windowRect = new Rect(Screen.width/2f - Screen.width/8f, 
				                           Screen.height/2f - Screen.height/8f, 
				                           Screen.width/4f,
				                           
				                           Screen.height/4f);
				string message = "Unable to find a paired Nod rings.\nLoad the blue tooth settings for your\nmachine and make sure a Nod ring is connected.";
				GUI.Window(0, windowRect, noConnectionWindow, message);
			} else {
				
				//Display the status of each button
				string [] buttonNames = {"touch0", "touch1", "touch2", "tactile0", "tactile1"};
				string [] buttonPressStatus = {
					ring.buttonState.touch0 ? "Down" : "Up",
					ring.buttonState.touch1 ? "Down" : "Up",
					ring.buttonState.touch2 ? "Down" : "Up",
					ring.buttonState.tactile0 ? "Down" : "Up",
					ring.buttonState.tactile1 ? "Down" : "Up"
				};
				
				Debug.Log (ring.buttonState.touch0);
				int numWindows = buttonNames.Length;
				float windowWidth = Screen.width / numWindows;
				float windowHeight = 40;
				for (int ndx = 0; ndx < numWindows; ndx++) {
					float currentX = windowWidth * ndx;
					Rect windowRect = new Rect(currentX, 0, 
					                           windowWidth, windowHeight);
					string text = buttonNames[ndx] + " status:\n " + buttonPressStatus[ndx];
					GUI.Button(windowRect, text);
				}
				
				//Button to reorient the model relative to the current orientation of the ring
				Rect bottomWindowRect = new Rect(0, Screen.height - 30, Screen.width, 30);
				string message = "Click here, or tap space, to reorient the model relative to the current orientation of the ring";
				if (GUI.Button(bottomWindowRect, message)) {
					//recenter();
				}
			}
			
		}
		
		private void recenter()
		{
			//inverseInitialRotation = Quaternion.Inverse(ring.ringRotation);
		}
		
		private void noConnectionWindow(int windowID) 
		{
			const int buttonWidth = 100;
			const int buttonHeight = 20;
			if (GUI.Button(new Rect(Screen.width/8f - buttonWidth/2f, 
			                        Screen.width/8f - buttonHeight/2f - 15, 
			                        buttonWidth, 
			                        buttonHeight), "Ok")) 
			{
				Application.Quit();
			}
		}/**/

		private int buttonPress(){
		Debug.Log ("Hola "+nodRingConnected);
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
					Debug.Log (ring.buttonState.touch0);
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
