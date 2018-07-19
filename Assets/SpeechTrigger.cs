using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SpeechTrigger {

	// Use this for initialization
	public static event Action<Speech> SpeechTriggered;

	public static void SpeechTriggeredWrapper(Speech speech) {
		if(SpeechTriggered != null){
			SpeechTriggered(speech);
		}
	}
	
	// Update is called once per frame
		
}
