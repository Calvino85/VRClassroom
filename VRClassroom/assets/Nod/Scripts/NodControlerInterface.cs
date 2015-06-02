﻿/*
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
using System.Collections;
using Nod;

public interface NodControlerInterface
{
	void ConnectToNod();
	void ShutdownNodConnection();

	int GetNumDevices();

	NodRing GetRing(int ringID);
	string GetRingName(int ringIndex);
	
	NodQuaternionOrientation QuaternionOrientation(int ringIndex);
	int ButtonState(int ringIndex);
	int Gesture(int ringIndex);
	NodPosition2D Position2D(int ringIndex);
	NodPosition2D GamePosition(int ringIndex);
	int TriggerPressure(int ringIndex);
	
	bool Subscribe(NodSubscriptionType type, int ringIndex);
	bool Unsubscribe(NodSubscriptionType type, int ringIndex);
}