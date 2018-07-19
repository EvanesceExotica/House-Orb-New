using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
public class SwitchCameras : MonoBehaviour {

	// Use this for initialization
	private LayerMask NormalLayerMask;
	public LayerMask HidingSpaceLayerMask;

	public LayerMask OrbFollowLayerMask;

	void Awake(){
		NormalLayerMask = GameHandler.mainCamera.cullingMask; 
		HidingSpace.PlayerHiding += ChangeCameraToShowHidingPlace;
		HidingSpace.PlayerNoLongerHiding += ChangeCameraToDefault;
		ReturnPlayerToLastSconce.ReturningToLastSconceWithPlayer += ChangeCameraToShowOrbOnly;
		ReturnPlayerToLastSconce.ArrivedAtLastSconceWithPlayer += ChangeCameraToDefault;
	}
	void ChangeCameraToShowHidingPlace(MonoBehaviour ourObject){
		GameHandler.mainCamera.cullingMask = HidingSpaceLayerMask;
	}

	void ChangeCameraToShowOrbOnly(MonoBehaviour ourObject){
		GameHandler.mainCamera.cullingMask = OrbFollowLayerMask; 

	}

	void ChangeCameraToDefault(MonoBehaviour ourObject){
		GameHandler.mainCamera.cullingMask = NormalLayerMask;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.X)){

		}
		
	}
}
