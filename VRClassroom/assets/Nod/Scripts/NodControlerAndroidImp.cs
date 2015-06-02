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

#if UNITY_ANDROID && !UNITY_EDITOR_WIN
using UnityEngine;
using System;
using System.Collections;
using Nod;

//This file is out of date Android support in unity currently broken.
public class NodControlerAndroidImp : NodControlerInterface
{
	#region private data
	private NodRing[] rings;
	private AndroidJavaObject unityPlugin;
	#endregion // private data

	#region NodControlerInterface methods
	public void ConnectToNod()
	{
		AndroidJavaObject activity;

		using (AndroidJavaClass jc =
			new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		}

		using (AndroidJavaClass jc =
			new AndroidJavaClass("com.nod_labs.unityplugin.UnityPlugin")) {
			unityPlugin = jc.CallStatic<AndroidJavaObject>("getInstance");
			unityPlugin.Call("init", activity);
		}
	}

	public void ShutdownNodConnection()
	{
		if (unityPlugin != null) {
			unityPlugin.Call("shutdown");
		}
	}

	public int GetNumDevices()
	{
		if (null == rings) {
			int[] ringIds = unityPlugin.CallStatic<int[]>("getDeviceIds");
			int numRings = ringIds.Length;
			
			rings = new NodRing[numRings];
			for (int i = 0; i < numRings; i++) {
				int id = ringIds[i];
				string address = unityPlugin.CallStatic<string>("getDeviceAddress", id);
				
				rings[i] = new NodRing(id, address, this);
			}
		}
		
		return rings.Length;
	}

	public NodRing GetRing(int ringID)
	{
		if (ringID >= rings.Length)
			return null;

		return rings[ringID];
	}

	public string GetRingName(int ringId)
	{
		return "Foobar"; //unityPlugin.CallStatic<string>("getName", ringId);
	}

	public NodQuaternionOrientation QuaternionOrientation(int ringId)
	{
		float [] eulers = unityPlugin.CallStatic<float[]>("getRotationData", ringId);
		return eulerToQuaternion(eulers[0], eulers[1], eulers[2]);
	}

	private static NodQuaternionOrientation eulerToQuaternion(float pitch, float roll, float yaw)
	{
		float sinHalfYaw = Mathf.Sin(yaw / 2.0f);
		float cosHalfYaw = Mathf.Cos(yaw / 2.0f);
		float sinHalfPitch = Mathf.Sin(pitch / 2.0f);
		float cosHalfPitch = Mathf.Cos(pitch / 2.0f);
		float sinHalfRoll = Mathf.Sin(roll / 2.0f);
		float cosHalfRoll = Mathf.Cos(roll / 2.0f);

		NodQuaternionOrientation result;
		result.x = -cosHalfRoll * sinHalfPitch * sinHalfYaw
			+ cosHalfPitch * cosHalfYaw * sinHalfRoll;
		result.y = cosHalfRoll * cosHalfYaw * sinHalfPitch
			+ sinHalfRoll * cosHalfPitch * sinHalfYaw;
		result.z = cosHalfRoll * cosHalfPitch * sinHalfYaw
			- sinHalfRoll * cosHalfYaw * sinHalfPitch;
		result.w = cosHalfRoll * cosHalfPitch * cosHalfYaw
			+ sinHalfRoll * sinHalfPitch * sinHalfYaw;

		return result;
	}

	public int ButtonState(int ringId)
	{
		return unityPlugin.CallStatic<int>("getButtonData", ringId);
	}

	public int Gesture(int ringId)
	{
		return unityPlugin.CallStatic<int>("getGestureData", ringId);
	}

	public NodPosition2D Position2D(int ringId)
	{
		NodPosition2D result;
		int [] pointerData = unityPlugin.CallStatic<int[]>("getPointerData", ringId);
		result.x = pointerData[0];
		result.y = pointerData[1];
		return result;
	}

	public NodPosition2D GamePosition(int ringId)
	{
		NodPosition2D result;
		result.x = 0;
		result.y = 0;

		return result; //Josh need to fix this up to support backspin
		//return NodUtilities.NodGetGamePosition(ringIndex);
	}
	
	public int TriggerPressure(int ringId)
	{
		return 0; //Josh need to fix this up to support backspin
		//return NodUtilities.NodGetTrigger(ringIndex);
	}

	public bool Subscribe(NodSubscriptionType type, int ringId)
	{
		bool result = false;
		switch(type){
		case NodSubscriptionType.Button:
			result = unityPlugin.CallStatic<bool>("registerForButtonEvents", ringId);
			break;
		case NodSubscriptionType.GameStick:
			result = true; //NodUtilities.NodSubscribeToGameControl(ringIndex); //Josh need new entry point here
			break;
		case NodSubscriptionType.Gesture:
			result = unityPlugin.CallStatic<bool>("registerForGestureEvents", ringId);
			break;
		case NodSubscriptionType.Orientation:
			result = unityPlugin.CallStatic<bool>("registerForPose6DEvents", ringId);
			break;
		case NodSubscriptionType.Position2D:
			result = unityPlugin.CallStatic<bool>("registerForPointerEvents", ringId);
			break;
		default:
			Debug.Log ("Unhandeled Subscription type.");
			break;
		}
		
		return result;
	}
	
	public bool Unsubscribe(NodSubscriptionType type, int ringId)
	{
		bool result = false;
		switch(type){
		case NodSubscriptionType.Button:
			result = unityPlugin.CallStatic<bool>("unregisterFromButtonEvents", ringId);
			break;
		case NodSubscriptionType.GameStick:
			result = true; //NodUtilities.NodUnSubscribeToGameControl(ringIndex); //Josh need new entry point here
			break;
		case NodSubscriptionType.Gesture:
			result = unityPlugin.CallStatic<bool>("unregisterFromGestureEvents", ringId);
			break;
		case NodSubscriptionType.Orientation:
			result = unityPlugin.CallStatic<bool>("unregisterFromPose6DEvents", ringId);
			break;
		case NodSubscriptionType.Position2D:
			result = unityPlugin.CallStatic<bool>("unregisterFromPointerEvents", ringId);
			break;
		default:
			Debug.Log ("Unhandeled unsubscription type.");
			break;
		}
		
		return result;
	}
	#endregion NodControlerInterface
}
#endif
