using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPausable
{

    Transform animationTransform;
    [SerializeField] Animator playerAnimator;
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

    bool paused;
    public void PauseMe(){
        paused = true;
        cantMove = true;
    }

    public void UnpauseMe(){
        paused = false;
        cantMove = false;
    }
    SpriteRenderer spriteRenderer;
    // Use this for initialization
    void Awake()
    {
        animationTransform = transform.Find("Tamela");
        playerAnimator = GetComponentInChildren<Animator>();
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

        FatherOrb.PickedUp += SetHoldingOrbAnimation;
        FatherOrb.Dropped += SetNOTHoldingOrbAnimation;
    }

    void OnDisable()
    {
        OrbController.ChannelingOrb -= SetCantMove;
        //make this a 
        OrbController.ManuallyStoppedChannelingOrb -= SetYesCanMove;
        OrbController.SconceRevealedStoppedChannelingOrb -= SetYesCanMove;

        FatherOrb.MovingBetweenPlayerAndObject -= SetCantMove;
        FatherOrb.StoppedMovingBetweenPlayerAndObject -= SetYesCanMove;

        Memory.LookingAtMemory -= SetCantMove;
        Memory.StoppedLookingAtMemory -= SetYesCanMove;

        HidingSpace.PlayerHiding -= MakePlayerStatic;
        HidingSpace.PlayerNoLongerHiding -= MakePlayerDynamic;

        PromptPlayerHit.WaitingForScreamPrompt -= SetCantMove;
        PromptPlayerHit.ScreamPromptPassed -= SetYesCanMove;

        FatherOrb.PickedUp -= SetHoldingOrbAnimation;
        FatherOrb.Dropped -= SetNOTHoldingOrbAnimation;

    }

    void SetHoldingOrbAnimation(MonoBehaviour behavior)
    {
        playerAnimator.SetBool("OrbHeld", true);
    }

    void SetNOTHoldingOrbAnimation(MonoBehaviour behavior)
    {
        playerAnimator.SetBool("OrbHeld", false);
    }

    void MakePlayerStatic(MonoBehaviour ourObject)
    {
        SetCantMove(GameHandler.Instance().player);
        SetKinematic();
    }

    void MakePlayerDynamic(MonoBehaviour ourObject)
    {
        SetYesCanMove(GameHandler.Instance().player);
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

        if (!cantMove)
        {
            playerAnimator.SetFloat("HorizontalMove", Mathf.Abs(h));
        }
        if (h > 0 || h < 0)
        {
            //playerAnimator.SetFloat("HorizontalMove", h);
        }
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
        Vector3 theScale = animationTransform.localScale;
        theScale.x *= -1;
        animationTransform.transform.localScale = theScale;
        //spriteRenderer.flipX = facingRight;

        facingRight = !facingRight;
        GameHandler.Instance().SwitchOrbHoldPositions(facingRight);
        GameHandler.Instance().fatherOrb.HandleFlip();

    }

    public void ChangeSpeed(float value)
    {
        maxSpeed *= value;
    }
}

