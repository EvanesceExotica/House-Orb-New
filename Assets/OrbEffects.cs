﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MirzaBeig.ParticleSystems;
public class OrbEffects : MonoBehaviour
{

    [Header("Assign in Inspector")]

    //TODO: Add light changer
    List<ParticleSystems> parryParticles = new List<ParticleSystems>();
    AudioSource source;

    AudioSource corruptionSource;
    public AudioClip corruptionSound;

    public AudioClip ShriekClip; 
    public AudioClip failClip;

    public AudioClip succeedClip;
    public ParticleSystems baseParticleSystem;
    public ParticleSystems FizzingParticleSystems; //ultima looping

    public ParticleSystems parryParticleSystem; //ultima one s hot

    public ParticleSystems closeToHiddenSystem; //

    public ParticleSystems burningSystem;//solar ; 

    public ParticleSystems inRoomWithHiddenSconce; // this one should trigger if you've seen a hint in a memory?

    public ParticleSystems hoveringOverMemory;

    public ParticleSystems nearHiddenSconce;

    public ParticleSystems hintParticleSystems;

    public ParticleSystems autoParryReadySystem;

    public ParticleSystems returnToSconceReadySystem;

    public ParticleSystems autoReflectChargeParticleSystem;

    public ParticleSystems prevSconeTeleportParticleSystem;

    public ParticleSystems refreshGivenParticleSystem;

    public ParticleSystems buffSpinParticleSystem;

    ParticleSystems mainCurrentPlayingSystem;

    public AudioClip currentWoosh;

    public AudioClip standardWoosh;
    public AudioClip panicWoosh;

    public AudioClip repelWoosh;

    float wooshInterval = 1.0f;
    public ParticleSystems failureSystem;
    Light ourLight;
    float defaultIntensity;
    float defaultColor;

    public GameObject parryParticlesGO;

    bool burnedOut;
    void Awake()
    {
        var sources = GetComponents<AudioSource>();
        source = sources[0];
        corruptionSource = sources[1];
        ourLight = GetComponentInChildren<Light>();

        mainCurrentPlayingSystem = baseParticleSystem;

        PromptPlayerHit.PlayerParried += PlayParrySound;
        HiddenSconce.SconceRevealed += ReturnToStandardParticleEffect;
        ReturnPlayerToLastSconce.ArrivedAtLastSconceWithPlayer += ResetSystems;
        PromptPlayerHit.AutoRepelUsed += ReturnToStandardParticleEffect;

        Memory.AutoReflectGiven += StartAutoReflectChargeParticleSystem;
        Memory.RefreshGiven += StartRefreshParticleSystem;
        Memory.PrevSconceTeleportGiven += StartPrevSconceTeleportParticleSystem;
        Memory.HintGiven += StartHintParticleSystem;

        //PromptPlayerHit.PlayerParried += Parry;
        PromptPlayerHit.AutoRepelUsed += Parry;

        FatherOrb.OrbScream += ShriekFromCorruption;

        FatherOrb.Fizzing += StartFizz;
        FatherOrb.RedHot += IncreaseFizzTempo;
        FatherOrb.Critical += IncreaseToSuperPanicWoosh;
        //FatherOrb.Overheated += StopFizz;
        FatherOrb.OrbRefreshed += StopFizz;
        FatherOrb.Dropped += StopFizz;

        //PromptPlayerHit.PlayerFailed += PlayFailureEffect;

        //Room.RoomWithPlayerHit += SetParryParticlesToDoubleSpeed;

        parryParticles.AddRange(parryParticlesGO.GetComponentsInChildren<ParticleSystems>());
        parryParticles.Add(baseParticleSystem);
        parryParticles.Add(parryParticleSystem);
        parryParticles.Add(failureSystem);

        CorruptedObject.Corrupting += PlayCorruptionEffect;
        CorruptedObject.StoppedCorrupting += StopCorruptionEffect;

        StarScream.ScreamHitRoomAdjacent += HideCurrentEffect;
        OrbFire.SoulNotLaunching += ReturnToCurrentEffect;
    }

