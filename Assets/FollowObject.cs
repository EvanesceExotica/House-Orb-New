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
        Color c = Color.clear;
        while (Time.time < startTime + 0.5)
        {
            Renderer ren = ourParticles.GetComponent<Renderer>();
            Color newColor = Color.Lerp(ren.material.GetColor("_TintColor"), c, t);
            t += Time.deltaTime/0.5f;
            ourParticles.GetComponent<Renderer>().material.SetColor("_TintColor", newColor);
            yield return null;
        }
        // ParticleSystemRenderer pRenderer = ourParticles.GetComponent<ParticleSystemRenderer>();
        //pRenderer.material.SetColor ("_TintColor", c);
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