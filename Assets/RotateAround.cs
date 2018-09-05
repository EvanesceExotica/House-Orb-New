using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirzaBeig.ParticleSystems;

public class RotateAround : MonoBehaviour
{

    List<Transform> childLocations = new List<Transform>();
    [SerializeField] List<GameObject> lights = new List<GameObject>();
    // Use this for initialization
    List<ParticleSystems> lightParticles = new List<ParticleSystems>();

    [SerializeField] List<Soul> souls = new List<Soul>();
    List<TrailRenderer> trailRenderers = new List<TrailRenderer>();

    GameObject orbFire;
    void Awake()
    {
        //StarScream.ScreamHitPlayerCurrentRoom += ChangeSoulColor;
        orbFire = transform.parent.Find("LaunchedOrb").gameObject; 
        StarScream.ScreamHitRoomAdjacent += RotateSoulsOutWrapper;
        //Room.RoomWithPlayerHit += this.ChangeSoulColor;
        FatherOrb.Dropped += this.RotateSoulsInWrapper;
        OrbFire.WrongOrNoSoulChosen += DisperseSouls;
        OrbFire.CorrectSoulChosen += DisperseSouls;
        PromptPlayerHit.ScreamPrompted += ChangeSoulColor;
       // AutoRepel.AutoRepelTriggered += AutoRepelActivated;
        foreach (Transform child in transform)
        {
            if (child.tag == "Location")
            {
                childLocations.Add(child);
            }
            else if (child.tag == "Soul")
            {
                lights.Add(child.gameObject);
                souls.Add(child.GetComponent<Soul>());
            }
        }

        foreach (GameObject go in lights)
        {
            trailRenderers.AddRange(go.GetComponentsInChildren<TrailRenderer>());
            ParticleSystems[] particleSystems = go.GetComponentsInChildren<ParticleSystems>();
            lightParticles.Add(particleSystems[0]);
            //lightParticles.AddRange(go.GetComponentsInChildren<ParticleSystems>());
        }
        foreach (TrailRenderer tr in trailRenderers)
        {
            tr.time = 0f;
        }

    }

    void OnDisable(){
        StarScream.ScreamHitRoomAdjacent -= RotateSoulsOutWrapper;
        FatherOrb.Dropped -= this.RotateSoulsInWrapper;
        OrbFire.WrongOrNoSoulChosen -= DisperseSouls;
        OrbFire.CorrectSoulChosen -= DisperseSouls;
        PromptPlayerHit.ScreamPrompted -= ChangeSoulColor;

    }
    void Start()
    {

    }

    void RotateSoulsOutWrapper()
    {
        StartCoroutine(RotateSoulsOut());
    }

    void RotateSoulsOutWrapper(MonoBehaviour mono)
    {
        StartCoroutine(RotateSoulsOut());
    }

    void RotateSoulsInWrapper()
    {
        StartCoroutine(RotateSoulsIn());
    }

    void RotateSoulsInWrapper(MonoBehaviour mono){
        StartCoroutine(RotateSoulsIn());
    }

    public IEnumerator RotateSoulsOut()
    {
        foreach(Soul soul in souls){
            soul.PlayDefaultParticleSystem();
        }
        foreach (TrailRenderer tr in trailRenderers)
        {
            tr.time = 1.0f;
        }
        float distance = Vector2.Distance(lights[0].transform.position, childLocations[0].transform.position);
        while (distance > 0.5f)
        {
            // move them out away from center
            for (int i = 0; i < childLocations.Count; i++)
            {

                lights[i].transform.position = Vector2.MoveTowards(lights[i].transform.position, childLocations[i].position, 5.0f * Time.deltaTime);
            }
            yield return null;
        }

    }

    public IEnumerator RotateSoulsIn()
    {
        orbFire.SetActive(false);
        foreach (TrailRenderer tr in trailRenderers)
        {
            tr.time = 0f;
            tr.enabled = false;
        }
        float distance = Vector2.Distance(lights[0].transform.position, transform.position);
        while (distance > 0.5f)
        {
            //move them toward center
            for (int i = 0; i < childLocations.Count; i++)
            {

                lights[i].transform.position = Vector2.MoveTowards(lights[i].transform.position, transform.position, 5.0f * Time.deltaTime);
            }
            yield return null;
        }
        foreach (ParticleSystems particles in lightParticles)
        {
            particles.Stop();
        }

    }

    void ChangeSoulColor(MonoBehaviour something)
    {
        //this method chooses the soul that's going to be collided with to have a successful hit
        int randomIndex = UnityEngine.Random.Range(0, lights.Count);
        Soul chosenSoul = lights[randomIndex].GetComponent<Soul>();
        chosenSoul.Chosen();
        orbFire.SetActive(true);

    }

    void DisperseSouls(){
        Debug.Log("Soul being dispersed");
        foreach(Soul soul in souls){
            if(soul.chosen == false){
                soul.StopNormalParticleSystem();
                
            }
        }
        foreach(TrailRenderer trenderer in trailRenderers){
            trenderer.time = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){
            ChangeSoulColor(this);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RotateSoulsOutWrapper();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            RotateSoulsInWrapper();
        }
        transform.Rotate(0, 0, -50 * Time.deltaTime); //rotates 50 degrees per second around z axis

    }
}
