using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEffects : MonoBehaviour {

	public GameObject hauntingLights;
	// Use this for initialization
	void Awake(){
		hauntingLights = transform.Find("Haunting Lights").gameObject;
		HidingSpace.PlayerHiding += DisableLight;
		HidingSpace.PlayerNoLongerHiding += EnableLight;
	}

	void DisableLight(MonoBehaviour ourObject){
		hauntingLights.SetActive(false);
	}

	void EnableLight(MonoBehaviour ourObject){

		hauntingLights.SetActive(true);
	}



	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
