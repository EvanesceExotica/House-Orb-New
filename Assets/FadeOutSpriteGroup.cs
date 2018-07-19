using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeOutSpriteGroup : MonoBehaviour {

	List<SpriteRenderer> allChildSprites = new List<SpriteRenderer>();

	void Awake(){
		PromptPlayerHit.PlayerParried += ReturnSpritesToFull;
		PromptPlayerHit.PlayerFailed += ReturnSpritesToFull;
		StarScream.ScreamHitRoomAdjacent += FadeAllSpritesInGroup;
	}
	// Use this for initialization
	void Start () {
		StartCoroutine(InitializeSpecialObjects());
		
	}

	public IEnumerator InitializeSpecialObjects(){
		//doing this leaves room for special objects like hiding places and hidden sconces to be put under their respective rooms first I hope
		yield return null;
		allChildSprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
	}

	void FadeAllSpritesInGroup(){
		foreach(SpriteRenderer sRenderer in allChildSprites){
			//TODO: Put this back
			sRenderer.DOFade(0, 1.0f).SetUpdate(true);
		}
	}

	void ReturnSpritesToFull(){

		foreach(SpriteRenderer sRenderer in allChildSprites){
			sRenderer.DOFade(1, 1.0f).SetUpdate(true);
		}
	}

	bool faded = false;	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F) && !faded){
			FadeAllSpritesInGroup();
		}
		else if(Input.GetKeyDown(KeyCode.F) && faded){
			ReturnSpritesToFull();
		}
			
		
	}
}
