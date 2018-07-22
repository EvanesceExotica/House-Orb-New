using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MutePause : MonoBehaviour {

	AudioListener audioListener;
	// Use this for initialization

	void LowerVolume(){
		AudioListener.volume *= 0.5f;
	}

	void RaiseVolumeToNormal(){
		AudioListener.volume *= 2;

	}

	public IEnumerator MutePauseVolume(){
		LowerVolume();
		yield return new WaitForSeconds(1.0f);
		RaiseVolumeToNormal();

	}
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.V)){
			StartCoroutine(MutePauseVolume());
		}
		
	}
}
