using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;
public class HidingSpace : MonoBehaviour, iInteractable
{
 
 #region 
    CanvasGroup upCanvasGroup;

    Image upBar;
    CanvasGroup downCanvasGroup;

    Image downBar;
    Image playerBreathImage;

    CanvasGroup canvasGroupToFade;
    AudioSource audioSource;

    AudioClip hyperventilationSound;

    float hyperventilationDuration = 4.0f;
    public Room parentRoom;

    public LayerMask defaultLayer;

    public LayerMask hidingLayer;

    public Transform hidingTransform;

    public Transform unhiddenSpace;
    bool alreadyHiding = false;
    public static event Action<MonoBehaviour> PlayerHiding;

    bool orbInHidingPlace;
    bool hidingSpace;

    public List<Sprite> sprites = new List<Sprite>();

    public enum SpriteVariations
    {
        Wood
    }
    public SpriteVariations ourSpriteVariation;

    SpriteRenderer spriteRenderer;
    GenerateNewBounds boundsGenerator;
#endregion
    public static event Action BreathedTooLoud;

    GameObject ourInteriorBackgroundSprite;
    void ScaleInteriorBackground(){

       // Vector3 newVector = Vector3.zero;
        //newVector.x = spriteRenderer.bounds.size.x / ourInteriorBackgroundSprite.transform.localScale.x;
        //newVector.y = spriteRenderer.bounds.size.y / ourInteriorBackgroundSprite.transform.localScale.y;
       // Debug.Log(spriteRenderer.bounds.size);
//        ourInteriorBackgroundSprite.transform.localScale = spriteRenderer.bounds.size/3; 
        //ourInteriorBackgroundSprite.GetComponent<SpriteRenderer>().bounds.extents.x = newVector;
        
    }
    public void BreathedTooLoudWrapper()
    {
        if (BreathedTooLoud != null)
        {
            BreathedTooLoud();
        }
    }
    void Awake()
    {
//        ourInteriorBackgroundSprite = GetComponentsInChildren<SpriteRenderer>()[1].gameObject; 
audioSource = GetComponent<AudioSource>();
        upCanvasGroup = GameHandler.breathCanvas.Find("Up").GetComponent<CanvasGroup>();
        downCanvasGroup = GameHandler.breathCanvas.Find("Down").GetComponent<CanvasGroup>();
        upBar = upCanvasGroup.GetComponentsInChildren<Image>()[0];
        downBar = downCanvasGroup.GetComponentsInChildren<Image>()[0];
        boundsGenerator = GetComponent<GenerateNewBounds>();
        Debug.Log(parentRoom);
        //TODO: put the above back in after testing
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite != null)
        {
            gameObject.AddComponent<PolygonCollider2D>();
            // boundsGenerator.GenerateNewColliderSize();
        }
        ourKeyCodes.Add(KeyCode.I);
        ourKeyCodes.Add(KeyCode.K);
        //ScaleInteriorBackground();

    }

    public void SetParentRoomDependencies()
    {
        parentRoom.EnemyEnteredAdjacentRoom += HyperventilationHandlerWrapper;
        parentRoom.EnemyExitedAdjacentRoom += StopHyperventilating;
    }
    public void PlayerHidingWrapper()
    {
        alreadyHiding = true;
        thisHidingPlaceStatus = PlayerStatus.HidingInThisPlace;
        GameHandler.playerGO.transform.position = hidingTransform.position;
        GameHandler.playerGO.layer = LayerMask.NameToLayer("HidingSpace");//LayerMask.(int)hidingLayer;
        if (PlayerHiding != null)
        {
            PlayerHiding(this);
        }
    }

    void StopHyperventilating(Room room)
    {
        enemyNearby = false;
    }
    bool enemyNearby;
    float maxHyperventilationInterval = 3;
    float minHyperventilationInterval = 2;

    void HyperventilationHandlerWrapper(Room room)
    {
        enemyNearby = true;
        StartCoroutine(HyperventilationHandler());
    }
    public IEnumerator HyperventilationHandler()
    {

        float hyperventilationInterval = 0;
        hyperventilationInterval = UnityEngine.Random.Range(minHyperventilationInterval, maxHyperventilationInterval);
        Debug.Log(hyperventilationInterval);
        yield return new WaitForSeconds(hyperventilationInterval);
        audioSource.clip = hyperventilationSound;
        audioSource.volume = 0.3f;
        audioSource.Play();
        while (alreadyHiding)
        {
            if (enemyNearby == false)
            {
                break;
            }
            audioSource.DOFade(1.0f, hyperventilationDuration);
            audioSource.DOPitch(2.0f, hyperventilationDuration);

            if (!waitingForPrompt)
            {
                yield return StartCoroutine(PromptCalm());
            }
            if (hyperventilationStrike == 2)
            {
                break;
            }
            //audioSource.PlayOneShot(hyperventilationSound);
            hyperventilationInterval = UnityEngine.Random.Range(minHyperventilationInterval, maxHyperventilationInterval);
            Debug.Log(hyperventilationInterval);
            yield return new WaitForSeconds(hyperventilationInterval);
        }
        if (hyperventilationStrike == 2)
        {
            BreathedTooLoudWrapper();
            hyperventilationStrike = 0;
        }
    }




    List<KeyCode> ourKeyCodes = new List<KeyCode>();
    List<KeyCode> falseKeyCodes = new List<KeyCode>();

    bool CheckKeyCode(KeyCode codePressed)
    {
        bool correctCode = true;
        if (falseKeyCodes.Contains(codePressed))
        {
            //if the keycode is a member of the false keycodes, meaning it was the wrong key
            correctCode = false;
        }
        return correctCode;
    }

    bool waitingForPrompt;
    KeyCode lastHitKey;
    void OnGui()
    {
        if (waitingForPrompt)
        {
            if (Input.anyKeyDown)
            {
                lastHitKey = Event.current.keyCode;
            }
        }
    }