    void OnDisable()
    {
        FatherOrb.OrbScream -= ShriekFromCorruption;
        PromptPlayerHit.PlayerParried -= PlayParrySound;
        HiddenSconce.SconceRevealed -= ReturnToStandardParticleEffect;
        ReturnPlayerToLastSconce.ArrivedAtLastSconceWithPlayer -= ResetSystems;
        PromptPlayerHit.AutoRepelUsed -= ReturnToStandardParticleEffect;

        Memory.AutoReflectGiven -= StartAutoReflectChargeParticleSystem;
        Memory.RefreshGiven -= StartRefreshParticleSystem;
        Memory.PrevSconceTeleportGiven -= StartPrevSconceTeleportParticleSystem;
        Memory.HintGiven -= StartHintParticleSystem;

        //PromptPlayerHit.PlayerParried += Parry;
        PromptPlayerHit.AutoRepelUsed -= Parry;

        FatherOrb.Fizzing -= StartFizz;
        FatherOrb.RedHot -= IncreaseFizzTempo;
        FatherOrb.Critical -= IncreaseToSuperPanicWoosh;
        //FatherOrb.Overheated += StopFizz;
        FatherOrb.OrbRefreshed -= StopFizz;
        FatherOrb.Dropped -= StopFizz;

        CorruptedObject.Corrupting -= PlayCorruptionEffect;
        CorruptedObject.StoppedCorrupting -= StopCorruptionEffect;

        StarScream.ScreamHitRoomAdjacent -= HideCurrentEffect;
        OrbFire.SoulNotLaunching -= ReturnToCurrentEffect;
    }

    void ShriekFromCorruption(){
        corruptionSource.Stop();
       corruptionSource.clip = ShriekClip;
        corruptionSource.Play();
      //  source.PlayOneShot(ShriekClip);
    }
    void HideCurrentEffect()
    {
        mainCurrentPlayingSystem.Stop();
    }
    void ReturnToCurrentEffect()
    {
        mainCurrentPlayingSystem.Play();
    }

    void PlayParrySound()
    {
        source.PlayOneShot(repelWoosh);
    }
    void CloseInOnOrb()
    {

    }

    void StopAllButFizz()
    {

    }

    void SetParryParticlesToDoubleSpeed(bool held)
    {
        foreach (ParticleSystems system in parryParticles)
        {
            //system.Simulate(1.0f, false);
        }
        foreach (ParticleSystems system in parryParticles)
        {
            // system.SetPlaybackSpeed(2.0f);
        }
    }

    void SetParryParticlesToNormalSpeed()
    {
        foreach (ParticleSystems system in parryParticles)
        {
            //system.SetPlaybackSpeed(1.0f);
        }
    }

    void ResetSystems(MonoBehaviour mono)
    {
        ReturnToStandardParticleEffect();
    }

    void ReturnToStandardParticleEffect()
    {
        //this will return us to the base blue particle system
        if (mainCurrentPlayingSystem != null)
        {
            mainCurrentPlayingSystem.Stop();
        }
        baseParticleSystem.Play();
        mainCurrentPlayingSystem = baseParticleSystem;
        if (burnedOut)
        {
            //if the player has recently failed a parry and burned out the orb
            Sconce.OrbInSconce -= ResetSystems;
        }
    }

    void StartNormalWoosh()
    {
        wooshInterval = 1.0f;
        currentWoosh = panicWoosh;
        canWoosh = true;
        if (!wooshing)
        {
            StartCoroutine(Woosh());
        }
    }
    void IncreaseToPanicWoosh()
    {
        wooshInterval = 0.5f;
    }

    void IncreaseToSuperPanicWoosh()
    {
        wooshInterval = 0.25f;
    }

    bool canWoosh;
    void StopWoosh()
    {
        canWoosh = false;

    }
    bool wooshing = false;
    public IEnumerator Woosh()
    {
        wooshing = true;
        while (canWoosh)
        {
            PlayWooshSound(currentWoosh);
            yield return new WaitForSeconds(wooshInterval);
        }
        wooshing = false;
    }

    void InvokeWoosh(int interval, string methodName)
    {
        InvokeRepeating(methodName, 0, 1.0f);
    }

    void CancelWoosh(string methodName)
    {
        CancelInvoke(methodName);
    }

    void PlayWooshSound(AudioClip wooshClip)
    {
        source.PlayOneShot(wooshClip);
    }

