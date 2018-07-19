using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class InteractPrompt : MonoBehaviour
{

   public List<GameObject> objectPromptsDisplaying = new List<GameObject>();
    CanvasGroup ourCanvasGroup;
    TextMeshProUGUI ourText;

    bool displaying;
    void Awake()
    {
        ourCanvasGroup = GetComponent<CanvasGroup>();
        ourText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void FadeIn()
    {
        ourCanvasGroup.DOFade(1, 1.0f);
    }

    void FadeOut()
    {

        ourCanvasGroup.DOFade(0, 1.0f);
    }

    public void ChangePrompt(string promptToDisplay, GameObject gameObject)
    {
    }
    public void DisplayPrompt(string promptToDisplay, GameObject go)
    {
        if (!displaying)
        {
            objectPromptsDisplaying.Add(go);
            displaying = true;
            transform.parent = go.transform;
            transform.position = new Vector2(go.transform.position.x, go.transform.position.y + 1.0f);
            ourText.text = promptToDisplay;
            FadeIn();
        }
        else
        {
            ourText.text = promptToDisplay;
        }
    }

    public void HidePrompt(GameObject go)
    {
        if (objectPromptsDisplaying.Contains(go))
        {
            objectPromptsDisplaying.Remove(go);
        }
        if (objectPromptsDisplaying.Count == 0)
        {
            FadeOut();
            ourText.text = " ";
            transform.parent = GameHandler.playerGO.transform;
        }
    }

}
