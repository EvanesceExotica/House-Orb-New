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
    void Awake()
    {
		StarScream.ScreamHitPlayerCurrentRoom += ChangeSoulColor; 
        StarScream.ScreamBegun += RotateSoulsOutWrapper; 
        foreach (Transform child in transform)
        {
            childLocations.Add(child);
        }
        foreach(GameObject go in lights){
            lightParticles.AddRange(go.GetComponentsInChildren<ParticleSystems>());
        }
            
    }
    void Start()
    {

    }

    void RotateSoulsOutWrapper(){
        StartCoroutine(RotateSoulsOut());
    }

    void RotateSoulsInWrapper(){
        StartCoroutine(RotateSoulsIn());
    }

    public IEnumerator RotateSoulsOut()
    {
        foreach(ParticleSystems particles in lightParticles){
            particles.Play();
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
        foreach(ParticleSystems particles in lightParticles){
            particles.Stop();
        }

    }

    void ChangeSoulColor(int)
    {


    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -50 * Time.deltaTime); //rotates 50 degrees per second around z axis

    }
}
