using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocationHandler : MonoBehaviour
{

    LayerMask sconceLayer;
    LayerMask orbLayer;

    public List<iInteractable> interactablesWereHovering = new List<iInteractable>();
    public List<GameObject> gameObjectsWereHovering = new List<GameObject>();
    Player player;

    Sconce sconceHoveredOver;

    FatherOrb orbHoveredOver;
    iInteractable objectHovering = null;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    public void OnTriggerEnter2D(Collider2D hit)
    {
        iInteractable interactableObject = hit.GetComponent<iInteractable>();

        if (interactableObject != null)
        {

            interactableObject.OnHoverMe(player);
            interactablesWereHovering.Add(interactableObject);
            gameObjectsWereHovering.Add(hit.gameObject);
        }

    }

    public void OnTriggerExit2D(Collider2D hit)
    {
        iInteractable interactableObject = hit.GetComponent<iInteractable>();
        if (interactableObject != null)
        {
            
            interactableObject.OnStopHoverMe(player);

            if (interactablesWereHovering.Contains(interactableObject))
            {
                interactablesWereHovering.Remove(interactableObject);
            }
            if (gameObjectsWereHovering.Contains(hit.gameObject))
            {
                gameObjectsWereHovering.Remove(hit.gameObject);
            }
            //objectHovering.OnHoverMe(player);
        }

    }
    bool CheckIfHoveringOrbAndSconce()
    {
        bool hoveringSconce = false;
        bool hoveringOrb = false;
        foreach (GameObject go in gameObjectsWereHovering)
        {
            if (go.GetComponent<Sconce>() != null)
            {
                hoveringSconce = true;
            }
            if (go.GetComponent<FatherOrb>() != null)
            {
                hoveringOrb = true;
            }

        }
        if (hoveringSconce && hoveringOrb)
        {
            return true;
        }
        else { return false; }
    }

    // bool CheckIfHoveringOrbAndSconce(){
    //     foreach(iInteractable interactableObject in objectsWereHovering){

    //         }
    //     }
    // }

    GameObject FindClosest()
    {
        GameObject closestObject = null;
        float minDistance = Mathf.Infinity;
        foreach (GameObject go in gameObjectsWereHovering)
        {
            float distance = Vector2.Distance(go.transform.position, gameObject.transform.position);
            if (distance < minDistance)
            {
                closestObject = go;
                minDistance = distance;
            }
        }
        return closestObject;
    }

    void CheckInteractable(iInteractable interactableObject){
        if(interactableObject.GetType() == typeof(Food)){
            if(!player.cantEat){
                interactableObject.OnInteractWithMe(player);
            }
        }
        else{
            interactableObject.OnInteractWithMe(player);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //todo -- if hovering multiple objects, make sure 
            if (interactablesWereHovering.Count > 0)
            {
                CheckInteractable(FindClosest().GetComponent<iInteractable>());
                //FindClosest().GetComponent<iInteractable>().OnInteractWithMe(player);
            }
            // if (objectsWereHovering.Count >  1)
            // {
            //     objectsWereHovering[0].OnInteractWithMe(player);
            // }
        }
    }
}
