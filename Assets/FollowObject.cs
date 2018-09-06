using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirzaBeig.ParticleSystems;
using DG.Tweening;
public class FollowObject : MonoBehaviour
{


    AudioSource audioSource;

    public AudioClip eatingClip;
    public AudioClip drinkingClip;

    public AudioClip buffClip;
    enum FoodType
    {
        Food,
        Drink
    }

    FoodType ourFoodType;
    float followPlayerDuration = 14.0f;
    ParticleSystems ourSystem;
    ParticleSystem ourParticles;
    Transform objectWereFollowing;

    float infectedPlayerStartTime;

    void Awake()
    {

        ourSystem = GetComponentInParent<ParticleSystems>();
        ourParticles = GetComponentInChildren<ParticleSystem>();
        Debug.Log(ourSystem.gameObject.name);
        ScreamFollowObject.ScreamObjectMoving += SetObjectWereFollowing;
        PromptPlayerHit.PlayerFailed += ScreamSucceeded;
        PromptPlayerHit.PlayerParried += StopFollowing;
    }

    void OnDisable()
    {
        ScreamFollowObject.ScreamObjectMoving -= SetObjectWereFollowing;
        PromptPlayerHit.PlayerFailed -= ScreamSucceeded;
        PromptPlayerHit.PlayerParried -= StopFollowing;
    }
    void ScreamSucceeded()
    {
        Debug.Log("Our scream succeeded, now we're going to follow the player");
        //TODO: This will follow the player around for a while and then fade?
        SetObjectWereFollowing(GameHandler.Instance().playerGO.transform);
        GameHandler.Instance().player.EffectCountDown(14, Player.Effects.Tagged);
        infectedPlayerStartTime = Time.time;
        followingPlayer = true;
    }
    void PlayOurSystem()
    {
        ourSystem.Play();
    }

    void StopOurSystem()
    {
        ourSystem.Stop();
    }

    public float speed = 15.0f;
    public float distanceFromCamera = 5.0f;

    bool following;

    bool followingPlayer;
    public bool ignoreTimeScale;

    private MaterialPropertyBlock block;
    private Renderer ourRenderer;

    void SetObjectWereFollowing(Transform target)
    {
        following = true;
        objectWereFollowing = target;

        if (!ourSystem.IsPlaying())
        {
            PlayOurSystem();
        }
    }
    IEnumerator FadeOutParticles()
    {
        float startTime = Time.time;
        float t = 0;
            Color whiteWithAlpha = Color.white;
            whiteWithAlpha.a = 0;
        // ParticleSystemRenderer pRenderer = ourParticles.GetComponent<ParticleSystemRenderer>();
        //pRenderer.material.SetColor("_TintColor", c);
        //Renderer ren = ourParticles.GetComponent<Renderer>();
        //Material mat = ren.material;
        //Debug.Log(mat.name);
        // ParticleSystem

        //mat.SetColor("_TintColor", Color.clear);
        while (Time.time < startTime + 0.5)
        {
            var trails = ourParticles.trails;
            ourRenderer.GetPropertyBlock(block);
            Color newColor = Color.Lerp(block.GetColor("_TintColor"), whiteWithAlpha, t);
            trails.colorOverTrail = newColor;
            block.SetColor("_TintColor", newColor);
            ourRenderer.SetPropertyBlock(block);
            t += Time.deltaTime / 0.5f;
            yield return null;
        }
    }

    void Start()
    {
        block = new MaterialPropertyBlock();
        ourRenderer = ourParticles.GetComponent<Renderer>();
    }
    void StopFollowing()
    {
        Debug.Log("Stop following, and fade");
        following = false;
        objectWereFollowing = null;
        StartCoroutine(FadeOutParticles());
        //ourParticles.Stop();
        if (ourSystem.IsPlaying())
        {
            StopOurSystem();
        }
    }
    void Update()
    {
        if (following)
        {
            float deltaTime = !ignoreTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;
            Vector3 position = Vector3.Lerp(transform.position, objectWereFollowing.position, 1.0f - Mathf.Exp(-speed * deltaTime));

            transform.position = position;
            if (followingPlayer)
            {
                if (Time.time > infectedPlayerStartTime + followPlayerDuration)
                {
                    StopFollowing();
                    followingPlayer = false;
                }
            }
        }
    }

}