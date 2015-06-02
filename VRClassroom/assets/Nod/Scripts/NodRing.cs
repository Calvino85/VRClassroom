/*
 * Copyright 2014 Nod Labs
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using UnityEngine;
using Nod;

public class NodRing
{
	private NodControlerInterface ringInterface; 
	public int ringIndex;
	public string ringAddress;

	public Quaternion ringRotation;
	public NodButtons buttonState;
	public GestureEventType gestureState;
	public NodPosition2D position2D;
	public NodPosition2D gamePosition;

	public enum RingModes { TouchToMove, Pose6D, GamePad };

	private bool readFirstOrientation = false;
	private Quaternion inverseInitialOrientation = Quaternion.identity;

	private int [] subscribeCount = new int[(int)NodSubscriptionType.Count];
	private bool [] subscribedTo = new bool[(int)NodSubscriptionType.Count];

	private void init(int index, string address, NodControlerInterface nci)
	{
		ringIndex = index;
		ringAddress = address;
		ringInterface = nci;

		for (int ndx = 0; ndx < (int)NodSubscriptionType.Count; ndx++) {
			subscribedTo[ndx] = false;
		}

		ringRotation = Quaternion.identity;

		buttonState.touch0 = false;
		buttonState.touch1 = false;
		buttonState.touch2 = false;
		buttonState.tactile0 = false;
		buttonState.tactile1 = false;

		gestureState = GestureEventType.NONE;

		position2D.x = 0;
		position2D.y = 0;

		gamePosition.x = 0;
		gamePosition.y = 0;
	}

	public NodRing(int index, NodControlerInterface nci)
	{
		init(index, "unknown", nci);
	}

	public NodRing(int index, string address, NodControlerInterface nci)
	{
		init(index, address, nci);
	}

	public void CheckForUpdate()
	{
		if (subscribedTo[(int)NodSubscriptionType.Button]) {
			int buttonBitField = ringInterface.ButtonState(ringIndex);
			buttonState.touch0 = (buttonBitField & (1 << 0)) != 0;
			buttonState.touch1 = (buttonBitField & (1 << 1)) != 0;
			buttonState.touch2 = (buttonBitField & (1 << 2)) != 0;
			buttonState.tactile0 = (buttonBitField & (1 << 3)) != 0;
			buttonState.tactile1 = (buttonBitField & (1 << 4)) != 0;
		}
		if (subscribedTo[(int)NodSubscriptionType.Orientation]) {
			//Read the raw quaternion from the ring
			NodQuaternionOrientation orientation = ringInterface.QuaternionOrientation(ringIndex);
			Quaternion rot = new Quaternion(orientation.x, orientation.y, orientation.z, orientation.w);

			//Make the first value we read the new origin for this ring.  Until the ring updates rot will be Quaternion.identity
			//This will prevent drastic jumps the first time you read from the ring.
			if (!readFirstOrientation && (rot != Quaternion.identity)) {
				readFirstOrientation = true;
				inverseInitialOrientation = Quaternion.Inverse(rot);
			}
			if (readFirstOrientation) {
				rot = inverseInitialOrientation * rot;
			}

			ringRotation = rot;
		}
		if (subscribedTo[(int)NodSubscriptionType.Gesture]) {
			int gestureEnumValue = ringInterface.Gesture(ringIndex);
			gestureState = (GestureEventType)gestureEnumValue;
		}
		if (subscribedTo[(int)NodSubscriptionType.Position2D]) {
			position2D = ringInterface.Position2D(ringIndex);
		}
		if (subscribedTo[(int)NodSubscriptionType.GameStick]) 
		{
			gamePosition = ringInterface.GamePosition(ringIndex);
		}
	}

	public void PrintCurrentState()
	{
		if (subscribedTo[(int)NodSubscriptionType.Button]) {
			Debug.Log ("Button state: " +
			           "touch0: " + (buttonState.touch0 ? "D" : "U") + ", " +
			           "touch1: " + (buttonState.touch1 ? "D" : "U") + ", " +
			           "touch2: " + (buttonState.touch2 ? "D" : "U") + ", " +
			           "tactile0: " + (buttonState.tactile0 ? "D" : "U") + ", " +
			           "tactile1: " + (buttonState.tactile1 ? "D" : "U") + ", ");
		}
		if (subscribedTo[(int)NodSubscriptionType.Orientation]) {
			Debug.Log ("Rotation quaternion: " + ringRotation.ToString());
		}
		if (subscribedTo[(int)NodSubscriptionType.Gesture]) {
			Debug.Log("Gesture Event type: " + gestureState.ToString());
		}
		if (subscribedTo[(int)NodSubscriptionType.Position2D]) {
			Debug.Log("Position Delta (x, y): " + position2D.x + ", " + position2D.y);
		}
		if (subscribedTo[(int)NodSubscriptionType.GameStick]) {
			Debug.Log ("Game (x, y): " + gamePosition.x + ", " + gamePosition.y);
		}
	}

	public void StopTracking()
	{
		for (int ndx = 0; ndx < (int)NodSubscriptionType.Count; ndx++) {
			if (subscribedTo[ndx]) {
				Unsubscribe((NodSubscriptionType)ndx);
			}
		}
	}
	
	public bool Subscribe(NodSubscriptionType type)
	{
		if ((int)type >= (int)NodSubscriptionType.Count || (int)type < 0) {
			Debug.Log("Unknown subscription type");
			return false;
		}

		bool subscriptionWorked = false;

		int index = (int)type;

		subscribeCount[index]++;
		if (1 == subscribeCount[index]) {
			subscriptionWorked = ringInterface.Subscribe(type, ringIndex);
			subscribedTo[index] = subscriptionWorked;
		} else			
			subscriptionWorked = subscribedTo[index];

		return subscriptionWorked;
	}
	
	public bool Unsubscribe(NodSubscriptionType type)
	{
		if ((int)type >= (int)NodSubscriptionType.Count || (int)type < 0) {
			Debug.Log("Unknown subscription type");
			return false;
		}
		
		bool unSubscriptionWorked = false;

		int index = (int)type;
		
		subscribeCount[index]--;
		if (0 == subscribeCount[index]) {
			subscribedTo[index] = false;
			unSubscriptionWorked = ringInterface.Unsubscribe(type, ringIndex);
		} else
			unSubscriptionWorked = true;

		return unSubscriptionWorked;
	}

	public string GetRingName()
	{
		return ringInterface.GetRingName(ringIndex);
	}
}
