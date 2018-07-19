using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNewBounds : MonoBehaviour
{

    public Sprite baseInteriorSprite;
    // Use this for initialization
    void Start()
    {

    }

    SpriteRenderer spriteRenderer;

    BoxCollider2D boxCollider;
    CircleCollider2D circleCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void GenerateNewColliderSize()
    {
        Vector2 boundsSize = spriteRenderer.sprite.bounds.size;
        if (boxCollider != null)
        {
            boxCollider.size = boundsSize;
            boxCollider.offset = new Vector2((boundsSize.x / 2), 0);
        }
		else if(circleCollider != null){
			circleCollider.radius = boundsSize.x/2; 
			circleCollider.offset = new Vector2((boundsSize.x / 2), 0);
		}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
