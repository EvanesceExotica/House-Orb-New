using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class RoomManager : MonoBehaviour
{

    public Queue<Room> roomsPlayerEntered = new Queue<Room>();
    public List<Room> roomsEnteredList = new List<Room>();
    public List<Room> roomList = new List<Room>();

    public static event Action<Room> PlayerEnteredNewRoom;

    public Dictionary<Room, int> BackwardIndex = new Dictionary<Room, int>();

    public void PlayerEnteredNewRoomWrapper(Room room){
        if(PlayerEnteredNewRoom != null){
            PlayerEnteredNewRoom(room);
        }
    }
    int maxNumberOfRoomsScentLingersIn;
    [SerializeField] Room playerCurrentRoom;
    [SerializeField] Room enemyCurrentRoom;

    public int numberOfRooms;

    public void SetPlayerCurrentRoom(Room room)
    {
        playerCurrentRoom = room;
        PlayerEnteredNewRoomWrapper(room);
    }

    public int GetPlayerCurrentRoomIndex()
    {
        return roomList.IndexOf(playerCurrentRoom);
    }
    public Room GetPlayerCurrentRoom()
    {
        return playerCurrentRoom;
    }

    public void SetEnemyCurrentRoom(Room room)
    {

        enemyCurrentRoom = room;
    }

    public int GetIndexOfRoom(Room room){
        int ourIndex = roomList.IndexOf(room);
        return ourIndex;
    }

    public int DetermineHowCloseRoomIsToPlayer(Room room){

        int closestValue = 0;
        int playerRoomIndex = GetPlayerCurrentRoomIndex(); 

        int calcA = Mathf.Abs(room.ourRoomIndex - playerRoomIndex);
        int calcB = Mathf.Abs(room.ourReverseRoomIndex - playerRoomIndex);

        if(calcA < calcB) {
            closestValue = Mathf.Abs(room.ourRoomIndex);
        }
        else{
            closestValue = Mathf.Abs(BackwardIndex[room]);
        }
        
        int distance = Mathf.Abs(playerRoomIndex - closestValue);
        //this decides the distance of the player
       return distance;
    }

    void SetReversedIndex(){
        for(int i = 0; i < roomList.Count; i++){
            if(i == 0){
                BackwardIndex.Add(roomList[i], 0);
                continue;
            }
            BackwardIndex.Add(roomList[i], i - roomList.Count);
        }
        //   foreach(KeyValuePair<Room, int> entry in BackwardIndex){
        //       Debug.Log(entry.Key +  " vs " + entry.Value);
        //   }
    }
    void AddScentToRoomsEntered(Room room)
    {
        if (roomsPlayerEntered.Count == 4)
        {
            roomsEnteredList.Remove(roomsPlayerEntered.First());
            roomsEnteredList.Add(room);
            Room scentlessRoom = roomsPlayerEntered.Dequeue();
            ScentDispersed(scentlessRoom);
            roomsPlayerEntered.Enqueue(room);
        }
        else
        {
            if (roomsPlayerEntered.Contains(room))
            {
                roomsEnteredList.Remove(roomsPlayerEntered.First());
                roomsEnteredList.Add(room);
                Room scentlessRoom = roomsPlayerEntered.Dequeue();
                ScentDispersed(scentlessRoom);
                roomsPlayerEntered.Enqueue(room);
            }
            else
            {
                roomsPlayerEntered.Enqueue(room);
                roomsEnteredList.Add(room);
            }
        }



    }
    public void ClearAllRoomScent(){
        foreach(Room room in roomsPlayerEntered){
            ScentDispersed(room);
        }
        roomsPlayerEntered.Clear();
        roomsEnteredList.Clear();

    }

    void ScentDispersed(Room room)
    {
        room.StopScentParticleSystem();
    }

    public int GetEnemyCurrentRoomIndex()
    {

        return roomList.IndexOf(enemyCurrentRoom);
    }

    public Room GetEnemyCurrentRoom(){
        return enemyCurrentRoom;
    }

    public int GetReverseIndexOfRoom(Room room){
        int reverseIndex = BackwardIndex[room];
        return reverseIndex;
    }
    //TODO: You have to make sure this list grabs the objects in order of hwo they are in the scene -- maybe have a room instantiator
    void Awake()
    {
        roomList = transform.GetComponentsInChildren<Room>().ToList();
        SetReversedIndex();
        //	levelRooms = FindObjectsOfType<Room>().ToList();
        foreach (Room room in roomList)
        {
            room.PlayerEnteredRoom += SetPlayerCurrentRoom;
            room.PlayerEnteredRoom += AddScentToRoomsEntered;
            room.EnemyEntered += SetEnemyCurrentRoom;
            SetAdjacentRooms(room);
        }
        numberOfRooms = roomList.Count;
    }


    void SetAdjacentRooms(Room room){
        List<Room> adjacentRooms = new List<Room>();
        if((roomList.IndexOf(room) - 1) < 0){
            //if the index before is less than zero,
            Room roomToAdd = roomList.Last();
            adjacentRooms.Add(roomToAdd);
        }
        else if((roomList.IndexOf(room) - 1) >= 0){
            Room roomToAdd = roomList[roomList.IndexOf(room) - 1];
            adjacentRooms.Add(roomToAdd);
        }
        if((roomList.IndexOf(room) + 1) > (roomList.Count - 1)){
            //if the index is out of range and goes over the last room in the list

            Room roomToAdd = roomList[0];
            adjacentRooms.Add(roomToAdd);
        }
        else{
            Room roomToAdd = roomList[roomList.IndexOf(room) + 1];
            adjacentRooms.Add(roomToAdd);
        }
        room.adjacentRooms = adjacentRooms;  
    }
    // Use this for initialization
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearAllRoomScent();
        }
    }
}