    void PlayPanicWooshSound()
    {
        source.PlayOneShot(panicWoosh);
    }

    void Shake()
    {
        GameHandler.Instance().orbShakeObject.DOShakePosition(1.0f, 0.5f, 3, 90, false, true);
    }

    void Vibrate(){
        GameHandler.Instance().orbShakeObject.DOShakePosition(100f, 0.1f, 3, 90, false, true);
    }

    void StopVibrating(){
        DOTween.Kill(GameHandler.Instance().orbShakeObject);
    }

    public void ResetCorruptionSound()
    {
        corruptionSource.clip = null;
        corruptionSource.pitch = 1.0f;
        corruptionSource.volume = 0.2f;
    }

    public void PlayCorruptionSound(float corruptionLevel)
    {

        corruptionSource.clip = corruptionSound;
        corruptionSource.time = corruptionLevel;
        corruptionSource.volume = 1.0f;
        corruptionSource.Play();
        //source.volume = 0.5f;
        //source.pitch = corruptionLevel/maxCorruption;
        //source.volume = corruptionLevel/maxCorruption;
        //source.DOPitch(source.pitch + 0.5f, 0.5f);
        //source.DOFade(source.volume + 0.15f, 0.5f);
    }

    public void StopCorruptionSound()
    {
        Debug.Log("Corruption whine should stop");
        corruptionSource.Stop();

    }
    void PlayCorruptionEffect()
    {
        Vibrate();
        baseParticleSystem.Stop();
        if (failureSystem != null && failureSystem.isActiveAndEnabled == true)
        {
            failureSystem.SetLoop(true);
            failureSystem.Play();
        }
    }

    void StopCorruptionEffect()
    {
        StopVibrating();
        if (failureSystem != null && failureSystem.isActiveAndEnabled == true)
        {
            failureSystem.SetLoop(false);
            failureSystem.Stop();
        }
        baseParticleSystem.Play();
    }

    void PlayFailureEffect()
    {
        Sconce.OrbInSconce += ResetSystems;
        //TODO: We want maybe a red vingette effect 
        burnedOut = true;
        Shake();
        baseParticleSystem.Stop();
        if (failureSystem != null)
        {
            failureSystem.Play();
        }
        source.PlayOneShot(failClip);
        //Debug.Break();
    }
    void GeneralBuff()
    {
        buffSpinParticleSystem.Play();
    }

    void StartHintParticleSystem(HiddenSconce irrelevant)
    {
        baseParticleSystem.Stop();
        hintParticleSystems.Play();
        mainCurrentPlayingSystem = hintParticleSystems;
    }

    void PlayInRoomWithSconceParticleSystem()
    {

    }

    void StartAutoReflectChargeParticleSystem()
    {
        if (autoReflectChargeParticleSystem != null)
        {
            baseParticleSystem.Stop();
            autoReflectChargeParticleSystem.Play();
            mainCurrentPlayingSystem = autoReflectChargeParticleSystem;
        }
    }

    void StartRefreshParticleSystem()
    {
        refreshGivenParticleSystem.Play();
        //TODO: Maybe all of the memories should refresh time
        //TODO: This one maybe can just be demonstrated by the turn.
    }

    void StartPrevSconceTeleportParticleSystem()
    {
        baseParticleSystem.Stop();
        prevSconeTeleportParticleSystem.Play();
        mainCurrentPlayingSystem = prevSconeTeleportParticleSystem;
    }
    void StartFizz()
    {
        FizzingParticleSystems.Play();
        StartNormalWoosh();

    }

    void StopFizz(UnityEngine.Object ourObject)
    {
        FizzingParticleSystems.Stop();
        canWoosh = false;
    }

    void IncreaseFizzTempo()
    {
        FizzingParticleSystems.SetPlaybackSpeed(2.0f);
        IncreaseToPanicWoosh();
    }
    void ChangeLightIntensity(float intensity, float duration)
    {
        ourLight.DOIntensity(intensity, duration);
    }

    void ChangeLightColor(Color color, float duration)
    {
        ourLight.DOColor(color, duration);
    }

    void Parry()
    {
        parryParticleSystem.Play();
        source.PlayOneShot(succeedClip, 2.0f);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartFizz();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            IncreaseFizzTempo();
        }

    }
}
