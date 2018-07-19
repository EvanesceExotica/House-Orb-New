using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour {


	Rigidbody2D ourRigidbody2D;
	Collider2D ourCollider;

	void Awake(){
		ReturnPlayerToLastSconce.ReturningToLastSconceWithPlayer += MakePlayerGhost;
		ReturnPlayerToLastSconce.ArrivedAtLastSconceWithPlayer += MakePlayerCoporeal;
	}

	void MakePlayerGhost(UnityEngine.Object ourObject){
		DisableCollider();
		DisableRigidbody();
	}

	void MakePlayerCoporeal(UnityEngine.Object ourObject){
		EnableCollider();
		EnableRigidbody();
	}
	void DisableCollider(){
		ourCollider.enabled = false;
	}

	void EnableCollider(){
		ourCollider.enabled = true;

	}

	void DisableRigidbody(){

		ourRigidbody2D.bodyType = RigidbodyType2D.Static;
	}


	void EnableRigidbody(){
		ourRigidbody2D.bodyType =RigidbodyType2D.Kinematic;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
