using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;


[EventHandlerInfo("CutsceneStarters",
				  "MemoryStart",
				  "Starts a memory when the player interacts with a memory object")]

public class MemoryStartedEventHandler : EventHandler {


	void Awake(){
		Memory.LookingAtMemory += MemoryStarted;
	}

	void OnDisable(){
		Memory.LookingAtMemory -= MemoryStarted;

	}
	// Use this for initialization
	void Start () {
		
	}
	
	void MemoryStarted(MonoBehaviour memoryObject){
		ExecuteBlock();
	}
	// Update is called once per frame
	void Update () {
		
	}
}
