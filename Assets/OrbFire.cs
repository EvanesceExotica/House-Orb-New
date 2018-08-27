using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MirzaBeig.ParticleSystems;
using System.Linq;
public class OrbFire : MonoBehaviour
{

    //Variables
    #region

    public static event Action CorrectSoulChosen;

    public void CorrectSoulChosenWrapper(){
        if(CorrectSoulChosen != null){
            CorrectSoulChosen();
        }
    }

    public static event Action WrongOrNoSoulChosen;

    public void WrongOrNoSoulChosenWrapper(){
        if(WrongOrNoSoulChosen != null){
            WrongOrNoSoulChosen();
        }
    }
    public static event Action<GameObject> SoulToBeLaunched;
    public static event Action OrbPriming;
    public static event Action DonePriming;
    bool zoomed;
    #endregion

    //Actions and Action methods
    #region
    public void PrimingOrb()
    {
        if (OrbPriming != null)
        {
            OrbPriming();
        }
    }
    public void DonePrimingOrb()
    {
        if (DonePriming != null)
        {
            DonePriming();
        }
    }
    public static event Action SoulNotLaunching;

    public void LaunchingSoul(GameObject launchedSoul)
    {
        if (SoulToBeLaunched != null)
        {
            SoulToBeLaunched(launchedSoul);
        }
    }

    public void NotLaunchingSoul()
    {
        if (SoulNotLaunching != null)
        {
            SoulNotLaunching();
        }
    }
    #endregion



    void Start()
    {
        elasticity = 10.0f;
    }
    bool launching;
    public bool priming = false;
    float minimumHoldDuration = 1.0f;
    Vector2 mouseStartPosition;
    float maxPullDistance;
    public float elasticity;
    LineRenderer slingshotLineRenderer;
    bool stillHeld = false;
    float holdStartTime;//
    Rigidbody2D rb;
    //References to other scripts
    #region

    LineRenderer orbLaunchLineRenderer;
    Rigidbody2D soulRigidbody;

    ParticleSystems soulParticleSystems;

    Collider2D ourCollider;
    #endregion 

    void Awake()
    {

        orbLaunchLineRenderer = GetComponent<LineRenderer>();
        orbLaunchLineRenderer.enabled = false;
        soulRigidbody = GetComponent<Rigidbody2D>();
        soulParticleSystems = GetComponentInChildren<ParticleSystems>();
        ourCollider = GetComponent<Collider2D>();
        ourCollider.enabled = false;
    }
    public IEnumerator PrimeSlingshot()
    {

        soulParticleSystems.Play();
        PrimingOrb();
        priming = true;
        Vector2 mouseStartPosition = Input.mousePosition;
        float distance = 0;
        Vector2 direction = new Vector2(0, 0);

        orbLaunchLineRenderer.enabled = true;
        ourCollider.enabled = true;
        FreezeTime.SlowdownTime(0.10f);
        while (Time.time < holdStartTime + 1)
        {
            Debug.Log("Priming");
            if (Input.GetMouseButtonUp(0))
            {
                break;
            }
            if (Input.GetMouseButtonDown(1))
            {
                //right click to cancel
                //stillHeld = true;
                orbLaunchLineRenderer.enabled = false;
                yield break;
            }
            Vector3 transformPosition = new Vector3(transform.position.x, transform.position.y, 0);
            orbLaunchLineRenderer.SetPosition(0, transformPosition);
            Vector3 screenToWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
            screenToWorldPoint.z = 0;
            orbLaunchLineRenderer.SetPosition(1, screenToWorldPoint);
            yield return null;
        }
        orbLaunchLineRenderer.enabled = false;

        Vector2 mousePos = Input.mousePosition;
        Vector2 mousePositionWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));

        distance = Vector2.Distance(transform.position, mousePositionWorld);
        direction = (Vector2)((Vector2)transform.position - mousePositionWorld);


        float velocity = distance * Mathf.Sqrt(elasticity / soulRigidbody.mass);
        velocity *= (10); //multiply to cancel out low timescale
        soulRigidbody.isKinematic = false;
        soulRigidbody.velocity = (direction.normalized * velocity);

        //Debug.Log(pReference.rb.velocity);
        priming = false;
        //want the collider on now so it can impact with the ui collider
        DonePrimingOrb();
        launching = true;
        LaunchingSoul(this.gameObject);
        StartCoroutine(CountdownFromLaunch());
        //   StartCoroutine(PlotPath());
    }
    // Use this for initialization


    void ResetTimeAndSetLaunchToFalse()
    {
        FreezeTime.StartTimeAgain();
        // ZoomOut();
        launching = false;
        NotLaunchingSoul();
        GameHandler.prompter.PlayerMissedOrFailed();
        WrongOrNoSoulChosenWrapper();
        gameObject.SetActive(false);
    }
    IEnumerator CountdownFromLaunch()
    {
        FreezeTime.StartTimeAgain();
        yield return new WaitForSeconds(0.5f);
        soulParticleSystems.Stop();
        ResetTimeAndSetLaunchToFalse();
    }


    public void DetermineWhichSoulWasHit(Soul soul)
    {
       Dissolve(); 
        if (soul.chosen)
        {
            GameHandler.prompter.PlayerParriedScream();
            CorrectSoulChosenWrapper();
        }

        else{
            GameHandler.prompter.PlayerMissedOrFailed();
            WrongOrNoSoulChosenWrapper();
        }
    }
    void Dissolve()
    {
        soulParticleSystems.Stop();
    }


    void ReturnToPlayer()
    {
        // ourProCamera2D.RemoveCameraTarget(darkStar.transform);
        // ourProCamera2D.AddCameraTarget(player.transform);
        // focusedOnDarkStar = false;
    }
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            holdStartTime = Time.time;
            StartCoroutine(PrimeSlingshot());

        }
    }

}
