using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;
    public float moveForce = 15f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;


    private bool grounded = false;
    //private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] bool cantMove = false;


    SpriteRenderer spriteRenderer;
    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        OrbController.ChannelingOrb += SetCantMove;
        //make this a 
        OrbController.ManuallyStoppedChannelingOrb += SetYesCanMove;
        OrbController.SconceRevealedStoppedChannelingOrb += SetYesCanMove;

        FatherOrb.MovingBetweenPlayerAndObject += SetCantMove;
        FatherOrb.StoppedMovingBetweenPlayerAndObject += SetYesCanMove;

        Memory.LookingAtMemory += SetCantMove;
        Memory.StoppedLookingAtMemory += SetYesCanMove;

        HidingSpace.PlayerHiding += MakePlayerStatic;
        HidingSpace.PlayerNoLongerHiding += MakePlayerDynamic;

        PromptPlayerHit.WaitingForScreamPrompt += SetCantMove;
        PromptPlayerHit.ScreamPromptPassed += SetYesCanMove;
    }

    void MakePlayerStatic(MonoBehaviour ourObject)
    {
        SetCantMove(GameHandler.player);
        SetKinematic();
    }

    void MakePlayerDynamic(MonoBehaviour ourObject)
    {
        SetYesCanMove(GameHandler.player);
        SetDynamic();
    }

    public List<GameObject> incapacitators = new List<GameObject>();
    void SetCantMove(MonoBehaviour incapacitator)
    {
        rb.velocity = new Vector2(0, 0);
        Debug.Log(incapacitator.ToString() + " made us not move ");
        if (!incapacitators.Contains(incapacitator.gameObject))
        {
            incapacitators.Add(incapacitator.gameObject);
        }
        cantMove = true;
    }

    void SetYesCanMove(MonoBehaviour incapacitator)
    {
        if (incapacitators.Contains(incapacitator.gameObject))
        {
            incapacitators.Remove(incapacitator.gameObject);
        }
        if (incapacitators.Count == 0)
        {
            cantMove = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Floor"));

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
    }

    void SetKinematic()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void SetDynamic()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        Vector2 currentVelocity = rb.velocity;
        currentVelocity.x = Input.GetAxisRaw("Horizontal") * maxSpeed;
        // anim.SetFloat("Speed", Mathf.Abs(h));

        if (!cantMove)
        {
            rb.velocity = currentVelocity;
            // if (h * rb.velocity.x < maxSpeed)
            //     rb.velocity = new Vector2(0,;

            // if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            //     rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

            if (h > 0 && !facingRight)
                Flip();
            else if (h < 0 && facingRight)
                Flip();

            if (jump)
            {
                // anim.SetTrigger("Jump");
                rb.AddForce(new Vector2(0f, jumpForce));
                jump = false;
            }
        }
    }

    public bool flipping = false;
    void Flip()
    {
        flipping = true;
        spriteRenderer.flipX = facingRight;

        facingRight = !facingRight;
        GameHandler.SwitchOrbHoldPositions(facingRight);
        GameHandler.fatherOrb.HandleFlip();
        
    }

    public void ChangeSpeed(float value)
    {
        maxSpeed *= value;
    }
}

