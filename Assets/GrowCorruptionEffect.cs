using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowCorruptionEffect : MonoBehaviour {

	MaterialPropertyBlock ourBlock;
	Renderer ourRenderer;
	// Use this for initialization

	void Awake(){
		ourBlock = new MaterialPropertyBlock();
		ourRenderer = GetComponent<Renderer>();
	}
	void Start () {
		
	}

	public void SetCorruptionFloats(float value){
			ourRenderer.GetPropertyBlock(ourBlock);
			float currentSize1 = ourBlock.GetFloat("_CircleFade_Size_1");
			float currentSize2 = ourBlock.GetFloat("_CircleFade_Size_2");
			ourBlock.SetFloat("_CircleFade_Size_1", value);
			ourBlock.SetFloat("_CircleFade_Size_2", value);

	}

	void GrowCorruptionWrapper(float time){
		StartCoroutine(GrowCorruption(time));
	}

	public IEnumerator GrowCorruption(float time){
		float elapsedTime = 0;
		while(elapsedTime < time){
			ourRenderer.GetPropertyBlock(ourBlock);
			float currentSize1 = ourBlock.GetFloat("_CircleFade_Size_1");
			float currentSize2 = ourBlock.GetFloat("_CircleFade_Size_2");
			ourBlock.SetFloat("_CircleFade_Size_1", Mathf.Lerp(currentSize1, 1.0f, elapsedTime / time));
			ourBlock.SetFloat("_CircleFade_Size_2", Mathf.Lerp(currentSize2, 1.0f, elapsedTime/time));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
