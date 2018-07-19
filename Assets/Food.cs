using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MirzaBeig.ParticleSystems;
public class Food : MonoBehaviour, iInteractable {

	public string eatPrompt;
	public ParticleSystems eatenParticleSystem;
	public static event Action AteFood;

	AudioSource audioSource;

	public AudioClip eatSound;
	public AudioClip drinkSound;

	void Awake(){
		eatPrompt = " eat";
	}
	public void AteFoodWrapper(){
		if(AteFood != null){
			AteFood();
		}
	}

	void BeConsumed(){
		//TODO: Put this system back in
		//eatenParticleSystem.Play();

		AteFoodWrapper();
		gameObject.SetActive(false);

	}

	void PlayEatSound(){
		audioSource.PlayOneShot(eatSound);
	}

	void PlayDrinkSound(){
		audioSource.PlayOneShot(drinkSound);
	}
	public void OnHoverMe(Player player){
		player.interactPrompt.DisplayPrompt(eatPrompt, gameObject);
		Debug.Log("Press [E] to eat food");
	}

	public void OnInteractWithMe(Player player){
		BeConsumed();
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
