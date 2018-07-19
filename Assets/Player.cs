using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MirzaBeig.ParticleSystems;
using DG.Tweening;
public class Player : MonoBehaviour {

	Light playerLight;
	public ParticleSystems blinded; //serum2	
	public PlayerRender playerRenderer;
	public PlayerMovement movement;

	public InteractPrompt interactPrompt;

	public bool cantEat;
	public float eatingCooldown;
	// Use this for initialization
	public static event Action PickUpOrb;

	public static event Action Burning;

	public static event Action Blinded;

	public static event Action NoLongerBurning;

	public static event Action Hiding;

	public static event Action NoLongerHiding;

	void PickingUpOrb(){
		if(PickUpOrb != null){
			PickUpOrb();
		}
	}
	public enum PlayerState{
		NotCarryingOrb,
		CarryingOrb,
		Hiding,
		Burned
	}

	public enum TaggedState{
		Normal,
		HidingNormal,
		Tagged,
		HidingTagged
	}

	public TaggedState taggedState;

	void SetTagged(){
		taggedState = TaggedState.Tagged;
	}
	void EatingCooldownWrapper(){
		StartCoroutine(EatingCooldown());
	}
	IEnumerator EatingCooldown(){
		cantEat = true;
		yield return new WaitForSeconds(eatingCooldown);
		cantEat = false;

	}

	public PlayerState playerState;
	
	void SetCarryingOrb(MonoBehaviour ourObject){
		playerState = PlayerState.CarryingOrb;
	}


	void SetNotCarryingOrb(MonoBehaviour ourObject){
		playerState = PlayerState.NotCarryingOrb;
	}

	
	void SetBurned(){

		playerState = PlayerState.Burned;
		if(Burning != null){
			Burning();
		}
		StartCoroutine(EffectCountDown(4.0f, Effects.Burn));
	}

	void SetBlinded(){
		if(Blinded != null){
			Blinded();
		}
		StartCoroutine(EffectCountDown(8.0f, Effects.Blind));
	}

	public static event Action Boosted;
	void SetBoosted(){
		//maybe make these cancel better
		if(Boosted != null){
			Boosted();
		}
		ReverseBoostEffects();
		ReverseBlindEffects();
		StartCoroutine(EffectCountDown(8.0f, Effects.Boost));
	}
	void SetHiding(MonoBehaviour ourObject){
		playerState = PlayerState.Hiding;

	}

	public float burnDuration;
	public float burnStartTime;
	public IEnumerator EffectCountDown(float duration, Effects typeOfEffect){
		float startTime = Time.time;

		if(typeOfEffect == Effects.Burn){
			ApplyBurnEffects();
		}
		else if (typeOfEffect == Effects.Blind){
			ApplyBlindEffects();
		}
		else if(typeOfEffect == Effects.Boost){
			ApplyBoostEffects();
		}
		else if(typeOfEffect == Effects.Tagged){
			ApplyTaggedEffects();
		}

		while(Time.time < startTime + duration){

			yield return null;
		}

		if(typeOfEffect == Effects.Burn){
			ReverseBurnEffects();
		}
		else if(typeOfEffect == Effects.Blind){
			ReverseBlindEffects();
		} 
		else if(typeOfEffect == Effects.Boost){
			ReverseBoostEffects();
		}
		else if(typeOfEffect == Effects.Tagged){
			ReverseTaggedEffects();
		}
	}

	public enum Effects{
		Burn,
		Blind,

		Boost,

		Tagged

	}
	List<Effects> effects = new List<Effects>();

	void ApplyTaggedEffects(){
		effects.Add(Effects.Tagged);
	}

	void ReverseTaggedEffects(){
		effects.Remove(Effects.Tagged);
	}
	void ApplyBoostEffects(){
		effects.Add(Effects.Boost);
		Debug.Log("Boosted!");
		movement.maxSpeed *= 2.0f;
	}

	void ReverseBoostEffects(){
		effects.Remove(Effects.Blind);
		movement.maxSpeed /= 2.0f;
	}

	void ApplyBurnEffects(){
		Debug.Log("BURNED");
		effects.Add(Effects.Burn);
		///one effect of the burn is to half the speed
		movement.maxSpeed *= 0.5f;
	}

	void ReverseBurnEffects(){
		Debug.Log("No longer burned");
		effects.Remove(Effects.Burn);
		movement.maxSpeed /= 0.5f;
	}

	void ApplyBlindEffects(){
		Debug.Log("BLINDED");
		effects.Add(Effects.Blind);
		movement.maxSpeed *= 0.5f;
	}

	void ReverseBlindEffects(){
		Debug.Log("No longer blinded");
		effects.Remove(Effects.Blind);
		movement.maxSpeed /= 0.5f;
	}

	void SetNotBurned(){
		playerState = PlayerState.NotCarryingOrb;
		if(NoLongerBurning != null){
			NoLongerBurning();
		}

	}
	float defaultLightIntensity;

	void FadeOnPlayerLight(MonoBehaviour mono){
		playerLight.DOIntensity(defaultLightIntensity, 1.0f);

	}

	void FadeOffPlayerLight(MonoBehaviour mono){
		playerLight.DOIntensity(0, 1.0f);
	}
	void Awake(){
		playerLight = GetComponentInChildren<Light>();
		defaultLightIntensity = playerLight.intensity;
		interactPrompt = GetComponentInChildren<InteractPrompt>();
		burnDuration = 5.0f;

		playerState = PlayerState.NotCarryingOrb;
		FatherOrb.PickedUp += this.SetCarryingOrb;
		FatherOrb.Dropped += this.SetNotCarryingOrb;
		Sconce.OrbInSconce +=  this.SetNotCarryingOrb;

		playerRenderer = GetComponent<PlayerRender>();
		movement = GetComponent<PlayerMovement>();

		OrbController.ChannelingOrb += SetNotCarryingOrb;
		OrbController.ManuallyStoppedChannelingOrb += SetCarryingOrb;
		HidingSpace.PlayerHiding += SetHiding;

		//TODO -- separate the hiding and the orb-carrying. I want the light to cause the player to be unhidden.

		HidingSpace.PlayerNoLongerHiding += SetNotCarryingOrb;
		PromptPlayerHit.PlayerFailed += SetBlinded;
		Food.AteFood += SetBoosted;
		Food.AteFood +=EatingCooldownWrapper;

		PromptPlayerHit.PlayerFailed += SetTagged;

		OrbController.ChannelingOrb += FadeOffPlayerLight;
		OrbController.ManuallyStoppedChannelingOrb += FadeOnPlayerLight;
		OrbController.SconceRevealedStoppedChannelingOrb += FadeOnPlayerLight;
	}

}
