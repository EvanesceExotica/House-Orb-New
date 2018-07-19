using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[System.Serializable]
public class SetSpriteHue : MonoBehaviour {

	public float hue;
	public float saturation;

	public float brightness;

	private MaterialPropertyBlock ourBlock;
	SpriteRenderer ourSpriteRenderer;


	void Awake(){
		ourBlock = new MaterialPropertyBlock();
		ourSpriteRenderer = GetComponent<SpriteRenderer>();
	}

	MaterialPropertyBlock ourPropertyBlock;
	// Use this for initialization
	void Start () {
		
		ourBlock = new MaterialPropertyBlock();
	}
	
	// Update is called once per frame
	void Update () {

		ourBlock = new MaterialPropertyBlock();
		ourSpriteRenderer.GetPropertyBlock(ourBlock);
		ourBlock.SetFloat("Hue", hue);
		ourBlock.SetFloat("Saturation", saturation);
		ourBlock.SetFloat("Brightness", brightness);
		ourSpriteRenderer.SetPropertyBlock(ourPropertyBlock);

		
	}
}
