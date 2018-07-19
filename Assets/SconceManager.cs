using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class SconceManager : MonoBehaviour
{

    [SerializeField] int numberOfSconcesOnLevel;

   [SerializeField] bool allRevealed = false;

   [SerializeField] bool allLit = false;
    int TallySconceNumber()
    {
        int talliedSconces = revealedSconces.Count + unrevealedSconces.Count;
        return talliedSconces;
    }
    public List<Sconce> revealedSconces = new List<Sconce>();
    public List<HiddenSconce> unrevealedSconces = new List<HiddenSconce>();

    public List<Sconce> litSconces = new List<Sconce>();
    public static event Action AllSconcesLit;

    void AddLitSconceToList(MonoBehaviour sconceObject)
    {
        Sconce sconce = (Sconce)sconceObject;
        if (!litSconces.Contains(sconce))
        {
            litSconces.Add(sconce);
        }
        if (allRevealed)
        {
            if (CheckIfAllSconcesLit())
            {
                allLit = true;
                //if all of the sconces are lit
                AllSconcesLitWrapper();
            }
        }
    }



    void RemoveLitSconcesFromList(MonoBehaviour sconceObject)
    {
        Sconce sconce = (Sconce)sconceObject;
        if (litSconces.Contains(sconce))
        {
            litSconces.Remove(sconce);
        }
    }

    void AddRevealedSconceToList(Sconce sconce)
    {
        if (!revealedSconces.Contains(sconce))
        {
            revealedSconces.Add(sconce);
        }
    }

    void RemoveHiddenSconceAndAddRevealed(Sconce sconce, HiddenSconce hiddenScone)
    {
        if (unrevealedSconces.Contains(hiddenScone))
        {
            unrevealedSconces.Remove(hiddenScone);
        }
        if (!revealedSconces.Contains(sconce))
        {
            revealedSconces.Add(sconce);
            sconce.Extinguished += RemoveLitSconcesFromList;
        }
        if (CheckIfAllSconcesConvertedToRevealed())
        {
            allSconces.AddRange(revealedSconces);
            allRevealed = true;
            AllSconcesRevealedWrapper();
        }
    }
    public void AllSconcesLitWrapper()
    {
        if (AllSconcesLit != null)
        {
            AllSconcesLit();
        }
    }

    public void AllSconcesRevealedWrapper()
    {
        if (AllSconcesRevealed != null)
        {
            AllSconcesRevealed();
        }
    }

    bool CheckIfAllSconcesConvertedToRevealed()
    {
        bool allConverted = false;
        if (unrevealedSconces.Count == 0 && revealedSconces.Count == numberOfSconcesOnLevel)
        {
            allConverted = true;
        }
        return allConverted;

    }
    bool CheckIfAllSconcesLit()
    {
        bool containsAll = true;
        for (int i = 0; i < 1; i++)
        {
            if (!litSconces.Contains(allSconces[i]))
            {
                containsAll = false;
            }
        }
        return containsAll;
    }

    public static event Action AllSconcesRevealed;
    public List<Sconce> allSconces = new List<Sconce>();

    void Awake()
    {
        revealedSconces = GameObject.FindObjectsOfType<Sconce>().ToList();
        unrevealedSconces = GameObject.FindObjectsOfType<HiddenSconce>().ToList();

        Sconce.OrbInSconce += AddLitSconceToList;
        foreach (Sconce sconce in revealedSconces)
        {
            // sconce.Revealed += AddRevealedSconceToList;
            sconce.Extinguished += RemoveLitSconcesFromList;
            // if(sconce.fillStatus == Sconce.Status.Hidden){
            //     sconce.gameObject.SetActive(false);
            // }

        }
        foreach (HiddenSconce hiddenSconce in unrevealedSconces)
        {
            hiddenSconce.NewSconceRevealed += RemoveHiddenSconceAndAddRevealed;

        }
        numberOfSconcesOnLevel = TallySconceNumber();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
