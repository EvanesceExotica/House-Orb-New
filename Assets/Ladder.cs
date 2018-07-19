using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, iInteractable {

	public string climbPrompt;
	Transform PointA;
	Transform PointB;

	public Transform destinationPoint;

	void Awake(){
		climbPrompt = " take ladder";
		PointA = transform.Find("PointA");
		PointB = transform.Find("PointB");
	}

	public void OnHoverMe(Player player){
		player.interactPrompt.DisplayPrompt(climbPrompt, this.gameObject);
		Debug.Log("Press [E] to take ladder");
	}	

	public void OnInteractWithMe(Player player){
		
		GameHandler.playerGO.transform.position = destinationPoint.position;

	}

	public void OnStopHoverMe(Player player){

		player.interactPrompt.HidePrompt(gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
