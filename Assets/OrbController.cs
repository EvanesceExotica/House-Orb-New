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

    Transform shakingTransform;
    void Awake()
    {
        shakingTransform = transform.Find("ObjectThatCanBeShook");
        FatherOrb.PickedUp += SetCanBeChanneled;
        FatherOrb.Dropped += SetCanNOTBeChanneled;
        Sconce.OrbInSconce += SetCanNOTBeChanneled;
        HiddenSconce.SconceRevealed += StopOrbBeingChanneled;
        CorruptedObject.Corrupting += BeCorrupted;
        CorruptedObject.StoppedCorrupting += StopBeingCorrupted;
        orb = GetComponent<FatherOrb>();
        orbRigidBody = GetComponent<Rigidbody2D>();

    }

    void OnDisable(){
        FatherOrb.PickedUp -= SetCanBeChanneled;
        FatherOrb.Dropped -= SetCanNOTBeChanneled;
        Sconce.OrbInSconce -= SetCanNOTBeChanneled;
        HiddenSconce.SconceRevealed -= StopOrbBeingChanneled;
        CorruptedObject.Corrupting -= BeCorrupted;
        CorruptedObject.StoppedCorrupting -= StopBeingCorrupted;

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
        GameHandler.Instance().proCamera.RemoveCameraTarget(GameHandler.Instance().playerGO.transform);
        GameHandler.Instance().proCamera.AddCameraTarget(GameHandler.Instance().fatherOrbGO.transform);
        orb.SetOrbBeingChanneled();
        ChannelingOrbWrapper(this);
        orbRigidBody.bodyType = RigidbodyType2D.Dynamic;
        orbRigidBody.gravityScale = 0;

    }

    void StopOrbBeingChanneled()
    {

        channelingOrb = false;
        GameHandler.Instance().proCamera.RemoveCameraTarget(GameHandler.Instance().fatherOrbGO.transform);
        GameHandler.Instance().proCamera.AddCameraTarget(GameHandler.Instance().playerGO.transform);
        //todo: whether or not this is carried or handled depends on whether or not
        //todo: maybe the orb begins screaming when outside of sconce and hand for too long
        orbRigidBody.velocity = Vector2.zero;
        if (GameHandler.Instance().fatherOrb.heldStatus != FatherOrb.HeldStatuses.InSconce)
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
            if(movement.x == 0 && movement.y == 0){

            }
            if((movement.x != 0 || movement.y != 0) && beingCorrupted){
                StopVibrating();
                //beingMoved = true;
            }
            orbRigidBody.velocity = movement * speed;
        }

    } 

    bool beingCorrupted;

    void BeCorrupted(){
        beingCorrupted = true;
    }

    void StopBeingCorrupted(){

        beingCorrupted = false;
    }
    
    void Vibrate(){
        transform.DOShakePosition(100f, 0.1f, 3, 90, false, true);
    }

    void StopVibrating(){
        DOTween.Kill(transform);
    }

    bool beingMoved;

    void ReturnToPlayer()
    {
        orb.MoveUsWrapper(transform.position, GameHandler.Instance().fatherOrbHoldTransform.position, GameHandler.Instance().player);
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
