using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;
using DG.Tweening;
public class ScreenGlitch : MonoBehaviour
{

    [SerializeField] AnalogGlitch glitchEffect;
    // Use this for initialization
    Room currentRoom;
    void Awake()
    {
        glitchEffect = Camera.main.GetComponent<AnalogGlitch>();
        RoomManager.PlayerEnteredNewRoom += SetCurrentRoom;
    }

    void SetCurrentRoom(Room room)
    {
        //the player entered a new room, so let's reset it 
        if (currentRoom != null)
        {
            currentRoom.EnemyEnteredAdjacentRoom -= this.ActivateGlitchEffect;
            currentRoom.EnemyExitedAdjacentRoom -= DeactivateGlitchEffect;
			//if the player moved to a different room, we want to turn the glitch effect off
			DeactivateGlitchEffect(currentRoom);
        }
        currentRoom = room;
        currentRoom.EnemyEnteredAdjacentRoom += ActivateGlitchEffect;
        currentRoom.EnemyExitedAdjacentRoom += DeactivateGlitchEffect;
    }

    public void ActivateGlitchEffect(Room room)
    {
		StartCoroutine(GlitchEffect());
    }

    public void DeactivateGlitchEffect(Room room)
    {
		if(GameHandler.roomManager.GetPlayerCurrentRoom().enemyLocation == Room.EnemyStatus.OutOfRoom){
			cancel = true;
		}

    }

	bool cancel = false;
    float durationMax = 3.0f;
    float durationMin = 2.0f;

    float incrementMin = 2.0f;
    float incrementMax = 4.0f;
    public IEnumerator GlitchEffect()
    {
        float startTime = Time.time;
        float duration = Random.Range(durationMin, durationMax);
        float increment = Random.Range(incrementMin, incrementMax);
        float elapsedTime = 0;
        while (elapsedTime < 10)
        {
			if(cancel){
                cancel = false;
				yield break;
			}
            glitchEffect.scanLineJitter = 0.2f;
            glitchEffect.colorDrift = 0.3f;
            //glitchEffect.scanLineJitter = Mathf.Lerp(glitchEffect.scanLineJitter, 0.2f, elapsedTime);
            //glitchEffect.colorDrift = Mathf.Lerp(glitchEffect.colorDrift, 0.3f, elapsedTime);
            elapsedTime += Time.deltaTime;
            duration = Random.Range(durationMin, durationMax);
            increment = Random.Range(incrementMin, incrementMax);
			yield return StartCoroutine(LerpBackToNormal(increment, duration));
            //yield return new WaitForSeconds(increment);

        }
    }

    IEnumerator LerpBackToNormal(float increment, float duration)
    {
		yield return new WaitForSeconds(duration);
        float elapsedTime = 0;
        while (elapsedTime < increment)
        {
            glitchEffect.scanLineJitter = Mathf.Lerp(glitchEffect.scanLineJitter, 0, elapsedTime / increment);
            glitchEffect.colorDrift = Mathf.Lerp(glitchEffect.colorDrift, 0, elapsedTime / increment);
            elapsedTime += Time.deltaTime;
			yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(GlitchEffect());
        }
    }
}
