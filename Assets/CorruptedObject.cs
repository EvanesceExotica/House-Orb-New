using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class CorruptedObject : ParentTrigger
{

	MaterialPropertyBlock ourMaterialProperties;

	Transform corruptionEffect;
    CircleCollider2D ourCollider;
    VisibleCollider ourVisibleCollider;
	SpriteRenderer ourSpriteRenderer;
    float cooldownInterval;
    void Awake()
    {
		ourSpriteRenderer = GetComponent<SpriteRenderer>();
		ourMaterialProperties = new MaterialPropertyBlock();
		//ourMaterialProperties = GetComponent<SpriteRenderer>().GetPropertyBlock(0);
		corruptionEffect = transform.GetChild(0);
        ourCollider = GetComponentInChildren<CircleCollider2D>();
 //       ourVisibleCollider = GetComponentInChildren<VisibleCollider>();
//        ourVisibleCollider.OurColliderType = VisibleCollider.ColliderTypes.Circle;
        OrbController.ChannelingOrb += SetCanGrowCorruption;
		Sconce.OrbInSconce += SetOrbInSconce;
		Sconce.OrbRemovedFromSconce += SetOrbOutOfSconce;
    }
    public static event Action Corrupting;

    void CorruptingWrapper()
    {
		corrupting = true;
		StartCoroutine(GrowCollider());
        if (Corrupting != null)
        {
            Corrupting();
        }
    }
    public static event Action StoppedCorrupting;

    void StoppedCorruptingWrapper()
    {
		corrupting = false;
		StartCoroutine(Cooldown());
        if (StoppedCorrupting != null)
        {
            StoppedCorrupting();
        }
    }

	void SpriteFadeWrapper(float startValue, float duration, float endValue){
		StartCoroutine(SpriteFade(startValue, duration, endValue));
	}

	IEnumerator SpriteFade(float startValue, float duration, float endValue){
		float increment = startValue;
		float elapsedTime = 0;
		while(elapsedTime < duration){
			ourSpriteRenderer.GetPropertyBlock(ourMaterialProperties);
			increment = Mathf.Lerp(startValue, endValue, elapsedTime/duration);
			ourMaterialProperties.SetFloat("SpriteFade", increment);
			ourSpriteRenderer.SetPropertyBlock(ourMaterialProperties);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}

    bool canGrowCorruption = false;
	public bool corrupting = false;
    void SetCanGrowCorruption(MonoBehaviour mono)
    {
        canGrowCorruption = true;
    }

    public IEnumerator GrowCollider()
    {
		Vector2 newVector = Vector2.zero;
        while (true)
        {
			if(!corrupting){
				break;
			}
			newVector = new Vector2(corruptionEffect.localScale.x + 0.2f, corruptionEffect.localScale.y + 0.2f);
			//Debug.Log("We should be growing to New vector: "  + newVector);
			corruptionEffect.DOScale(newVector, 0.5f);

		//	ourCollider.radius = Mathf.Lerp(ourCollider.radius, ourCollider.radius + 0.5f, 0.5f);
   //         ourCollider.radius += 0.5f;

			yield return new WaitForSeconds(0.5f);
        }
    }

	bool orbInSconce = false;

	void SetOrbInSconce(MonoBehaviour mono){
		orbInSconce = true;
	}

	void SetOrbOutOfSconce(MonoBehaviour mono){
		orbInSconce = false;
	}

	public IEnumerator Cooldown(){
		Vector2 newVector = Vector2.zero;
		while(true){
			if(corrupting){
				break;
			}
			newVector = new Vector2(corruptionEffect.localScale.x - 0.2f, corruptionEffect.localScale.y - 0.2f);
			corruptionEffect.DOScale(newVector, 0.5f);
			if(orbInSconce || GameHandler.fatherOrb.coolingDownFromCorruption){
				//the orb being in the sconce should drastically reduce the uncorruption time
				yield return new WaitForSeconds(1.0f);
			}
			else{
				yield return new WaitForSeconds(5.0f);
			}
		}
	}

	void ClearCorruption(){
		corruptionEffect.DOScale(Vector2.zero, 1.0f);
		corruptionEffect.gameObject.SetActive(false);
	}
    public override void OnChildTriggerEnter2D(Collider2D hit, GameObject child)
    {
        if (hit.gameObject == GameHandler.fatherOrbGO && canGrowCorruption)
        {
            CorruptingWrapper();
        }
    }

    public override void OnChildTriggerExit2D(Collider2D hit, GameObject child)
    {
        if (hit.gameObject == GameHandler.fatherOrbGO && canGrowCorruption)
        {
            StoppedCorruptingWrapper();
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
