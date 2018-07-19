using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AutoRepel : MonoBehaviour {


	public Memory.BuffGiven ourGivenBuff;

	//House-Father: "I... will protect you."

	void Awake(){
		ourGivenBuff = Memory.BuffGiven.AutoReflect;
		Memory.AutoReflectGiven += ActivateAutoRepel;
		PromptPlayerHit.AutoRepelUsed += DeactivateAutoRepel;
	}
	public bool activated;

	public static event Action AutoRepelTriggered; 
	void ActivateAutoRepel(){
		activated = true;
		if(AutoRepelTriggered != null){
			AutoRepelTriggered();
		}
	}

	void DeactivateAutoRepel(){
		activated = false;
	}



	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
