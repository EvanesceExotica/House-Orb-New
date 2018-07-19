using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class StarScream : MonoBehaviour
{

    public static event Action ScreamBegun;

    void ScreamBegunWrapper()
    {
        if (ScreamBegun != null)
            ScreamBegun();
    }
    public static event Action<int> ScreamHitPlayerCurrentRoom;

    void ScreamHitPlayerRoom(int direction)
    {

        if (ScreamHitPlayerCurrentRoom != null)
        {
            ScreamHitPlayerCurrentRoom(direction);
        }
    }

    public static event Action RumbleHitPlayerCurrentRoom;

    public static event Action ScreamHitRoomAdjacent;

    void ScreamHitRoomAdjacentWrapper()
    {
        if (ScreamHitRoomAdjacent != null)
        {
            ScreamHitRoomAdjacent();
        }
    }

    void RumbleHitPlayerRoom()
    {
        if (RumbleHitPlayerCurrentRoom != null)
        {
            RumbleHitPlayerCurrentRoom();
        }
    }
    RoomManager roomManager;
    // Use this for initialization
    Room currentMonsterRoom;
    bool screaming;

    bool reachedPlayer = false;
    int screamStartIndex;
    Monster monster;

    public ScreamFollowObject screamFollowObject;

    float screamDelay = 2.0f;
    void Awake()
    {
        roomManager = GameObject.Find("Managers").GetComponent<RoomManager>();
        Monster.ReadyToScream += StartScreamWrapper;
    }

    void StartScreamWrapper()
    {
        StartCoroutine(StartScream());
    }

    public IEnumerator StartScream()
    {
        //have something play audio clip here
        //we want the scream object to start from the monster
        GameHandler.screamFollowObject.gameObject.transform.position = GameHandler.monsterGO.transform.position;
        yield return new WaitForSeconds(screamDelay);
        StartCoroutine(SendScream());
    }
    IEnumerator SendScream()
    {
        ScreamBegunWrapper();
        //TODO: for some reason I don't think this is ending after screaming is set to false :( FIX IT

        //this int determines which direction it will go in
        int positiveOrNegative = UnityEngine.Random.Range(0, 2) * 2 - 1;
        //The scream will start out in the room the monster is in
        List<Room> roomList = roomManager.roomList;
        screamStartIndex = roomManager.GetEnemyCurrentRoomIndex();
        int currentRoomIndexOfScream = screamStartIndex;
        int numberOfRoomsWeveBeenThrough = 0;
        //Perhaps have a gentle screenshake that gets more forceful the closer it gets to the player?	
        screaming = true;
        while (screaming)
        {
            GameHandler.screamFollowObject.MoveScreamObjectWrapper(roomList[currentRoomIndexOfScream], positiveOrNegative, 0.5f);
            //	Debug.Log("We're on room " + roomList[roomIndexOfScream].gameObject.name + " which has an index of " + roomIndexOfScream);
            if (currentRoomIndexOfScream == roomManager.GetPlayerCurrentRoomIndex())
            {
                //if we're in the player's room
                ScreamHitPlayerRoom(positiveOrNegative);
                screaming = false;
                reachedPlayer = true;
                //we want it to stop after hitting the player room, failure means the enemy finds the player anyway, success means it's repelled
                // break;
            }
            if (currentRoomIndexOfScream == screamStartIndex && numberOfRoomsWeveBeenThrough == roomManager.numberOfRooms)
            {
                break;
            }

            roomList[currentRoomIndexOfScream].HandleScreamWrapper();

            currentRoomIndexOfScream += (positiveOrNegative);
            numberOfRoomsWeveBeenThrough++;
            if (currentRoomIndexOfScream == -1)
            {
                //if it equals the first index
                currentRoomIndexOfScream = roomList.Count - 1;
                if (currentRoomIndexOfScream == roomManager.GetPlayerCurrentRoomIndex() + 1 && !reachedPlayer)
                {
                    ScreamHitRoomAdjacentWrapper();
                    Debug.Log(currentRoomIndexOfScream + " vs " + roomManager.GetPlayerCurrentRoomIndex());
                    //if this room is adjacent to player
                }
                //swap to go in  the other direction
                //positiveOrNegative*= -1;
            }
            else if (currentRoomIndexOfScream == roomList.Count)
            {
                //if it equals the last index
                currentRoomIndexOfScream = 0;
                if (currentRoomIndexOfScream == roomManager.GetPlayerCurrentRoomIndex() - 1 && !reachedPlayer)
                {
                    ScreamHitRoomAdjacentWrapper();
                    Debug.Log(currentRoomIndexOfScream + " vs " + roomManager.GetPlayerCurrentRoomIndex());

                }
                //positiveOrNegative*= -1;
            }
            else
            {
                if ((currentRoomIndexOfScream == roomManager.GetPlayerCurrentRoomIndex() - 1 || currentRoomIndexOfScream == roomManager.GetPlayerCurrentRoomIndex() + 1) && !reachedPlayer)
                {
                    ScreamHitRoomAdjacentWrapper();
                    Debug.Log(currentRoomIndexOfScream + " vs " + roomManager.GetPlayerCurrentRoomIndex());
                }
            }
            //this will send it to the "left" or the "right" depending
            yield return new WaitForSeconds(0.5f);


        }

    }

    IEnumerator Rumble()
    {

        List<Room> acceptableRooms = GameHandler.monster.GetRoomAtLeastMinRoomsAway(4);

        int positiveOrNegative = UnityEngine.Random.Range(0, 2) * 2 - 1;
        List<Room> roomList = roomManager.roomList;
        int randomRumbleStartLocation = UnityEngine.Random.Range(0, acceptableRooms.Count - 1);
        Room roomToStartIn = acceptableRooms[randomRumbleStartLocation];
        int rumbleStartIndex = roomManager.roomList.IndexOf(roomToStartIn);
        int currentRoomIndexOfRumble = rumbleStartIndex;
        int numberOfRoomsWeveBeenThrough = 0;
        while (true)
        {
            if (currentRoomIndexOfRumble == roomManager.GetPlayerCurrentRoomIndex())
            {
                RumbleHitPlayerRoom();
            }
            if (currentRoomIndexOfRumble == rumbleStartIndex && numberOfRoomsWeveBeenThrough == roomManager.numberOfRooms)
            {
                break;
            }
            roomList[currentRoomIndexOfRumble].HandleRumbleWrapper();
            currentRoomIndexOfRumble += positiveOrNegative;
            numberOfRoomsWeveBeenThrough++;
            if (currentRoomIndexOfRumble == -1)
            {
                //if it equals the first index
                currentRoomIndexOfRumble = roomList.Count - 1;
                //swap to go in  the other direction
            }
            else if (currentRoomIndexOfRumble == roomList.Count)
            {
                //if it equals the last index
                currentRoomIndexOfRumble = 0;
            }
            yield return new WaitForSeconds(0.5f);
        }

    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            // StartCoroutine(SendScream());
        }
    }
}
