using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAdheringPlayerMovement : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }
    public float movementRight;
    public float movementLeft;
    public LayerMask WhatIsGround;
    public bool grounded;
    public bool jumping;
    public bool falling;

    bool inputJump;
    float maxJumpHeight;
    float jumpDuration;

    SpriteRenderer ourSpriteRenderer;

    float gravityConstant = 9.8f;


    public bool fallingToPlanet;

    //public float area;

    public List<GameObject> inOrbitOfThesePlanets;




    public Vector3 stickPoint;
    public Vector3 stickNormal;

    public RaycastHit2D left;
    public RaycastHit2D right;


    public Transform LeftAlign;
    public Transform RightAlign;

    public LayerMask whatIsGround;

    Vector2 averagePlanetNormal;
    Vector2 locationPoint;

    [SerializeField]
    public bool cantOrient;

    float jumpStartTime;

    Vector2 center;
    float radius_;

    Vector2 rightPosition;
    Vector2 leftPosition;
    Rigidbody2D ourRigidbody;

    public Transform groundCheck;

    [SerializeField] bool cantMove = false;


    // Use this for initialization
    void Awake()
    {
        ourSpriteRenderer = GetComponent<SpriteRenderer>();
        ourRigidbody = GetComponent<Rigidbody2D>();
        ourRigidbody.bodyType = RigidbodyType2D.Kinematic;
        // anim = GetComponent<Animator>();
       // OrbController.ChannelingOrb += SetCantMove;
        //make this a 
       // OrbController.StoppedChannelingOrb += SetYesCanMove;

//        HidingSpace.PlayerHiding += SetCantMove;
      //  HidingSpace.PlayerNoLongerHiding += SetYesCanMove;
    }

    List<GameObject> incapacitators = new List<GameObject>();
    void SetCantMove(GameObject incapacitator)
    {
        Debug.Log(incapacitator.name + " made us not move ");
        incapacitators.Add(incapacitator);
        cantMove = true;
    }

    void SetYesCanMove(GameObject incapacitator)
    {
        incapacitators.Remove(incapacitator);
        if (incapacitators.Count == 0)
        {
            cantMove = false;
        }
    }

    void OnEnable()
    {


    }
    bool doubleRaycastDown()
    {
        Vector3 transformUp = transform.up;
        Vector3 transformRight = transform.right;
        float distanceToCenter = 0.53f; // Vector2.Distance(LeftAlign.position, transform.position);

        leftPosition = transform.position + 0.5f * transform.up + distanceToCenter * transform.right;

        rightPosition = transform.position + 0.5f * transform.up - distanceToCenter * transform.right;

        RaycastHit2D leftHit = Physics2D.Raycast(leftPosition, -transformUp, 5.0f, whatIsGround);
        left = leftHit;
        Debug.DrawRay(leftPosition, -transform.up * 10, Color.green);
        Debug.DrawRay(transform.position, leftHit.normal * 10, Color.magenta);

        RaycastHit2D rightHit = Physics2D.Raycast(rightPosition, -transformUp, 5.0f, whatIsGround);
        right = rightHit;
        Debug.DrawRay(transform.position, rightHit.normal * 10, Color.yellow);
        Debug.DrawRay(rightPosition, -transform.up * 10, Color.red);

        return right && left;

    }
    void Orientatate()
    {
        Vector3 transformUp = transform.up;
        Vector3 transformRight = transform.right;
        float distanceToCenter = 0.53f; // Vector2.Distance(LeftAlign.position, transform.position);


        leftPosition = transform.position + 0.5f * transform.up + distanceToCenter * transform.right;

        rightPosition = transform.position + 0.5f * transform.up - distanceToCenter * transform.right;

        RaycastHit2D leftHit = Physics2D.Raycast(leftPosition, -transformUp, 5.0f, whatIsGround);
        left = leftHit;
        Debug.DrawRay(leftPosition, -transform.up * 10, Color.green);
        Debug.DrawRay(transform.position, leftHit.normal * 10, Color.magenta);

        RaycastHit2D rightHit = Physics2D.Raycast(rightPosition, -transformUp, 5.0f, whatIsGround);
        right = rightHit;
        Debug.DrawRay(transform.position, rightHit.normal * 10, Color.yellow);
        Debug.DrawRay(rightPosition, -transform.up * 10, Color.red);



        if (leftHit && rightHit)
        {
            //Debug.Log("LEFT AND RIGHT HIT DOING STUFF CHECK");
            if (ourRigidbody.velocity.sqrMagnitude > 0)
            {
                if (ourRigidbody.velocity.x < 0)
                {
                    RaycastHit2D overrideLeftHit = Physics2D.Raycast(transform.position + 2.0f * transformUp, -transformRight, 5.0f, whatIsGround);

                    if (overrideLeftHit)
                    {
                        leftHit = overrideLeftHit;
                    }
                }
                else
                {
                    RaycastHit2D overrideRightHit = Physics2D.Raycast(transform.position + 2.0f * transformUp, -transformRight, 5.0f, whatIsGround);

                    if (overrideRightHit)
                    {
                        rightHit = overrideRightHit;
                    }
                }
            }





            Vector2 preciseOrientation = (leftHit.normal + rightHit.normal) / 2;
            averagePlanetNormal = preciseOrientation;
            Debug.DrawRay(transform.position, averagePlanetNormal * 30f, Color.magenta);
            Debug.DrawRay(transform.position, preciseOrientation * 10, Color.cyan);
            Vector2 precisePoint = (leftHit.point + rightHit.point) / 2;

            locationPoint = precisePoint;

            //TODO: Change this back to Vector3.up instead of transform.up
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, preciseOrientation);

            float speed = 200.0f;
            Quaternion finalRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0, 0, finalRotation.eulerAngles.z);
            float offset = Vector2.Distance(locationPoint, transform.position);

            // testPoint = locationPoint;
            // testPoint = (Vector3)locationPoint + transformUp * offset;

            if (!jumping && !cantOrient)
            {
                //      testPoint = (Vector3)locationPoint + transformUp * 0.9f;
                transform.position = (Vector3)locationPoint + transformUp * 0.9f;
                //   //Debug.Log("We're orientating!" + locationPoint);
            }



        }



    }

    bool moving = false;
    float maxSpeed = 5.0f;

    float horizontal;
    float vertical;
    bool jumpPressed;
    // Update is called once per frame
    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        jumpPressed = Input.GetButtonDown("Jump");
    }
    public void Update()
    {
        if (!cantMove)
        {
            Move(horizontal, vertical, jumpPressed);
        }
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Floor"));

        // if (moving)
        // {
        //     if (movementRight != 0)
        //     {

        //         transform.Translate(Vector3.right * maxSpeed * Time.deltaTime);
        //     }
        //     if (movementLeft != 0)
        //     {

        //         transform.Translate(-Vector3.right * maxSpeed * Time.deltaTime);
        //     }
        // }
        //OrientationTest();
        Orientatate();


        Debug.DrawRay(transform.position, transform.right * 50, Color.white);
        Debug.DrawRay(transform.position, transform.up * 10, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.up * 50, Color.cyan);
        //base.Update();

        if (Time.time > jumpStartTime + jumpDuration)
        {
            jumping = false;
        }



    }
    public IEnumerator WaitForFall()
    {
        //Debug.Log("Waiting for fall");
        yield return new WaitForSeconds(1.0f);
        fallingToPlanet = true;
    }





    public void Move(float moveH, float moveV, bool jump)
    {
        if (moveH == 0 && moveV == 0 && !jump)
        {
            movementLeft = 0;
            movementRight = 0;
            moving = false;
        }
        if (moveH > 0)
        {
            moving = true;
            movementRight = moveH;
            //ourRigidbody.AddForce(transform.right * maxSpeed);
            transform.Translate(Vector3.right * maxSpeed * Time.deltaTime);
//            ourSpriteRenderer.flipX = false;

        }
        else
        {
            movementRight = 0;
        }
        if (moveH < 0)
        {
            moving = true;
            transform.Translate(-Vector3.right * maxSpeed * Time.deltaTime);
            movementLeft = moveH;
           // ourRigidbody.AddForce(-transform.right * maxSpeed);
        //    ourSpriteRenderer.flipX = true;
        }
        else
        {
            movementLeft = 0;
        }
        if (grounded && jump)
        {
            fallingToPlanet = false;
            jumpStartTime = Time.time;

            jumping = true;
            StartCoroutine(WaitForFall());
            ourRigidbody.isKinematic = false;

            ourRigidbody.AddForce(transform.up * 200f);
        }


        if (grounded && fallingToPlanet)
        {
            // pReference.rb.isKinematic = true;
            fallingToPlanet = false;
            jumping = false;

        }







    }
}
