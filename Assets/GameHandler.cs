using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using TMPro;
public class GameHandler : MonoBehaviour
{

    public static ScreamFollowObject screamFollowObject;
    public static RoomManager roomManager;
    public static LayerMask defaultPlayerLayer;
    public static Player player;
    public static GameObject playerGO;

    public static FatherOrb fatherOrb;

    public static GameObject managerObject;
    public static GameObject fatherOrbGO;

    public static Monster monster;
    public static GameObject monsterGO;

    public static ProCamera2D proCamera;

    public static Camera mainCamera;

    public static Transform fatherOrbHoldTransform;

    public static Transform rightFacingFatherOrbTransform;

    public static Transform leftFacingFatherOrbTransform;

    public static OrbController orbController;

    public static OrbEffects orbEffects;
    public static Transform bubbleLineStartTransform;

    public static AudioSource screamSoundObjectSource;

    public static Transform breathCanvas;

    public static DialogueDisplayer dialogueDisplayer;    

    public static CrossFade fader;
    void Awake()
    {
        dialogueDisplayer = GameObject.Find("DialogueDisplayer").GetComponent<DialogueDisplayer>();
        screamSoundObjectSource = GameObject.Find("ScreamSound").GetComponent<AudioSource>();
        screamFollowObject = GameObject.Find("ScreamFollowObject").GetComponent<ScreamFollowObject>();

        rightFacingFatherOrbTransform = GameObject.Find("RightFaceFatherOrbPos").transform;
        leftFacingFatherOrbTransform = GameObject.Find("LeftFaceFatherOrbPos").transform;

        fatherOrbHoldTransform = GameObject.Find("RightFaceFatherOrbPos").transform;

        bubbleLineStartTransform = GameObject.Find("LineStartPosition").transform;
        proCamera = Camera.main.GetComponent<ProCamera2D>();
        mainCamera = Camera.main;
        managerObject = GameObject.Find("Managers");
       fader = managerObject.GetComponent<CrossFade>(); 
        roomManager = GameObject.Find("Managers").GetComponent<RoomManager>();
        playerGO = GameObject.Find("Player");
        player = playerGO.GetComponent<Player>();
        defaultPlayerLayer = playerGO.layer;
        fatherOrbGO = GameObject.Find("FatherOrb");
        fatherOrb = fatherOrbGO.GetComponent<FatherOrb>();
        monsterGO = GameObject.Find("Monster");
        orbController = fatherOrbGO.GetComponent<OrbController>();
        orbEffects = fatherOrbGO.GetComponent<OrbEffects>();
        if (monsterGO != null)
        {
            monster = monsterGO.GetComponent<Monster>();
        }
        breathCanvas = playerGO.transform.Find("BreathCanvas");
        Monster.MonsterReachedPlayer += GameOver;
    }

    public static void SwitchOrbHoldPositions(bool facingRight)
    {
        if (facingRight == true)
        {
            fatherOrbHoldTransform = rightFacingFatherOrbTransform;
            Debug.Log("We're switching positions to right! " + fatherOrbHoldTransform.name);
        }
        else
        {
            fatherOrbHoldTransform = leftFacingFatherOrbTransform;
            Debug.Log("We're switching positions to left! " + fatherOrbHoldTransform.name);
        }
    }
    // Use this for initialization

    void GameOver()
    {
        Debug.Log("GAme over :O");
    }

}
