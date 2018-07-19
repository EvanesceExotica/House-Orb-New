using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogueElements {

   // public enum Characters { David, Megan };
    public Character character;
    //public List<Character> characters;
    //This is the character "enum";
   // public enum AvatarPos { left, right };
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
  //  public Characters Character;
    //public AvatarPos CharacterPosition;
    public Emotion emo;
    public Sprite CharacterPic;
    public string DialogueText;
  //  public GUIStyle DialogueTextStyle;
    public float TextPlayBackSpeed;
    public float startDelay;
    public Color fontColor;
   // public AudioClip PlayBackSoundFile;

    void Awake()
    {
        //for (int i = 0; i < character.keys.Count; i++) {
        //    if (character.keys[i].ToString() == emo.ToString())
        //    {
        //        CharacterPic = character.values[i];
        //    }
        //}

    }

    void OnEnable(){
        //for (int i = 0; i < character.keys.Count; i++)
        //{
        //    if(character.keys[i].ToString() == emo.ToString()){
        //        CharacterPic = character.values[i];
        //    }
        //}
    }
}
