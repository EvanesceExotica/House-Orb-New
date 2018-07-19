using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Speech", menuName = "DialogeOptions/Speech", order = 1)]
public class Speech : ScriptableObject {

	public float textSpeed;

	public AudioClip speechBlip;
	public Color bubbleColor;
	public Color playerColor;
	public Color orbColor;
	public Color friendColor;
	public enum OurSpeaker{
		player,
		orb,
		friend
	}
	public OurSpeaker speaker;

	public List<OurSpeaker> speakers;

	public List<string> textChoices;
	public string text;
	void Start(){
	}
	 
	void Awake(){
		playerColor = new Color32(182, 102, 210, 255);
		orbColor = new Color32(64, 224, 208, 255);
		friendColor = new Color32(240,230,140, 255);
		SetTextColor();

	}
	public void SetTextColor(){
		if(speaker == OurSpeaker.player){
			bubbleColor = playerColor;
		}
		else if(speaker == OurSpeaker.orb){
			bubbleColor = orbColor;
		}
		else if(speaker == OurSpeaker.friend){
			bubbleColor = friendColor;
		}
	}

	public void SetSpeakerToIndex(int index){
		speaker = speakers[index];
		SetTextColor();
	}
	public string GrabTextChoiceAtIndex(int index){
		//use this if the speech is a conversation	
		if(index < 0 || index > textChoices.Count - 1){
			return " ";
		}
		else{
			return textChoices[index];
		}

	}
	public string GrabRandomTextChoice(){
		//use this if you wish to pull random line from a group of options
		string ourTextChoice = null;
		int randomIndex = UnityEngine.Random.Range(0, textChoices.Count);
		ourTextChoice = textChoices[randomIndex];
		return ourTextChoice;
	}
	
}
