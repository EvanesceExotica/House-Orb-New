using UnityEngine;
using System.Collections;

public class TriggerForParent : MonoBehaviour {

    Transform parent;
    ParentTrigger parentTrigger;

    public virtual void OnTriggerEnter2D(Collider2D hit)
    {
        parentTrigger.OnChildTriggerEnter2D(hit, gameObject);
    }

    public virtual void OnTriggerExit2D(Collider2D hit)
    {
        parentTrigger.OnChildTriggerExit2D(hit, gameObject);
    }


   
	// Use this for initialization
	void Start () {

        parent = transform.parent;
        parentTrigger = gameObject.GetComponentInParent<ParentTrigger>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
