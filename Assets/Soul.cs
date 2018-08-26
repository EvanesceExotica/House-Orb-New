
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MirzaBeig.ParticleSystems;
using System.Linq;

public class Soul : MonoBehaviour
{

    public ParticleSystems defaultParticleSystems;

    public ParticleSystems failedParticleSystems;

    public ParticleSystems chosenBurstParticleSystems;

    public ParticleSystems collisionParticleSystems;
    void Awake()
    {

    }

    bool chosen;

    void BeCollidedWith()
    {
        if (chosen)
        {
            defaultParticleSystems.Stop();
            collisionParticleSystems.Play();
        }
        else
        {
            defaultParticleSystems.Stop();
            failedParticleSystems.Play();
        }
    }

    public void Chosen()
    {
        chosen = true;
        defaultParticleSystems.Stop();
        if (chosenBurstParticleSystems != null)
        {
            chosenBurstParticleSystems.Play();
        }
    }

    public void PlayDefaultParticleSystem(){
        defaultParticleSystems.Play();
    }

    public void PlayChosenParticleSystem()
    {
        chosenBurstParticleSystems.Play();

    }

    public void PlayFailedParticleSystem()
    {
        failedParticleSystems.Play();
    }

    public void StopNormalParticleSystem()
    {
        defaultParticleSystems.Stop();
    }

    void OnTriggerEnter2D(Collider2D hit){
        OrbFire orbFire = hit.GetComponent<OrbFire>();
        if(orbFire != null){
            BeCollidedWith();
            orbFire.Dissolve();

        }
    }


}