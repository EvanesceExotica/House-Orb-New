using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HidingPlaceLight : MonoBehaviour
{

    public bool horizontal;
    Color defaultColor;
    float defaultIntensity;

	float blendedIntensity;
    public float monsterInRoomIntensity;

    public Color softVioletColor;
    public Color blendedColor;
    Light crackLight;
    [SerializeField] Room parentRoom;
    void Awake()
    {
        if (horizontal)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        crackLight = GetComponent<Light>();
        defaultColor = crackLight.color;
        blendedColor = (softVioletColor + defaultColor) / 2;
		blendedIntensity = (monsterInRoomIntensity + defaultIntensity)/2;
        crackLight.enabled = false;
        HidingSpace.PlayerHiding += EnableCrackLight;
        HidingSpace.PlayerNoLongerHiding += DisableCrackLight;
        StartCoroutine(InitializeAdjacentRooms());
    }

    public IEnumerator InitializeAdjacentRooms()
    {
        yield return new WaitForSeconds(0.5f);
        parentRoom = GetComponentInParent<HidingSpace>().parentRoom;
        parentRoom.EnemyEntered += FadeLightToPurple;
        parentRoom.EnemyExited += FadeLightToBlended;
        parentRoom.EnemyEnteredAdjacentRoom += FadeLightToBlended;
        parentRoom.EnemyExitedAdjacentRoom += CheckEnemyLocation;

    }

    void EnableCrackLight(MonoBehaviour ourObject)
    {

        crackLight.enabled = true;
    }

    void DisableCrackLight(MonoBehaviour ourObject)
    {
        crackLight.enabled = false;
    }
    void FadeLightToBlended(Room room)
    {
		crackLight.DOIntensity(blendedIntensity, 2.0f);
        crackLight.DOColor(blendedColor, 2.0f);

    }

    void CheckEnemyLocation(Room room)
    {
        //if the enemy leaves the adjacent room but goes to another room rather than entering the player's current room
        //the "enemyEnteredRoom" action hsould take care of the fading to brighter purple
        if (parentRoom.enemyLocation == Room.EnemyStatus.OutOfRoom)
        {
            FadeLightToWhite();

        }
    }

    void FadeLightToPurple(Room room)
    {
		crackLight.DOIntensity(monsterInRoomIntensity, 2.0f);
        crackLight.DOColor(softVioletColor, 2.0f);
    }

    void FadeLightToWhite()
    {
		crackLight.DOIntensity(defaultIntensity, 2.0f);
        crackLight.DOColor(defaultColor, 2.0f);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            FadeLightToBlended(parentRoom);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            FadeLightToPurple(parentRoom);
        }

    }
}
