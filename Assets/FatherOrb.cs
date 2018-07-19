using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class FatherOrb : MonoBehaviour//, iInteractable
{

    Transform FatherOrbPos;
    GameObject player;
    [SerializeField] Sconce previousSconce;
    [SerializeField] Sconce currentSconce;

    [SerializeField] float corruptionMeter = 0;

    [SerializeField] float maxCorruption = 19.0f;

    float durationBeforeCritical;
    public static event Action<MonoBehaviour> MovingBetweenPlayerAndObject;

    void MovingBetweenPlayerAndObjectWrapper(MonoBehaviour mono)
    {
        if (MovingBetweenPlayerAndObject != null)
        {
            MovingBetweenPlayerAndObject(mono);
        }
    }

    public static event Action<MonoBehaviour> StoppedMovingBetweenPlayerAndObject;

    void StoppedMovingBetweenPlayerAndObjectWrapper(MonoBehaviour mono)
    {
        if (StoppedMovingBetweenPlayerAndObject != null)
        {
            StoppedMovingBetweenPlayerAndObject(mono);
        }
    }
    public static event Action<MonoBehaviour> PickedUp;

    public static event Action<MonoBehaviour> Dropped; //as soon as the orb burns the player and leaves their hands, this should trigger

    public static event Action<MonoBehaviour> Placed;
    public static event Action<MonoBehaviour> ArrivedAtPreviousSconce;


    void ArrivedAtPreviousSconceWrapper(Sconce sconce)
    {
        if (ArrivedAtPreviousSconce != null)
        {
            ArrivedAtPreviousSconce(sconce);
        }
    }

   
    void PlayerPlacedOrb(MonoBehaviour mono)
    {
        if (Dropped != null)
        {
            Dropped(this);
        }
    }
    void PlayerDroppedOrb(MonoBehaviour mono)
    {
        heldStatus = HeldStatuses.Travelling;
        if (Dropped != null)
        {
            Dropped(this);
        }
    }


    public static event Action Fizzing;

    void OrbFizzing()
    {
        if (Fizzing != null)
        {
            Fizzing();
        }
    }
    public static event Action RedHot;

    void OrbRedHot()
    {
        if (RedHot != null)
        {
            RedHot();
        }
    }

    public static event Action Critical;

    void OrbCritical(){
        if(Critical != null){
            Critical();
        }
    }
    public static event Action Overheated;

    public static event Action<UnityEngine.Object> OrbRefreshed;

    void OrbRefreshedWrapper()
    {
        if (OrbRefreshed != null)
        {
            OrbRefreshed(this);
        }
    }
    void OrbOverheated()
    {
        PlayerDroppedOrb(this);
        StartCoroutine(ReturnToLastSconce());
        if (Overheated != null)
        {
            Overheated();
        }
        instabilityStatus = InstabilityStatus.NotPickedUp;
    }

    [SerializeField] bool movingToObject;

    Vector2 tempPos = new Vector2();
    public Vector2 posOffset = new Vector2();

    float frequency = 1.0f;

    float amplitude = 0.1f;

    void FloatMe()
    {
        if ((heldStatus == HeldStatuses.Carried && !movingToObject && !flipping /*GameHandler.player.movement.flipping*/) || (inSconce && !movingToObject) /*||  /*!GameHandler.player.movement.flipping*/ )
        {
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.localPosition = tempPos;
        }
        if (flipping)
        {
            if (transform.localPosition.x == GameHandler.fatherOrbHoldTransform.localPosition.x)
            {
                flipping = false;
            }
        }

    }

    bool flipping;
    public void HandleFlip()
    {
        flipping = true;
        if (heldStatus == HeldStatuses.Carried)
        {
            float newX = GameHandler.fatherOrbHoldTransform.localPosition.x;
            Vector2 newPosition = new Vector2(newX, transform.localPosition.y);
            transform.localPosition = newPosition;
            posOffset = transform.localPosition;
        }
        // 
    }


    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        player = GameHandler.playerGO;
        FatherOrbPos = player.transform.Find("FatherOrbPos");
        durationHeld = 35.0f;
        durationBeforeFizzing = 25.0f;
        durationBeforeRedHot = 30.0f;
        durationBeforeCritical = 33.0f;
        Memory.RefreshGiven += RefreshTime;
        Sconce.OrbInSconce += this.PlayerDroppedOrb;
        Sconce.OrbInSconce += this.EnteredSconce;
        Sconce.OrbRemovedFromSconce += this.PickedUpByPlayer;
        //you want a successful parry to refresh the orb time -- maybe a failure should too so it's not a double fail? Or have the failure make the orb return to sconce early w/ out burn
        PromptPlayerHit.PlayerParried += RefreshTime;
        PromptPlayerHit.PlayerFailed += FailureDelayWrapper;
        //TODO: Fix the below
        OrbController.ManuallyStoppedChannelingOrb += PickedUpByPlayer;
        //TODO: Add for if the orb remains charged w/ power up even when it is not in the player's hands
        if (transform.parent != null)
        {
            Sconce sconce = transform.parent.GetComponent<Sconce>();
            sconce.OrbPlacedInUs(sconce);
            SetZToNegative2();
        }
        CorruptedObject.Corrupting += BeCorrupted;
        CorruptedObject.StoppedCorrupting += SetCorruptionSourceRemoved;
        //posOffset = transform.position;
        //SetInSconce(transform.parent.gameObject);
    }

    bool beingCorrupted;
    void SetCorruptionSourceRemoved()
    {
        beingCorrupted = false;
    }

    void BeCorrupted()
    {
        if (!beingCorrupted && !coolingDownFromCorruption)
        {
            StartCoroutine(IncreaseCorruptionMeter());
        }
    }

    public Image corruptionImage;

    public IEnumerator IncreaseCorruptionMeter()
    {
        Debug.Log("This corruption meter is increasing");
        beingCorrupted = true;
        GameHandler.orbEffects.PlayCorruptionSound(corruptionMeter);
        bool buildUpStopped = false;
        while (corruptionMeter <= maxCorruption && corruptionMeter >= 0)
        {
            //this should only end if the corruption meter has had time to return to zero, or hits max 
            if (!beingCorrupted)
            {

                if (!buildUpStopped)
                {
                    Debug.Log("Stopped being corrupted");
                    GameHandler.orbEffects.StopCorruptionSound();
                    buildUpStopped = true;
                }
                if (inSconce)
                {
                    corruptionMeter -= 3.0f;
                    //corruptionImage.fillAmount -= 3.0f;
                }
                else
                {
                    corruptionMeter -= 1.0f;
                    //corruptionImage.fillAmount -= 1.0f;

                }
                if (corruptionMeter <= 0)
                {
                    break;
                }
            }
            else
            {
                if (buildUpStopped)
                {
                    Debug.Log("Started being corrupted again");
                    GameHandler.orbEffects.PlayCorruptionSound(corruptionMeter);
                    buildUpStopped = false;
                }
                corruptionMeter += 1.0f;
                if (corruptionMeter >= maxCorruption)
                {
                    break;
                }
                //corruptionImage.fillAmount += 1.0f;
            }
            //corruptionMeter += 3.0f;
            //GameHandler.orbEffects.PlayCorruptionSound(corruptionMeter);
            yield return new WaitForSeconds(0.5f);
        }
        if (corruptionMeter >= maxCorruption)
        {
            corruptionMeter = maxCorruption;
            OrbScreamWrapper();
            beingCorrupted = false;
        }
        else if (corruptionMeter <= 0)
        {
            corruptionMeter = 0;
            beingCorrupted = false;
        }
        StartCoroutine(CooldownFromCorruption());
        GameHandler.orbEffects.StopCorruptionSound();
    }

    public bool coolingDownFromCorruption;

    IEnumerator CooldownFromCorruption()
    {
        corruptionMeter = 0;
        coolingDownFromCorruption = true;
        yield return new WaitForSeconds(10.0f);
        coolingDownFromCorruption = false;

    }

    void FailureDelayWrapper()
    {
        StartCoroutine(FailureDelay());
    }

    public IEnumerator FailureDelay()
    {
        yield return new WaitForSeconds(1);
        ReturnToLastSconceEarlyWrapper();
    }

    void SetZToNegative2()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -2);
    }

    void SetInSconce(Sconce sconce)
    {
        //sconce.GetComponent<Sconce>().OrbPlacedInUs();
        heldStatus = HeldStatuses.InSconce;
        currentSconce = sconce;
        inSconce = true;
        transform.parent = sconce.transform;
        //transform.localPosition = Vector3.zero;
        SetZToNegative2();
        instabilityStatus = InstabilityStatus.NotPickedUp;
    }
    public float durationHeld;
    public float heldStartTime;
    public float elapsedTime;
    public enum HeldStatuses
    {
        InSconce,
        Travelling,
        Channeled,
        Carried
    }
    public HeldStatuses heldStatus;

    float durationBeforeFizzing;
    float durationBeforeRedHot;

    public enum InstabilityStatus
    {
        NotPickedUp,
        FreshPickedUp,
        Fizzing,

        RedHot,

        _Critical,
        Screaming

    }

    public InstabilityStatus instabilityStatus;
    bool inSconce;
    public void EnteredSconce(MonoBehaviour sconce)
    {
        // PlayerDroppedOrb();
        SetInSconce((Sconce)sconce);
        MonoBehaviour mono = (MonoBehaviour)sconce;
        if (Vector2.Distance(transform.position, mono.transform.position) > 0.1f)
        {
            //TODO: Put this back in
            StartCoroutine(MoveUs(transform.position, sconce.transform.position, sconce));
        }
    }



    public void SetOrbBeingChanneled()
    {
        //channeling should add a small time boost
        //a small animation should play around the orb to show time's been added to it 
        changeElapsedTime(5.0f);
        heldStatus = HeldStatuses.Channeled;
    }

    public void PickedUpByPlayer(MonoBehaviour ourObject)
    {
        heldStatus = HeldStatuses.Carried;
        inSconce = false;
        if (ourObject.GetComponent<Sconce>() != null)
        {
            previousSconce = currentSconce;
            currentSconce = null;
        }
        instabilityStatus = InstabilityStatus.FreshPickedUp;
        StartCoroutine(MoveUs(transform.position, GameHandler.fatherOrbHoldTransform.position, GameHandler.player));
        transform.parent = player.transform;
        //posOffset = transform.localPosition;
        if (PickedUp != null)
        {
            PickedUp(this);
        }
        StartCoroutine(BeingCarried());
    }

    public void changeElapsedTime(float amount)
    {
        if (carriedOrChanneled)
        {
            //if the elapsed time of the orb being carried or channeled is ticking down, add or remove an amount from it
            elapsedTime += amount;
        }
    }

    bool carriedOrChanneled;
    public IEnumerator BeingCarried()
    {
        //this coroutine handles the orb being carried by the player
        carriedOrChanneled = true;
        heldStartTime = Time.time;

        while (elapsedTime < durationHeld)
        {
            FloatMe();
            

            if (timeRefreshed)
            {
                //if something triggered the time to refresh, like the "memory" that has this function
                elapsedTime = 0;
                instabilityStatus = InstabilityStatus.FreshPickedUp;
                OrbRefreshedWrapper();
                timeRefreshed = false;
            }
            if (inSconce)
            {
                //if the player places the orb in a sconce, reset and break free
                elapsedTime = 0;
                yield break;
            }
            if (cancelCarryCoroutineEarly)
            {
                //if the coroutine was cancelled early, like by the function that allows the player to return to a previous sconce
                elapsedTime = 0;
                yield break;
            }

            if (elapsedTime >= durationBeforeFizzing && instabilityStatus != InstabilityStatus.Fizzing)
            {

                instabilityStatus = InstabilityStatus.Fizzing;
                OrbFizzing();
            }
            if (elapsedTime >= durationBeforeRedHot && instabilityStatus != InstabilityStatus.RedHot)
            {
                instabilityStatus = InstabilityStatus.RedHot;
                OrbRedHot();

            }
            if(elapsedTime >= durationBeforeCritical && instabilityStatus != InstabilityStatus._Critical){
                instabilityStatus = InstabilityStatus._Critical;
                OrbCritical();
            }
            if (!movingToObject)
            {
                //we want to pause the timer while the object is being moved from a sconce or to a memory

                elapsedTime += Time.deltaTime;
            }
            yield return null;
        }
        elapsedTime = 0;
        carriedOrChanneled = false;
        if (heldStatus == HeldStatuses.Carried)
        {
            //if the player was carrying the orb when time ran out
            OrbOverheated();
        }
        else if (heldStatus == HeldStatuses.Channeled)
        {
            //if the player was channeling the orb when time ran out
            //OrbScreamWrapper();
        }

    }


    public static event Action OrbScream;

    void OrbScreamWrapper()
    {
        if (OrbScream != null)
        {
            OrbScream();
        }
    }

    public void MoveUsWrapper(Vector2 startingPosition, Vector2 destination, MonoBehaviour destinationObject)
    {
        if (!movingToObject)
        {
            StartCoroutine(MoveUs(startingPosition, destination, destinationObject));
        }
    }

    public void ChangeHoldDuration(float addedDuration)
    {
        durationHeld += addedDuration;
        Debug.Log("New orb held duration " + durationHeld);
    }

    public IEnumerator MoveUs(Vector2 startingPosition, Vector2 destination, MonoBehaviour destinationObject)
    {

        movingToObject = true;
        MovingBetweenPlayerAndObjectWrapper(this);
        transform.parent = destinationObject.transform;
        while (Vector2.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, 5 * Time.deltaTime);
            yield return null;

        }
        if (destinationObject.GetType() == typeof(Sconce))
        {
            transform.localPosition = Vector2.zero;
        }
        posOffset = transform.localPosition;
        movingToObject = false;
        StoppedMovingBetweenPlayerAndObjectWrapper(this);
        SetZToNegative2();
    }

    bool cancelCarryCoroutineEarly;
    public void ReturnToLastSconceEarlyWrapper()
    {
        if (heldStatus == HeldStatuses.Carried)
        {
            cancelCarryCoroutineEarly = true;
            PlayerDroppedOrb(this);
            StartCoroutine(ReturnToLastSconce());
        }
    }

    bool timeRefreshed;
    public void RefreshTime()
    {
        timeRefreshed = true;
    }

    public IEnumerator ReturnToLastSconce()
    {
        yield return StartCoroutine(MoveUs(transform.position, previousSconce.transform.position, previousSconce));
        ArrivedAtPreviousSconceWrapper(previousSconce);
        //SetInSconce(previousSconce);
        // currentSconce = previousSconce;
        //transform.parent = currentSconce.gameObject.transform;

    }
    MeshRenderer renderer;
    public string sortingLayerName;        // The name of the sorting layer .
    public int sortingOrder;            //The sorting order

    void Start()
    {
        // Set the sorting layer and order.
        renderer.sortingLayerName = sortingLayerName;
        renderer.sortingOrder = sortingOrder;
    }

    void Update()
    {
        if (inSconce && !movingToObject)
        {
            FloatMe();
        }
    }
    // Use this for initialization

    // Update is called once per frame

}
