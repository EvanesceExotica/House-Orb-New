using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using DG.Tweening;
public class CameraShakeCustom : MonoBehaviour {

	ProCamera2D ourCamera;
	Camera mainCamera;

	void Awake(){
		ourCamera = GetComponent<ProCamera2D>();
		mainCamera = Camera.main;
		Monster.ReadyToScream += this.ScreamShake;
		StarScream.ScreamHitPlayerCurrentRoom += ScreamShake;
		PromptPlayerHit.PlayerParried += ScreamShake;
	}

	public void ScreamShake(int x){
		mainCamera.DOShakePosition(3, 10, 10, 90, true);
		//mainCamera.DOShakePosition(10, 10, 10, 90, true);
	}
	public void ScreamShake(){
		Camera.main.DOShakePosition(3, 10, 10, 90, true);
	}
	public void ShakeUs(float duration, float strength, int vibrato, float randomness, bool fadeOut){
		Camera.main.DOShakePosition(duration, strength, vibrato, randomness, fadeOut);

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
}
