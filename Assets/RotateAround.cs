using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirzaBeig.ParticleSystems;

public class RotateAround : MonoBehaviour
{

    List<Transform> childLocations = new List<Transform>();
    List<GameObject> lights = new List<GameObject>();
    // Use this for initialization
    List<ParticleSystems> lightParticles = new List<ParticleSystems>();

    List<TrailRenderer> trailRenderers = new List<TrailRenderer>();
    void Awake()
    {
        StarScream.ScreamHitPlayerCurrentRoom += ChangeSoulColor;
        StarScream.ScreamBegun += RotateSoulsOutWrapper;
        foreach (Transform child in transform)
        {
            if (child.tag == "Location")
            {
                childLocations.Add(child);
            }
            else if (child.tag == "Soul")
            {
                lights.Add(child.gameObject);
            }
        }

        foreach (GameObject go in lights)
        {
            trailRenderers.AddRange(go.GetComponentsInChildren<TrailRenderer>());
            lightParticles.AddRange(go.GetComponentsInChildren<ParticleSystems>());
        }
        foreach (TrailRenderer tr in trailRenderers)
        {
            tr.time = 0f;
        }

    }
    void Start()
    {

    }

    void RotateSoulsOutWrapper()
    {
        StartCoroutine(RotateSoulsOut());
    }

    void RotateSoulsInWrapper()
    {
        StartCoroutine(RotateSoulsIn());
    }

    public IEnumerator RotateSoulsOut()
    {
        foreach (ParticleSystems particles in lightParticles)
        {
            particles.Play();
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
        foreach (TrailRenderer tr in trailRenderers)
        {
            tr.time = 0f;
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

    void ChangeSoulColor(int something)
    {


    }
    // Update is called once per frame
    void Update()
    {
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
