using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class HiddenSconce : ParentTrigger
{
    public Room parentRoom;
    public enum SpriteVariations
    {
        GreenPotion,

        PurplePotion,
        FlowerDoll,

        DevilDoll,

    }

    public static event Action InSemiCloseRange;

    void InSemiCloseRangeWrapper()
    {
        if (InSemiCloseRange != null)
        {
            InSemiCloseRange();
        }
    }

    public static event Action OutOfRange;
    void OutOfRangeWrapper()
    {
        if (OutOfRange != null)
        {
            OutOfRange();
        }

    }
    public static event Action InCloseRange;

    void InCloseRangeWrapper()
    {
        if (InCloseRange != null)
        {
            InCloseRange();
        }
    }
    public SpriteVariations ourVariation;
    public Sprite[] ourSprites = new Sprite[4];

    SpriteRenderer spriteRenderer;

    public event Action ThisSconceRevealed;
    public static event Action SconceRevealed;
    public event Action<Sconce, HiddenSconce> NewSconceRevealed;

    public void ThisSconceRevealedWrapper()
    {
        if (ThisSconceRevealed != null)
        {
            ThisSconceRevealed();
        }
    }
    public void NewSconceRevealedWrapper(Sconce sconce, HiddenSconce hiddenSconce)
    {
        if (NewSconceRevealed != null)
        {
            NewSconceRevealed(sconce, hiddenSconce);
        }
    }
    public void SconceWasRevealed()
    {
        if (SconceRevealed != null)
        {
            SconceRevealed();
        }
    }

    float proximityRange = 0.5f;


    public Sconce sconceToReveal;
    public GenerateNewBounds boundsGenerator;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boundsGenerator = GetComponent<GenerateNewBounds>();

        if (spriteRenderer.sprite != null)
        {
            boundsGenerator.GenerateNewColliderSize();
        }

    }
    // Use this for initialization
    void Start()
    {
        proximityRange = 0.5f;

    }

    // Update is called once per frame
    void Update()
    {

    }

    bool orbOverlappingUs = false;

    public override void OnChildTriggerEnter2D(Collider2D hit, GameObject go)
    {
        if (hit.gameObject == GameHandler.fatherOrbGO)
        {
            InSemiCloseRangeWrapper();
        }
    }

    public override void OnChildTriggerExit2D(Collider2D hit, GameObject go)
    {
        if (hit.gameObject == GameHandler.fatherOrbGO)
        {
            OutOfRangeWrapper();
        }
    }
    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject == GameHandler.fatherOrbGO)
        {
            if (GameHandler.fatherOrb.heldStatus == FatherOrb.HeldStatuses.Channeled)
            {
                orbOverlappingUs = true;
                StartCoroutine(BeginDeterminingProximity());
                InCloseRangeWrapper();
            }
        }
    }

    void OnTriggerExit2D(Collider2D hit)
    {
        if (hit.gameObject == GameHandler.fatherOrbGO)
        {
            orbOverlappingUs = false;
        }

    }

    IEnumerator BeginDeterminingProximity()
    {
        //we only want to begin calculating the distance once the transforms are somewhat overlapped
        while (orbOverlappingUs)
        {

            if (Vector2.Distance(GameHandler.fatherOrbGO.transform.position, transform.position) <= proximityRange)
            {
                //TODO: Code in the sconce appearing
                //FOUND IT
                RevealSconce();
                break;
            }
            yield return null;
        }
    }

    void RevealSconce()
    {
        //play some animation	
        Sconce revealedSconce = sconceToReveal.GetPooledInstance<Sconce>();
        revealedSconce.transform.position = transform.position;
        //sconceToRevealGO.SetActive(true);
        //sconceToRevealGO.transform.parent = null;
        revealedSconce.OrbPlacedInUs(revealedSconce);
        SconceWasRevealed();
        NewSconceRevealedWrapper(revealedSconce, this);
        gameObject.SetActive(false);


    }
}
