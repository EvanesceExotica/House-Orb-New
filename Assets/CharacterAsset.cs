using UnityEngine;
using System.Collections;
using UnityEditor;

public class CharacterAsset : MonoBehaviour
{
    [MenuItem("Assets/Create/Character")]
    public static void CreateAsset()
    {
        CustomAssetUtility.CreateAsset<Character>();
    }
}