public enum PlayerStatus{
        NotHidingHere,
        HidingInThisPlace

    }

public PlayerStatus thisHidingPlaceStatus;


    KeyCode GrabKeyCodes()
    {
        int index = UnityEngine.Random.Range(0, ourKeyCodes.Count);
        KeyCode chosenKeyCode = ourKeyCodes[index];
        foreach (KeyCode code in ourKeyCodes)
        {
            if (code == chosenKeyCode)
            {
                continue;
            }
            falseKeyCodes.Add(code);
        }
        return chosenKeyCode;
    }
    public IEnumerator PromptCalm()
    {
        Debug.Log("Prompting calm");
        waitingForPrompt = true;
        float startTime = Time.time;
        float hitDurationWindow = 1.0f;
        bool hitSuccess = false;
        KeyCode ourKeyCode = GrabKeyCodes();
        if (ourKeyCode == KeyCode.I)
        {
            canvasGroupToFade = upCanvasGroup;
            playerBreathImage = upBar;
        }
        else if (ourKeyCode == KeyCode.K)
        {
            canvasGroupToFade = downCanvasGroup;
            playerBreathImage = downBar;
        }

        playerBreathImage.fillAmount = 1.0f;
        canvasGroupToFade.DOFade(1, 0.3f);
        while (Time.time < startTime + hitDurationWindow)
        {
            playerBreathImage.fillAmount -= Time.deltaTime / hitDurationWindow;
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(ourKeyCode))
                {
                    playerBreathImage.color = Color.cyan;
                    hitSuccess = true;
                    break;
                }
                if (!CheckKeyCode(lastHitKey))
                {

                    playerBreathImage.color = Color.red;
                    break;
                }

            }
            yield return null;
        }
        waitingForPrompt = false;
        canvasGroupToFade.DOFade(0, 0.3f);
        if (!hitSuccess)
        {
            Debug.Log("Now we're hyperventilating worse");
            hyperventilationStrike++;
        }
        else{
            Debug.Log("Successfuly calmed down for now");
        }
    }

    int hyperventilationStrike = 0;


    public static event Action<MonoBehaviour> PlayerNoLongerHiding;
    public void PlayerStoppedHidingWrapper()
    {
        alreadyHiding = false;
        thisHidingPlaceStatus = PlayerStatus.NotHidingHere;
        GameHandler.playerGO.transform.position = unhiddenSpace.position;
        GameHandler.playerGO.layer = GameHandler.defaultPlayerLayer;
        if (PlayerNoLongerHiding != null)
        {
            PlayerNoLongerHiding(this);
        }

    }

    public void OnHoverMe(Player player)
    {
        Debug.Log("Press E to hide");
    }

    public void OnStopHoverMe(Player player)
    {

    }

    public void OnInteractWithMe(Player player)
    {
        if (!alreadyHiding)
        {
            PlayerHidingWrapper();
        }
        else
        {
            PlayerStoppedHidingWrapper();
        }

    }

    void HidePlayer(Player player)
    {
    }
    // Use this for initialization
    void Start()
    {
        thisHidingPlaceStatus = PlayerStatus.NotHidingHere;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && !waitingForPrompt && thisHidingPlaceStatus == PlayerStatus.HidingInThisPlace)
        {
            StartCoroutine(PromptCalm());
        }
    }
}
