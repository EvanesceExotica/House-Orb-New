using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MirzaBeig.ParticleSystems;
public class Sconce : PooledObject, iInteractable
{

    public AudioClip orbInSconceSound;

    public AudioClip orbOutOfSconceSound;

    public AudioClip sconceRevealedSound;
    public AudioSource audioSource;
    public Room parentRoom;

    public ParticleSystem orbAcceptedParticleSystem;
    public ParticleSystems orbOccupiedParticles;
    public ParticleSystems revealedParticles;
    public event Action<MonoBehaviour> Revealed;

    FatherOrb orbPrefab;

    public bool startsWithOrb;

    public void RevealedWrapper(Sconce sconce)
    {
        PlayRevealedParticles();
        if (Revealed != null)
        {
            Revealed(sconce);
        }
    }

    public event Action<MonoBehaviour> Extinguished;

    public void ExtinguishedWrapper(Sconce sconce)
    {
        Debug.Log("This is being triggered somehow");
        StopOccupiedParticles();
        fillStatus = Status.Empty;
        if (Extinguished != null)
        {
            Extinguished(sconce);
        }
    }

    // Use this for initialization
    public static event Action<MonoBehaviour> OrbInSconce;


    public void OrbPlacedInUs(MonoBehaviour ourObject)
    {
        Debug.Log("The orb is placed in us");
        PlayOccupiedParticles();
        PlayEnteredSconceSound();
        fillStatus = Status.HoldingOrb;
        if (OrbInSconce != null)
        {
            OrbInSconce(ourObject);
        }
    }
    public static event Action<Sconce> OrbRemovedFromSconce;

    public void OrbRemovedFromUs(Sconce sconce)
    {
        PlayExitedSconceSound();
        fillStatus = Status.FreshlyLit;
        // ChangeRevealedParticleColor(new Color32(0, 116, 255, 25));
        StartCoroutine(CountdownToEmpty());
        if (OrbRemovedFromSconce != null)
        {
            OrbRemovedFromSconce(sconce);
        }
    }

    float countdownStartTime;
    float countdownDuration = 50.0f;
    public IEnumerator CountdownToEmpty()
    {

        countdownStartTime = Time.time;
        while (Time.time < countdownStartTime + countdownDuration)
        {
            if (fillStatus == Status.HoldingOrb)
            {
                yield break;
            }
            yield return null;
        }
        ExtinguishedWrapper(this);
    }
    public enum Status
    {
        Hidden,
        Empty,

        FreshlyLit,
        HoldingOrb,
    }

    public Status fillStatus;



    public void OnInteractWithMe(Player player)
    {
        if (player.playerState == Player.PlayerState.CarryingOrb)
        {
            OrbPlacedInUs(this);
            //we're reseting the onHoverMe so that the prompt is reset to treat it as if we walked up to it again
            OnHoverMe(player);
        }
        else if (fillStatus == Status.HoldingOrb && (player.playerState != Player.PlayerState.Hiding || player.playerState != Player.PlayerState.Burned || player.playerState != Player.PlayerState.CarryingOrb))
        {
            //if the player is able to receive the orb, isn't carrying it, isn't burned or hiding, the orb will be removed upon interaction
            OrbRemovedFromUs(this);
            //we're reseting the onHoverMe so that the prompt is reset to treat it as if we walked up to it again
            OnHoverMe(player);
        }
    }

    public string orbHeldPrompt;
    public string emptyButOrbCarriedPrompt;

    public void OnHoverMe(Player player)
    {
        if (player.playerState == Player.PlayerState.CarryingOrb)
        {
            player.interactPrompt.DisplayPrompt(emptyButOrbCarriedPrompt, this.gameObject);
            Debug.Log("Press E to place orb in sconce");
        }
        else if (fillStatus == Status.HoldingOrb && player.playerState == Player.PlayerState.NotCarryingOrb)
        {
            player.interactPrompt.DisplayPrompt(orbHeldPrompt, this.gameObject);
            Debug.Log("Press E to take orb from sconce");
        }
    }

    public void OnStopHoverMe(Player player)
    {
        player.interactPrompt.HidePrompt(gameObject);
        //todo: Remove Prompt
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        orbHeldPrompt = " take orb from sconce";
        emptyButOrbCarriedPrompt = " place orb in sconce";
        //TODO: Fix this so that it's an assignment later
        orbOccupiedParticles = transform.GetChild(1).GetComponent<ParticleSystems>();
        revealedParticles = transform.GetChild(0).GetComponent<ParticleSystems>();
        RevealedWrapper(this);
        if (fillStatus != Status.HoldingOrb)
        {
            StopOccupiedParticles();
        }
        if (startsWithOrb)
        {
            GameHandler.fatherOrbGO.transform.parent = transform;
            GameHandler.fatherOrb.transform.position = transform.position;
        }
        FatherOrb.ArrivedAtPreviousSconce += OrbPlacedInUs;
        //fillStatus = Status.Empty;
    }

    void PlayOccupiedParticles()
    {

        orbAcceptedParticleSystem.Play();
        orbOccupiedParticles.Play();
        // ChangeRevealedParticleColor(new Color32(255, 215, 0, 25));
    }

    void PlayEnteredSconceSound()
    {
        if (parentRoom != null)
        {
            float audioScale = ScaleAudio.ScaleAudioByRoomDistance(GameHandler.roomManager.DetermineHowCloseRoomIsToPlayer(parentRoom));
        }
        audioSource.time = 1.0f;
        audioSource.PlayOneShot(orbInSconceSound);
    }

    void PlayExitedSconceSound()
    {
        audioSource.time = 1.0f;
        audioSource.PlayOneShot(orbOutOfSconceSound);
    }

    void PlayRevealedSound()
    {
        audioSource.PlayOneShot(sconceRevealedSound);
    }

    void StopOccupiedParticles()
    {
        orbOccupiedParticles.Stop();
    }

    void PlayRevealedParticles()
    {
        revealedParticles.Play();

    }

    void StopRevealedParticles()
    {
        revealedParticles.Stop();
    }

    void ChangeRevealedParticleColor(Color color)
    {
        revealedParticles.ChangeColor(color);
    }



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayOccupiedParticles();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StopOccupiedParticles();
        }

    }
}
