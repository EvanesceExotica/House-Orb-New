using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Com.LuisPedroFonseca.ProCamera2D;
public class ReturnPlayerToLastSconce : MonoBehaviour
{

	public Transform playerHoldSpot;
    public Memory.BuffGiven givenBuff;
    public static event Action<MonoBehaviour> ReturningToLastSconceWithPlayer;

    public static event Action<MonoBehaviour> ArrivedAtLastSconceWithPlayer;

    bool canReturn;
    ProCamera2D ourCamera;

    bool orbInPlayersHands;


    void Awake()
    {
        ourCamera = Camera.main.GetComponent<ProCamera2D>();
        Memory.PrevSconceTeleportGiven += SetCanReturn;
        FatherOrb.ArrivedAtPreviousSconce += ArrivedAtLastSconceWithPlayerWrapper;
        OrbController.ChannelingOrb += SetOrbNotInPlayersHands;
        OrbController.ManuallyStoppedChannelingOrb += SetOrbInPlayersHands;
        FatherOrb.Dropped += SetOrbNotInPlayersHands;
        FatherOrb.PickedUp += SetOrbInPlayersHands;
    }
    void ReturningToLastSconceWithPlayerWrapper()
    {
        if (canReturn)
        {
            if (ReturningToLastSconceWithPlayer != null)
            {
                ReturningToLastSconceWithPlayer(this);
            }
            GameHandler.fatherOrb.ReturnToLastSconceEarlyWrapper();
			//the player technically stays in the same location, but need to make sure not targeted by enemy
			ourCamera.RemoveAllCameraTargets();
            GameHandler.playerGO.layer = LayerMask.NameToLayer("OrbMovement");
            //ourCamera.RemoveCameraTarget(GameHandler.playerGO.transform);
            ourCamera.AddCameraTarget(GameHandler.fatherOrbGO.transform);
        }
    }

    public void ArrivedAtLastSconceWithPlayerWrapper(MonoBehaviour ourObject)
    {
        if (canReturn)
        {
            GameHandler.playerGO.transform.position = GameHandler.fatherOrbGO.transform.position;
            if (ArrivedAtLastSconceWithPlayer != null)
            {
                ArrivedAtLastSconceWithPlayer(this);
            }
            GameHandler.playerGO.layer = GameHandler.defaultPlayerLayer;
            ourCamera.RemoveCameraTarget(GameHandler.fatherOrbGO.transform);
            ourCamera.AddCameraTarget(GameHandler.playerGO.transform);
            SetCANTReturn();
        }
    }

    void SetCanReturn()
    {
        canReturn = true;
    }


    void SetOrbNotInPlayersHands(MonoBehaviour ourObject){
        orbInPlayersHands = false;
    }
    void SetOrbInPlayersHands(MonoBehaviour ourObject){
       orbInPlayersHands = true; 
    }

    void SetCANTReturn()
    {
        canReturn = false;
    }
    // Use this for initialization
    void Start()
    {
       // SetCanReturn();

    }

    // Update is called once per frame
    void Update()
    {
        if (canReturn && orbInPlayersHands && Input.GetKeyDown(KeyCode.X))
        {
            //TODO: Connect everythign
            ReturningToLastSconceWithPlayerWrapper();

            //	GameHandler.fatherOrb.ReturnToLastSconceEarlyWrapper() ;
        }

    }
}
