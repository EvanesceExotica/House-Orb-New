using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour, iInteractable {


	BoxCollider2D ourExitCollider;

	void Awake(){
		ourExitCollider = GetComponent<BoxCollider2D>();
		ourExitCollider.enabled = false;
		SconceManager.AllSconcesLit += UnlockedExit;	
	}
	void UnlockedExit(){
		//todo: door-opening animations and stuff
		ourExitCollider.enabled = true;
		Debug.Log("Door opened!");
	}

	public void OnInteractWithMe(Player player){
		Debug.Log("Load next level");

	}

	public void OnHoverMe(Player player){
		Debug.Log("Press E to load next level");
	}

	public void OnStopHoverMe(Player player){

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
