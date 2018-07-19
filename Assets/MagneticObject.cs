using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MagneticObject : MonoBehaviour
{

    public Speech reactionToPull;
//Add dreamy, distant dialogue stuff like "I think I remember this..." "this reminds me of .... something..."
    public static event Action Attracted;

    public void AttractedWrapper()
    {
        if (Attracted != null)
        {
            Attracted();
        }
    }
    public static event Action StoppedAttraction;

    public void StoppedAttractionWrapper()
    {
        if (StoppedAttraction != null)
        {
            StoppedAttraction();
        }
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject == GameHandler.fatherOrbGO)
        {
            AttractedWrapper();
        }
    }

    void OnTriggerExit2D(Collider2D hit)
    {
		if(hit.gameObject == GameHandler.fatherOrbGO){
			StoppedAttractionWrapper();
		}

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
