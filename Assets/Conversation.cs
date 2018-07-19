using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
public class Conversation : MonoBehaviour
{
	AudioSource blipSource;

    CanvasGroup FadeToBlackGroup;

    TextMeshProUGUI memoryText;

    int conversationIteration = 0;

    Speech currentSpeech;

    Memory currentMemory;

    public static event Action FinishedDisplayingMemory;

    public void FinishedDisplayingMemoryWrapper()
    {
        FadeOut(this);
        if (FinishedDisplayingMemory != null)
        {
            FinishedDisplayingMemory();
        }
    }

    float buffer = 2.0f;
    // Use this for initialization
    void Awake()
    {
		blipSource = GetComponent<AudioSource>();
        Memory.LookingAtMemory += FadeIn;
        // Memory.StoppedLookingAtMemory += FadeOut;
        FadeToBlackGroup = GetComponent<CanvasGroup>();
        memoryText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void FadeIn(MonoBehaviour mono)
    {
        FadeToBlackGroup.DOFade(1.0f, 1.0f).SetUpdate(true);
        if (mono.GetType() == typeof(Memory))
        {
            currentMemory = (Memory)mono;
        }
        currentSpeech = currentMemory.inMemoryConversation;
        StartCoroutine(IterateThroughMemory());
    }

    void FadeOut(MonoBehaviour mono)
    {

        FadeToBlackGroup.DOFade(0, 2.0f).SetUpdate(true);
    }

    public IEnumerator IterateThroughMemory()
    {
        yield return new WaitForSecondsRealtime(buffer);
        while (conversationIteration < currentMemory.inMemoryConversation.textChoices.Count)
        {

            yield return StartCoroutine(TypeMemoryText(currentSpeech, memoryText, conversationIteration));
            conversationIteration++;
            yield return new WaitForSecondsRealtime(buffer);
            if (conversationIteration != currentMemory.inMemoryConversation.textChoices.Count - 1)
            {
                memoryText.text = " ";
            }
        }
        FinishedDisplayingMemoryWrapper();
    }

    public IEnumerator TypeMemoryText(Speech speech, TextMeshProUGUI textObject, int speechIndex)
    {
        string speechText = speech.GrabTextChoiceAtIndex(speechIndex);
        speech.SetSpeakerToIndex(speechIndex);
        textObject.color = speech.bubbleColor;
        float delayTime = 0.1f;
        foreach (char letter in speechText)
        {
            if (letter == '^')
            {
                //we put this character before ellipses to add pause
                delayTime = 0.3f;
            }
            else
            {
                delayTime = 0.07f;
                textObject.text += letter;
            }
			blipSource.PlayOneShot(speech.speechBlip);
            yield return new WaitForSecondsRealtime(delayTime/*speech.textSpeed*/);
        }
    }



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
