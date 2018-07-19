using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using System.Linq;
using MirzaBeig.ParticleSystems;
public class Memory : MonoBehaviour, iInteractable
{



    AudioClip music;
    public string promptString;

    Speech reaction;

    public Speech inMemoryConversation;
    public Speech hintGivenComment;
    public Speech previousSconceTeleportGivenComment;
    public Speech autoRepelComment;
    public HiddenSconce ourConnectedHiddenSconce;
    public ParticleSystems memoryParticles;
    public ParticleSystem memoryParticles2;
    public Room parentRoom;
    Material memoryBackgroundMaterial;

    MeshRenderer memoryPlaneRenderer;

    public List<Material> materials = new List<Material>();
    List<Camera> memoryCameras = new List<Camera>();

    // public static Action <Speech> TriggeredSpeech;

    // public void TriggeredSpeechWrapper(Speech speech){
    //     //TODO: put this in a general class maybe?
    //     if(TriggeredSpeech != null){
    //         TriggeredSpeech(speech);
    //     }
    // }
    public static Action HoveringOverMemoryObject;

    public static Action<MonoBehaviour> LookingAtMemory;

    [SerializeField] bool canLookAtMemory = false;
    void SetCanLookAtMemory(MonoBehaviour mono)
    {
        canLookAtMemory = true;
    }

    void SetCantLookAtMemory(MonoBehaviour mono)
    {
        canLookAtMemory = false;
    }
    public void LookingAtMemoryWrapper()
    {
        if (LookingAtMemory != null)
        {
            LookingAtMemory(this);
        }
    }

    public void StoppedLookingAtMemoryWrapper()
    {
        if (StoppedLookingAtMemory != null)
        {
            StoppedLookingAtMemory(this);
        }
    }

    Light ourLight;
    void ActivateLight()
    {
        ourLight.DOIntensity(1, 2.0f);
    }

    public static Action<MonoBehaviour> StoppedLookingAtMemory;

    public static Action StoppedHoveringOverMemoryObject;
    void StoppedHoveringOverMemoryObjectWrapper()
    {
        memoryParticles.Stop();
        if (StoppedHoveringOverMemoryObject != null)
        {
            StoppedHoveringOverMemoryObject();
        }
    }
    void HoveringOverMemoryObjectWrapper()
    {
        memoryParticles.SetPlaybackSpeed(0.5f);
        memoryParticles.Play();
        // memoryParticles.Play();
        if (HoveringOverMemoryObject != null)
        {
            HoveringOverMemoryObject();
        }
    }

    IEnumerator PlayMemory()
    {

        //if it's a visual memory there's an actual hint
        //TODO: put this back in
        //        StartMemoryVisual();
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(5.0f);
        Time.timeScale = 1;
    }
    void StartMemoryVisual()
    {
        int randomBackgroundViewIndex = UnityEngine.Random.Range(0, materials.Count);
        memoryPlaneRenderer.material = materials[randomBackgroundViewIndex];

    }
    // Use this for initialization
    public enum BuffGiven
    {
        AutoReflect,
        Hint,

        TimeDouble,

        PrevSconceTeleport
    }
    //Add a type of buff
    public void OnHoverMe(Player player)
    {
        if (canLookAtMemory)
        {
            //TODO: Play some sparkly effect for the object, make orb glow brighter
            Debug.Log("Press [E] to show memory to house father");
            player.interactPrompt.DisplayPrompt(promptString, this.gameObject);
            HoveringOverMemoryObjectWrapper();
        }
    }

    public void Awake()
    {
        OrbController.ChannelingOrb += SetCantLookAtMemory;
        FatherOrb.PickedUp += SetCanLookAtMemory;
        FatherOrb.Dropped += SetCantLookAtMemory;
        Conversation.FinishedDisplayingMemory += ApplyMemoryEffectsWrapper;        

        ourLight = GetComponent<Light>();
        ourLight.intensity = 0;
        promptString = " show object to house-father";
        memoryParticles2 = GetComponentInChildren<ParticleSystem>();
        memoryParticles = GetComponentInChildren<ParticleSystems>();
        List<GameObject> memoryCameraGOs = GameObject.FindGameObjectsWithTag("MemoryCam").ToList();
        foreach (GameObject go in memoryCameraGOs)
        {
            memoryCameras.Add(go.GetComponent<Camera>());
        }
        //memoryCameras = GameObject.FindGameObjectsWithTag("MemoryCam").GetComponents<Camera>();
    }
    public void OnStopHoverMe(Player player)
    {
        if (canLookAtMemory)
        {
            player.interactPrompt.HidePrompt(gameObject);
            StoppedHoveringOverMemoryObjectWrapper();
        }
    }

    public IEnumerator InitializeMemory(){
        while (Vector2.Distance(GameHandler.fatherOrbGO.transform.position, transform.position) > 0.1f)
        {
            yield return null;
        }
        Time.timeScale = 0;
        LookingAtMemoryWrapper();
    }

    public void ApplyMemoryEffectsWrapper(){
        Time.timeScale = 1;
        StartCoroutine(ApplyMemoryEffects());
    }
    public IEnumerator ApplyMemoryEffects()
    {
       
        if (RefreshGiven != null)
        {
            //have the time refresh be every time you see a memory since they're rare enough
            RefreshGiven();
        }

        if (givenBuff == BuffGiven.Hint)
        {
            if (HintGiven != null)
            {
                HintGiven(ourConnectedHiddenSconce);
                reaction = hintGivenComment;
            }

        }
        else if (givenBuff == BuffGiven.AutoReflect)
        {
            Debug.Log("We were given the buff auto reflect the scream");
            if (AutoReflectGiven != null)
            {
                AutoReflectGiven();
                reaction = autoRepelComment;
            }
        }
        else if (givenBuff == BuffGiven.PrevSconceTeleport)
        {
            Debug.Log("We were given the buff to teleport");
            if (PrevSconceTeleportGiven != null)
            {
                PrevSconceTeleportGiven();
                reaction = previousSconceTeleportGivenComment;
            }
        }
        MoveBack();
        while (Vector2.Distance(GameHandler.fatherOrbGO.transform.position, GameHandler.fatherOrbHoldTransform.position) > 0.1f)
        {
            yield return null;
        }
        StoppedLookingAtMemoryWrapper();
        SpeechTrigger.SpeechTriggeredWrapper(reaction);

    }

    public void OnInteractWithMe(Player player)
    {
        if (canLookAtMemory)
        {
            StartCoroutine(InitializeMemory());
            MoveToMemory();
        }

    }

    public static event Action<HiddenSconce> HintGiven;

    public static event Action RefreshGiven;

    public static event Action AutoReflectGiven;

    public static event Action PrevSconceTeleportGiven;
    public BuffGiven givenBuff;

    void MoveToMemory()
    {
        //play like "Hands lifted" animation
        GameHandler.fatherOrb.MoveUsWrapper(GameHandler.fatherOrbHoldTransform.position, transform.position, this);
    }

    void MoveBack()
    {
        GameHandler.fatherOrb.MoveUsWrapper(this.gameObject.transform.position, GameHandler.fatherOrbHoldTransform.position, GameHandler.player);

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
