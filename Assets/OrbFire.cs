using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class OrbFire : MonoBehaviour {

  //Variables
    #region

    bool poweredUp;

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
    #endregion 
	public IEnumerator PrimeSlingshot(GameObject whichSoul)
    {

        PrimingOrb();
        if (!zoomed)
        {
          //  ZoomOnPlayer();
        }
        priming = true;
        Vector2 mouseStartPosition = Input.mousePosition;
        float distance = 0;
        Vector2 direction = new Vector2(0, 0);

        orbLaunchLineRenderer = whichSoul.GetComponentInChildren<LineRenderer>();
        orbLaunchLineRenderer.enabled = true;
        FreezeTime.SlowdownTime(0.10f);
        while (true)
        {

            if (Input.GetMouseButtonUp(1))
            {
                break;
            }
            if (Input.GetMouseButtonDown(0))
            {
                //right click to cancel
                stillHeld = true;
                orbLaunchLineRenderer.enabled = false;
                yield break;
            }
            orbLaunchLineRenderer.SetPosition(0, whichSoul.transform.position);
            orbLaunchLineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
            yield return null;
        }
        orbLaunchLineRenderer.enabled = false;

        Vector2 mousePos = Input.mousePosition;
        Vector2 mousePositionWorld = Camera.main.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));

        Rigidbody2D soulRigidbody = whichSoul.GetComponent<Rigidbody2D>();
        distance = Vector2.Distance(whichSoul.transform.position, mousePositionWorld);
        direction = (Vector2)((Vector2)whichSoul.transform.position - mousePositionWorld);


        float velocity = distance * Mathf.Sqrt(elasticity / soulRigidbody.mass);
        velocity *= (10); //multiply to cancel out low timescale
        soulRigidbody.isKinematic = false;
        soulRigidbody.velocity = (direction.normalized * velocity);

        //Debug.Log(pReference.rb.velocity);
        priming = false;
        //want the collider on now so it can impact with the ui collider
        DonePrimingOrb();
        launching = true;
        LaunchingSoul(whichSoul);
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
    }
    IEnumerator CountdownFromLaunch()
    {

        yield return new WaitForSeconds(0.5f * 0.1f);
        ResetTimeAndSetLaunchToFalse();
    }

  

   

    void ReturnToPlayer()
    {
        // ourProCamera2D.RemoveCameraTarget(darkStar.transform);
        // ourProCamera2D.AddCameraTarget(player.transform);
        // focusedOnDarkStar = false;
    }
    void Update()
    {
        if (Input.GetMouseButton(1) && poweredUp)
        {
            holdStartTime = Time.time;
            StartCoroutine(PrimeSlingshot(ourSoulHandler.soulChargingUs));

        }
    }
	
}
