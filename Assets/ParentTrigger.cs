using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParentTrigger : MonoBehaviour {

     public abstract void OnChildTriggerEnter2D(Collider2D hit, GameObject hitchild);





     public abstract void OnChildTriggerExit2D(Collider2D hit, GameObject hitChild);
    


    


}
