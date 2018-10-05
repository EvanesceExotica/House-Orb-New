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
    // GrowCorruptionEffect corruptionEffectGrower;

    MaterialPropertyBlock childBlock;
    SpriteRenderer childRenderer;
    void Awake()
    {
        //TODO: Add distortion effect around edges
        childBlock = new MaterialPropertyBlock();
        foreach (Transform tr in transform)
        {
            if (tr != transform)
            {

                childRenderer = tr.GetComponent<SpriteRenderer>();
            }
        }
        Debug.Log(childRenderer.gameObject.name);
        ourSpriteRenderer = GetComponent<SpriteRenderer>();
        ourMaterialProperties = new MaterialPropertyBlock();
        //ourMaterialProperties = GetComponent<SpriteRenderer>().GetPropertyBlock(0);
        // corruptionEffectGrower = GetComponentInChildren<GrowCorruptionEffect>();
        corruptionEffect = transform.GetChild(0);
        ourCollider = GetComponentInChildren<CircleCollider2D>();
        //       ourVisibleCollider = GetComponentInChildren<VisibleCollider>();
        //        ourVisibleCollider.OurColliderType = VisibleCollider.ColliderTypes.Circle;
        OrbController.ChannelingOrb += SetCanGrowCorruption;
        Sconce.OrbInSconce += SetOrbInSconce;
        Sconce.OrbRemovedFromSconce += SetOrbOutOfSconce;
    }

    void OnDisable()
    {
        OrbController.ChannelingOrb -= SetCanGrowCorruption;
        Sconce.OrbInSconce -= SetOrbInSconce;
        Sconce.OrbRemovedFromSconce -= SetOrbOutOfSconce;
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

    void SpriteFadeWrapper(float startValue, float duration, float endValue)
    {
        StartCoroutine(SpriteFade(startValue, duration, endValue));
    }

    IEnumerator SpriteFade(float startValue, float duration, float endValue)
    {
        float increment = startValue;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            ourSpriteRenderer.GetPropertyBlock(ourMaterialProperties);
            increment = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
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
        float initialSize1 = childBlock.GetFloat("_CircleFade_Size_1");
        float initialSize2 = childBlock.GetFloat("_CircleFade_Size_2");
        float elapsedTime = 0;
        float _radius = ourCollider.radius;
        while (initialSize1 < 1)
        {
            if (!corrupting)
            {
                break;
            }
            //newVector = new Vector2(corruptionEffect.localScale.x + 0.2f, corruptionEffect.localScale.y + 0.2f);
            //Debug.Log("We should be growing to New vector: "  + newVector);
            // currentSize1 = childBlock.GetFloat("_CircleFade_Size_1");
            //currentSize2 = childBlock.GetFloat("_CircleFade_Size_2");
            childRenderer.GetPropertyBlock(childBlock);
            childBlock.SetFloat("_CircleFade_Size_1", Mathf.Lerp(initialSize1, 1.0f, elapsedTime / 20.0f));
            //childBlock.SetFloat("_CircleFade_Size_2", Mathf.Lerp(initialSize2, 1.0f, elapsedTime / 20.0f));
            childRenderer.SetPropertyBlock(childBlock);
            ourCollider.radius = Mathf.Lerp(_radius, 3.0f, elapsedTime / 20.0f);
            elapsedTime += Time.deltaTime;
            //corruptionEffect.DOScale(newVector, 0.5f);

            //	ourCollider.radius = Mathf.Lerp(ourCollider.radius, ourCollider.radius + 0.5f, 0.5f);
            //         ourCollider.radius += 0.5f;

            yield return null;
        }
        //TODO: Maybe add a small screenshake when it hits max
    }

    bool orbInSconce = false;

    void SetOrbInSconce(MonoBehaviour mono)
    {
        orbInSconce = true;
    }

    void SetOrbOutOfSconce(MonoBehaviour mono)
    {
        orbInSconce = false;
    }

    public IEnumerator Cooldown()
    {
        Vector2 newVector = Vector2.zero;
        float currentSize1 = childBlock.GetFloat("_CircleFade_Size_1");
        // float currentSize2 = childBlock.GetFloat("_CircleFade_Size_2");
        float elapsedTime = 0;
        float _radius = ourCollider.radius;
        while (currentSize1 > -0.15f)
        {

            if (corrupting)
            {
                break;
            }
            currentSize1 = childBlock.GetFloat("_CircleFade_Size_1");
            //   currentSize2 = childBlock.GetFloat("_CircleFade_Size_2");
            childRenderer.GetPropertyBlock(childBlock);
            childRenderer.SetPropertyBlock(childBlock);
            //newVector = new Vector2(corruptionEffect.localScale.x - 0.2f, corruptionEffect.localScale.y - 0.2f);
            //corruptionEffect.DOScale(newVector, 0.5f);
            if (orbInSconce || GameHandler.Instance().fatherOrb.coolingDownFromCorruption)
            {
                //the orb being in the sconce should drastically reduce the uncorruption time
                childBlock.SetFloat("_CircleFade_Size_1", Mathf.Lerp(currentSize1, 0f, elapsedTime / 15.0f));
                ///  childBlock.SetFloat("_CircleFade_Size_2", Mathf.Lerp(currentSize2, 0f, elapsedTime / 15.0f));
                // ourCollider.radius = Mathf.Lerp(ourCollider.bounds.extents.x, 3.0f, elapsedTime / 15.0f);
                ourCollider.radius = Mathf.Lerp(_radius, 3.0f, elapsedTime / 15.0f);
            }
            else
            {
                childBlock.SetFloat("_CircleFade_Size_1", Mathf.Lerp(currentSize1, 0f, elapsedTime / 20.0f));
                //  childBlock.SetFloat("_CircleFade_Size_2", Mathf.Lerp(currentSize2, 0f, elapsedTime / 20.0f));
                ourCollider.radius = Mathf.Lerp(_radius, 3.0f, elapsedTime / 20.0f);

            }
            childRenderer.SetPropertyBlock(childBlock);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void ClearCorruption()
    {
        corruptionEffect.DOScale(Vector2.zero, 1.0f);
        corruptionEffect.gameObject.SetActive(false);
    }
    public override void OnChildTriggerEnter2D(Collider2D hit, GameObject child)
    {
        if (hit.gameObject == GameHandler.Instance().fatherOrbGO && canGrowCorruption)
        {
            CorruptingWrapper();
        }
    }

    public override void OnChildTriggerExit2D(Collider2D hit, GameObject child)
    {
        if (hit.gameObject == GameHandler.Instance().fatherOrbGO && canGrowCorruption)
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
