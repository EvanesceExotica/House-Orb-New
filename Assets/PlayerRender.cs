using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRender : MonoBehaviour {

	//SpriteRenderer spriteRenderer;
	SpriteRenderer spriteRenderer;

	void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer>();
		//spriteRenderer = GetComponent<SpriteRenderer>();
	//	HidingSpace.PlayerHiding += TurnOffPlayerRenderer;
		//HidingSpace.PlayerNoLongerHiding += TurnOnPlayerRenderer;
		ReturnPlayerToLastSconce.ReturningToLastSconceWithPlayer +=TurnOffPlayerRenderer;
		ReturnPlayerToLastSconce.ArrivedAtLastSconceWithPlayer += TurnOnPlayerRenderer;
	//TODO: Maybe put the above back in? The camera script should handle it
	}

	public void TurnOffPlayerRenderer(UnityEngine.Object ourObject){
		spriteRenderer.enabled = false;
	}

	public void TurnOnPlayerRenderer(UnityEngine.Object ourObject){
		spriteRenderer.enabled = true;
	}
	public void TurnOffPlayerRenderer(GameObject go){
		spriteRenderer.enabled = false;
	}

	public void TurnOnPlayerRenderer(GameObject go){
		spriteRenderer.enabled = true;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
