using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class Character : ScriptableObject{



	public string charName;
	public List<Emotion> keys = new List<Emotion>();
	public List<Sprite> values = new List<Sprite>();

   void Awake(){
		//foreach(Emotion nm in keys){
		//    foreach(Texture2D sp in values){
		//    charPictures.Add(nm, sp);

		//    }
		//}

	}
	

	
	public Dictionary<Emotion, Sprite> charPictures;
	//public Dictionary<string, CharPic> charPictures;
	public enum Emotion
	{
		Idle,
		Serious,
		Happy,
		Estatic,
		Sad,
		Angry,
		Pouty,
		Awe,
		Afraid,
		Panicked,
		Enamoured,
		Turned_On,
		Embarrassed,
		Shy,
		Amused,
		Laughing,
		Cold,
		Tsundere,
		Furious,
		Heroic_BSOD,
		Sleeping,
		Shocked,
		In_Pain,
		Drunk,
		Poisoned,
		Burnt,
		Electrocuted,
		Freezing,
		Sweating
	};
	public Color fontColor;
	//public enum Emotion
	//{
	//    Idle,
	//    Serious,
	//    Happy,
	//    Estatic,
	//    Sad,
	//    Angry,
	//    Pouty,
	//    Afraid,
	//    Panicked,
	//    Enamoured,
	//    Turned_On,
	//    Embarrassed,
	//    Shy,
	//    Amused,
	//    Laughing,
	//    Cold,
	//    Tsundere,
	//    Furious,
	//    Heroic_BSOD,
	//    Sleeping,
	//    Shocked,
	//    In_Pain,
	//    Drunk,
	//    Poisoned,
	//    Burnt,
	//    Electrocuted,
	//    Freezing,
	//    Sweating



	//};
	
}
