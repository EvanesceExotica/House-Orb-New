using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]


public class VisibleCollider : MonoBehaviour {

    private Collider2D ourCollider;
    public Color ourColor;

    public enum ColliderTypes{
        Circle,
        Box
    }

    public ColliderTypes OurColliderType;

	private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;


	void Start () {

        ourColor = Color.red;
        circleCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();

     
    }



	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
        if (OurColliderType == ColliderTypes.Box && boxCollider != null)
        {
            DebugExtension.DrawBounds(boxCollider.bounds, ourColor);
        }
        else if(OurColliderType == ColliderTypes.Circle && circleCollider != null)
        {
           
            DebugExtension.DrawCircle(transform.position, Vector3.forward, ourColor, circleCollider.bounds.extents.x);
           // DebugExtension.DrawCircle(transform.position, Vector3.forward, Color.yellow, circleCollider.radius);
        }
	
	}

}


