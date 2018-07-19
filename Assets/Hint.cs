using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MirzaBeig.ParticleSystems;
public class Hint : MonoBehaviour
{

    public Speech speech; //something like "House Father must have sensed something." or "It's here."

    public ParticleSystems sensingSystem;
    HiddenSconce ourHiddenSconce;
    bool canSenseHiddenSconce;
    public static event Action InRoomWithHiddenSconce;


    void Awake()
    {
        //TODO: Will the player seeing another memory override this?
        HiddenSconce.InSemiCloseRange += SetSemiClosePlaybackSpeed;
        HiddenSconce.InCloseRange += SetVeryClosePlaybackSpeed; 
        Memory.HintGiven += SetCanSenseSconce;
    }

    void CheckIfRoomWithOurSconce(Room room)
    {
        if (room == ourHiddenSconce.parentRoom && canSenseHiddenSconce)
        {
            PlaySensingEffect();
            if (speech != null)
            {
                SpeechTrigger.SpeechTriggeredWrapper(speech);
            }
        }
        else{
            if(sensingSystem.IsPlaying()){
                StopSensingEffect();
            }
        }
    }

    void StopSensingEffect(){
        sensingSystem.Stop();
    }

    void PlaySensingEffect()
    {
        sensingSystem.SetPlaybackSpeed(0.5f);
        sensingSystem.Play();

    }
	void SetSemiClosePlaybackSpeed(){
		sensingSystem.SetPlaybackSpeed(1f);
	}

    void  SetVeryClosePlaybackSpeed(){
        sensingSystem.SetPlaybackSpeed(1.5f);
    }
    void SetCanSenseSconce(HiddenSconce sconce)
    {
        ourHiddenSconce = sconce;
        canSenseHiddenSconce = true;
        ourHiddenSconce.ThisSconceRevealed += this.SetCanNOTSenseSconce;
        RoomManager.PlayerEnteredNewRoom += CheckIfRoomWithOurSconce;
    }

    void SetCanNOTSenseSconce()
    {
        StopSensingEffect();
        canSenseHiddenSconce = false;
        RoomManager.PlayerEnteredNewRoom -= CheckIfRoomWithOurSconce;
        ourHiddenSconce.ThisSconceRevealed -= this.SetCanNOTSenseSconce;
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
