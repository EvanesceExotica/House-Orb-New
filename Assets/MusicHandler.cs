using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicHandler : MonoBehaviour
{

    public float fadeDuration = 1.0f;

    public AudioClip screamMusic;
    public AudioSource musicSource;
    public AudioClip normalMusic;

    public AudioClip memoryMusic;

    public AudioClip chaseMusic;

	public AudioClip channelingMusic;
    // Use this for initialization
    void Awake()
    {
        musicSource = GetComponent<AudioSource>();
        //SetNormalMood();
		ChangeClip(Mood.Normal);
		PromptPlayerHit.PlayerFailed += SetChasedMood;
        PromptPlayerHit.PlayerFailed += StopScreamDrone;

        PromptPlayerHit.PlayerParried += SetNormalMood;
        PromptPlayerHit.PlayerParried += StopScreamDrone;

        Monster.BackToSearching += SetNormalMood;
        StarScream.ScreamBegun += PlayScreamDrone;

        OrbController.ChannelingOrb += SetChannelingMusic;
        //TODO: Test that the scream isn't happening before setting back to normal for example
        OrbController.SconceRevealedStoppedChannelingOrb += SetNormalMood;
        OrbController.ManuallyStoppedChannelingOrb += SetNormalMood;

    }

    void SetChannelingMusic(MonoBehaviour mono){
        //different channeling music while being chased
        GameHandler.fader.Play(channelingMusic);
        //ChangeClip(Mood.Channeling);
    }

    void PlayScreamDrone(){
        GameHandler.screamSoundObjectSource.clip = screamMusic;
        GameHandler.screamSoundObjectSource.Play();
        //musicSource.PlayOneShot(screamMusic);
    }

    void StopScreamDrone(){
        GameHandler.screamSoundObjectSource.Stop();
    }

	void SetChasedMood(){
        GameHandler.fader.Play(chaseMusic);
		//ChangeClip(Mood.Chased);
	}

    void SetNormalMood(MonoBehaviour mono){

        GameHandler.fader.Play(normalMusic);
        //ChangeClip(Mood.Normal);
    }

    void SetNormalMood(){
        Debug.Log("Should be back to normal");
        ChangeClip(Mood.Normal);
    }

    // void SetNormalMood(){
    // 	ourMood = Mood.Normal;
    // 	musicSource.volume = 0;
    // 	musicSource.clip = normalMusic;
    // 	FadeInMusic();
    // }

    void ChangeClip(Mood moodToChangeTo)
    {
        FadeOutMusic();
        ourMood = moodToChangeTo;
        if (moodToChangeTo == Mood.Chased)
        {
            musicSource.clip = chaseMusic;
        }
        else if (moodToChangeTo == Mood.Normal)
        {
			musicSource.clip = normalMusic; 
        }
        else if (moodToChangeTo == Mood.Channeling)
        {
			musicSource.clip = channelingMusic;
        }
        FadeInMusic();
    }

    public enum Mood
    {

        Normal,
        Chased,
        Memory,
        ScreamComing,
        Channeling,
        AllSconcesLit
    }

    public Mood ourMood;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FadeOutMusic()
    {
        musicSource.DOFade(0f, fadeDuration);
    }

    void FadeInMusic()
    {
		musicSource.Play();
        musicSource.DOFade(1.0f, fadeDuration);

    }
}
