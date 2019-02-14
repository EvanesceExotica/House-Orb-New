using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
public class GameHandler : MonoBehaviour
{

    public static GameHandler handlerInstance;
    public static GameHandler Instance() { return handlerInstance; }
    public PromptPlayerHit prompter;
    public ScreamFollowObject screamFollowObject;
    public RoomManager roomManager;
    public LayerMask defaultPlayerLayer;
    public Player player;
    public GameObject playerGO;

    public FatherOrb fatherOrb;

    public GameObject managerObject;
    public GameObject fatherOrbGO;

    public Monster monster;
    public GameObject monsterGO;

    public ProCamera2D proCamera;

    public Camera mainCamera;

    public Transform fatherOrbHoldTransform;

    public Transform rightFacingFatherOrbTransform;

    public Transform leftFacingFatherOrbTransform;

    public OrbController orbController;

    public OrbEffects orbEffects;
    public Transform bubbleLineStartTransform;

    public AudioSource screamSoundObjectSource;

    public Transform breathCanvas;

    public DialogueDisplayer dialogueDisplayer;

    public CrossFade fader;

    public Transform orbShakeObject;

    public List<IPausable> pausableObjects = new List<IPausable>();

    public Camera CutsceneCamera;
    public CanvasGroup FadeToBlackGroup;
    void Awake()
    {
        if (!handlerInstance)
        {
            handlerInstance = this;
        }
        var pausables = FindObjectsOfType<MonoBehaviour>().OfType<IPausable>();
        foreach(IPausable p in pausables){
            pausableObjects.Add(p);
        }
        var cameras = Resources.FindObjectsOfTypeAll<Camera>();
        foreach (Camera camera in cameras){
            if(camera.gameObject.name == "Cutscene Camera"){
                CutsceneCamera = camera;
                break;
            }
        }
        FadeToBlackGroup = GameObject.Find("DialogueCanvas").GetComponent<CanvasGroup>();
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
        prompter = player.GetComponentInChildren<PromptPlayerHit>();
        defaultPlayerLayer = playerGO.layer;
        fatherOrbGO = GameObject.Find("FatherOrb");
        orbShakeObject = fatherOrbGO.transform.GetChild(0);
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
    void Start()
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
        prompter = player.GetComponentInChildren<PromptPlayerHit>();
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
    public void SwitchOrbHoldPositions(bool facingRight)
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

    bool gameplayPaused;
    public void PauseGameplay()
    {
        //This shouldn't pause everything, just "gameplay". Doesn't pause cinematics.
        foreach (IPausable pausableObject in pausableObjects)
        {
            pausableObject.PauseMe();
        }
        gameplayPaused = true;
    }

    public void UnpauseGameplay()
    {
        foreach (IPausable pausableObject in pausableObjects)
        {
            pausableObject.UnpauseMe();
        }
        gameplayPaused = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            //RestartScene();
            if (!gameplayPaused)
            {
                PauseGameplay();

            }
            else
            {
                UnpauseGameplay();
            }

        }
    }
    void RestartScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

}
