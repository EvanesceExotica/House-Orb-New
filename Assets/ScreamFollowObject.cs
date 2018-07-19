using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG;
using System;
public class ScreamFollowObject : MonoBehaviour
{

    public static event Action<Transform> ScreamObjectMoving;

    public void ScreamObjectMovingWrapper()
    {
        if (ScreamObjectMoving != null)
        {
            ScreamObjectMoving(transform);
        }
    }
    void Awake()
    {

    }
    // Use this for initialization
    SpriteRenderer spriteRenderer;
    IEnumerator MoveScreamObject(Room room, int direction, float duration)
    {
        Debug.Log("We have started to move the screamObject");
        float startTime = Time.time;
        float elapsedTime = 0;
        Transform destination = null;
        if (direction == 1)
        {
            destination = room.entranceB;
        }
        else if (direction == -1)
        {
            destination = room.entranceA;
        }

        while (Time.time < startTime + duration)
        {
            if ((room = GameHandler.roomManager.GetPlayerCurrentRoom()))
            {
                if (Vector2.Distance(GameHandler.playerGO.transform.position, transform.position) <= 1.0f)
                {
                    //the particles will either attach to the player or disperse on their own time
                    yield break;
                }
            }
            Debug.Log("We are moving the scream object in room " + room.name);
            transform.position = Vector2.Lerp(transform.position, destination.position, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }





    public void MoveScreamObjectWrapper(Room room, int direction, float duration)
    {

        ScreamObjectMovingWrapper();
        StartCoroutine(MoveScreamObject(room, direction, duration));
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
