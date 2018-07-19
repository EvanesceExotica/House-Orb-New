using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirzaBeig.ParticleSystems;
public class FollowObject : MonoBehaviour
{


    AudioSource audioSource;

    public AudioClip eatingClip;
    public AudioClip drinkingClip;

    public AudioClip buffClip;
    enum FoodType{
        Food,
        Drink
    }

    FoodType ourFoodType;
    float followPlayerDuration = 14.0f;
    ParticleSystems ourSystem;
    Transform objectWereFollowing;

    float infectedPlayerStartTime;

    void Awake()
    {
        ourSystem = GetComponentInParent<ParticleSystems>();
        ScreamFollowObject.ScreamObjectMoving += SetObjectWereFollowing;
        PromptPlayerHit.PlayerFailed += ScreamSucceeded;
        PromptPlayerHit.PlayerParried += StopFollowing;
    }
    
    void ScreamSucceeded(){
        Debug.Log("Our scream succeeded, now we're going to follow the player");
        //TODO: This will follow the player around for a while and then fade?
        SetObjectWereFollowing(GameHandler.playerGO.transform);
        GameHandler.player.EffectCountDown(14, Player.Effects.Tagged);
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

    void StopFollowing()
    {
        Debug.Log("Stop following, and fade");
        following = false;
        objectWereFollowing = null;
        if(ourSystem.IsPlaying()){
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
            if(followingPlayer){
                if(Time.time > infectedPlayerStartTime + followPlayerDuration){
                    StopFollowing();
                    followingPlayer = false;
                }
            }
        }
    }

}
