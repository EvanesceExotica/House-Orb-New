using UnityEngine;
using UnityEditor;


[InitializeOnLoad]
public class AutosaveOnRun: ScriptableObject
{
	static AutosaveOnRun()
	{
		EditorApplication.playmodeStateChanged = () =>
		{
			if(EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
			{
				Debug.Log("Auto-Saving scene before entering Play mode: " + EditorApplication.currentScene);
				
				EditorApplication.SaveScene();
				AssetDatabase.SaveAssets();
			}
		};
	}
}