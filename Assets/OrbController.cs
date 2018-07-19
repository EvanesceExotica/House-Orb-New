using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class OrbController : MonoBehaviour
{

    public bool flipped;
    public static event Action<MonoBehaviour> ChannelingOrb;
    void ChannelingOrbWrapper(MonoBehaviour orb)
    {
        if (ChannelingOrb != null)
        {
            ChannelingOrb(orb);
        }
    }

    public static event Action<MonoBehaviour> SconceRevealedStoppedChannelingOrb;

    void SconceRevealedStoppedChannelingOrbWrapper(MonoBehaviour mono)
    {
        if (SconceRevealedStoppedChannelingOrb != null)
        {
            SconceRevealedStoppedChannelingOrb(mono);
        }
    }

    //this event is specifically to put the orb back into the player's hands.
    public static event Action<MonoBehaviour> ManuallyStoppedChannelingOrb;
    void ManuallyStoppedChannelingOrbWrapper(MonoBehaviour orb)
    {
        if (ManuallyStoppedChannelingOrb != null)
        {
            ManuallyStoppedChannelingOrb(orb);
        }
    }


    bool canChannelOrb = false;
    bool channelingOrb = false;
    // Use this for initialization
    Rigidbody2D orbRigidBody;
    FatherOrb orb;
    float speed = 5.0f;

    void Awake()
    {
        FatherOrb.PickedUp += SetCanBeChanneled;
        FatherOrb.Dropped += SetCanNOTBeChanneled;
        Sconce.OrbInSconce += SetCanNOTBeChanneled;
        HiddenSconce.SconceRevealed += StopOrbBeingChanneled;
        orb = GetComponent<FatherOrb>();
        orbRigidBody = GetComponent<Rigidbody2D>();

    }

    void SetCanBeChanneled(MonoBehaviour ourObject)
    {

        canChannelOrb = true;
    }

    void SetCanNOTBeChanneled()
    {
        canChannelOrb = false;
    }

    void SetCanNOTBeChanneled(MonoBehaviour ourObject)
    {
        canChannelOrb = false;
    }

    void StartOrbBeingChanelled()
    {
        channelingOrb = true;
        GameHandler.proCamera.RemoveCameraTarget(GameHandler.playerGO.transform);
        GameHandler.proCamera.AddCameraTarget(GameHandler.fatherOrbGO.transform);
        orb.SetOrbBeingChanneled();
        ChannelingOrbWrapper(this);
        orbRigidBody.bodyType = RigidbodyType2D.Dynamic;
        orbRigidBody.gravityScale = 0;

    }

    void StopOrbBeingChanneled()
    {

        channelingOrb = false;
        GameHandler.proCamera.RemoveCameraTarget(GameHandler.fatherOrbGO.transform);
        GameHandler.proCamera.AddCameraTarget(GameHandler.playerGO.transform);
        //todo: whether or not this is carried or handled depends on whether or not
        //todo: maybe the orb begins screaming when outside of sconce and hand for too long
        orbRigidBody.velocity = Vector2.zero;
        if (GameHandler.fatherOrb.heldStatus != FatherOrb.HeldStatuses.InSconce)
        {
            //if it's not in the sconce, then we manually recalled it
            ManuallyStoppedChannelingOrbWrapper(this);
        }
        else
        {
            SconceRevealedStoppedChannelingOrbWrapper(this);
        }
        orbRigidBody.bodyType = RigidbodyType2D.Kinematic;
    }



    void FixedUpdate()
    {
        if (channelingOrb)
        {
            Debug.Log("Orb is being channeled now");
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            if (moveHorizontal > 0)
            {
                if (!flipped){

                }
              //      orbRigidBody.AddForce(Vector3.right * speed);
              //transform.Translate(Vector3.right * speed * Time.deltaTime);
                else
                {
                    //       orbRigidBody.AddForce(-Vector3.right * speed);
                    //transform.Translate(-Vector3.right * speed * Time.deltaTime);
                }

            }
            if (moveHorizontal < 0)
            {
                if (!flipped)
                {

                }
            //        orbRigidBody.AddForce(-Vector3.right * speed);
            //transform.Translate(-Vector3.right * speed * Time.deltaTime);
                else
                {

                    //         orbRigidBody.AddForce(Vector3.right * speed);
                    //transform.Translate(Vector3.right * speed * Time.deltaTime);
                }
            }
            if (moveVertical > 0)
            {
                //          orbRigidBody.AddForce(Vector3.up * speed);
                //transform.Translate(Vector3.up * speed * Time.deltaTime);

            }
            if (moveVertical < 0)
            {
                //         orbRigidBody.AddForce(Vector3.down * speed);
                //transform.Translate(-Vector3.up * speed * Time.deltaTime);
            }
            Vector2 movement;
            if (!flipped)
            {
                movement = new Vector2(moveHorizontal, moveVertical);
            }
            else
            {
                movement = new Vector2(-moveHorizontal, moveVertical);
            }
            orbRigidBody.velocity = movement * speed;
        }

    }

    void ReturnToPlayer()
    {
        orb.MoveUsWrapper(transform.position, GameHandler.fatherOrbHoldTransform.position, GameHandler.player);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (canChannelOrb && !channelingOrb)
            {
                StartOrbBeingChanelled();
            }
            else if (channelingOrb)
            {
                StopOrbBeingChanneled();
                //ReturnToPlayer();
            }
        }
    }
}
