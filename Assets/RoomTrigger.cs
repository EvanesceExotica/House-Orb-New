using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour {

	GameObject player;
	Room room;

	public void Awake(){
		room = GetComponent<Room>();
	}
	void OnTriggerEnter2D(Collider2D hit){
//		Debug.Log( hit.gameObject.name + " entered room at " + Time.time);
		if(hit.gameObject.tag == "Player"){
			room.PlayerIntoRoom(room);
		}
		else if (hit.gameObject.tag == "Enemy"){
			room.EnemyEnteredRoom(room);
		}

		Sconce sconce = hit.GetComponent<Sconce>();
		if(sconce != null){
			hit.gameObject.transform.parent = gameObject.transform;
			sconce.parentRoom = room;

		}
		HidingSpace space = hit.GetComponent<HidingSpace>();
		if(space != null){
			hit.transform.parent = gameObject.transform;
			space.parentRoom = room;
			space.SetParentRoomDependencies();
		}
		Memory memory = hit.GetComponent<Memory>();
		if(memory != null){
			hit.transform.parent = gameObject.transform;
			memory.parentRoom = room;
		}
		FurnitureGroup furnitureGroup = hit.GetComponent<FurnitureGroup>();
		if(furnitureGroup != null){
			hit.transform.parent = gameObject.transform;

		}
		HiddenSconce hidden = hit.GetComponent<HiddenSconce>();
		if(hidden != null){
			hit.transform.parent = gameObject.transform;
			hidden.parentRoom = room;
		}
	}

	void OnTriggerExit2D(Collider2D hit){
	//	Debug.Log(hit.gameObject.name + " exited room at " + Time.time);
		if(hit.gameObject.tag == "Player"){
			room.PlayerExited(room);
		}
		else if (hit.gameObject.tag == "Enemy"){
			room.EnemyExitedRoom(room);
		}
	}
	// Use this for initialization

}
